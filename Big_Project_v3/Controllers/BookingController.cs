using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Big_Project_v3.Models;
using System.Diagnostics;
using System.Globalization;
using Big_Project_v3.ViewModels;

namespace Big_Project_v3.Controllers
{
    [Route("Booking")]
    public class BookingController : Controller
    {
        private readonly ITableDbContext _context;

        public BookingController(ITableDbContext context)
        {
            _context = context;
        }

        // GET 方法: 用於載入訂位頁面//
        [HttpGet]
        [Route("BookingPage/{RestaurantId?}")]
        public async Task<IActionResult> BookingPage(int? RestaurantId)
        {

            if (!RestaurantId.HasValue)
            {
                return NotFound(); // 如果 RestaurantId 是 null，返回 404
            }

            // 從資料庫檢索餐廳資訊
            var restaurant = await _context.Restaurants
                                           .Where(r => r.RestaurantId == RestaurantId.Value)
                                           .Select(r => new { r.RestaurantId, r.IsReservationOpen })
                                           .FirstOrDefaultAsync();

            if (restaurant == null)
            {
                return NotFound(); // 如果餐廳不存在，返回 404
            }

            // 確認餐廳是否開放訂位
            if (!restaurant.IsReservationOpen)
            {
                ViewBag.Message = "今日不開放訂位";
                return View("ReservationClosed");
            }

            // 使用 GetInt32 讀取 UserId
            var UserId = HttpContext.Session.GetInt32("UserId");

            if (UserId.HasValue)
            {
                var user = await _context.Users
                                         .Where(u => u.UserId == UserId.Value)
                                         .Select(u => new
                                         {
                                             u.Name,
                                             u.ContactPhone,
                                             u.ContactEmail
                                         })
                                         .FirstOrDefaultAsync();

                if (user != null)
                {
                    ViewBag.Name = user.Name;
                    ViewBag.ContactPhone = user.ContactPhone;
                    ViewBag.ContactEmail = user.ContactEmail;
                }
            }

            if (!UserId.HasValue)
            {
                // 如果未登入，重定向到登入頁面
                return RedirectToAction("Login", "User");
            }

            var photoUrl = await _context.Photos
                                          .Where(p => p.RestaurantId == RestaurantId.Value)
                                          .Select(p => p.PhotoUrl)
                                          .FirstOrDefaultAsync();

            var viewModel = await GetBookingPageViewModel(restaurant.RestaurantId);
            viewModel.PhotoUrl = photoUrl;
            viewModel.UserId = UserId;

            return View(viewModel);
        }

        // POST 方法: 提交訂位資料
        [HttpPost]
        [Route("BookingPage")]
        public async Task<IActionResult> BookingPagePost(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                var totalGuests = (reservation.NumAdults ?? 0) + (reservation.NumChildren ?? 0);
                var availability = await _context.RestaurantAvailabilities
                    .FirstOrDefaultAsync(ra => ra.RestaurantId == reservation.RestaurantId);

                if (availability == null || availability.AvailableSeats == null)
                {
                    ModelState.AddModelError("", "無法找到餐廳的座位資訊");
                    return View("BookingPage", await GetBookingPageViewModel(reservation.RestaurantId));
                }

                if (availability.AvailableSeats < totalGuests)
                {
                    ModelState.AddModelError("", "超出目前可用座位數量，請重新選擇人數或時段");
                    return View("BookingPage", await GetBookingPageViewModel(reservation.RestaurantId));
                }

                availability.AvailableSeats -= totalGuests;
                await _context.SaveChangesAsync(); // 儲存座位數更新

                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                TempData["ReservationDetails"] = System.Text.Json.JsonSerializer.Serialize(new Dictionary<string, object>
                {
                    { "RestaurantName", await _context.Restaurants.Where(r => r.RestaurantId == reservation.RestaurantId).Select(r => r.Name).FirstOrDefaultAsync() ?? "未知餐廳" },
                    { "Date", reservation.ReservationDate?.ToString("yyyy/MM/dd") ?? "未指定日期" },
                    { "Time", reservation.ReservationTime?.ToString() ?? "未指定時間" },
                    { "NumAdults", reservation.NumAdults ?? 0 },
                    { "NumChildren", reservation.NumChildren ?? 0 },
                    { "BookerName", reservation.BookerName ?? "未提供姓名" },
                    { "BookerPhone", reservation.BookerPhone ?? "未提供電話" },
                    { "BookerEmail", reservation.BookerEmail ?? "未提供Email" },
                    { "SpecialRequests", reservation.SpecialRequests ?? "無備註" }
                });
                Console.WriteLine($"TempData content: {TempData["ReservationDetails"]}");  // 確認 TempData 的內容

                return RedirectToAction("BookingSuccess");
            }

            //return View(await GetBookingPageViewModel(reservation.RestaurantId));
            return View("BookingPage", await GetBookingPageViewModel(reservation.RestaurantId));

        }

        // 訂位成功頁面
        [HttpGet]
        [Route("BookingSuccess")]
        public IActionResult BookingSuccess()
        {
            var reservationDetailsJson = TempData["ReservationDetails"] as string;
            if (reservationDetailsJson == null)
            {
                return RedirectToAction("BookingPage");
            }

            var reservationDetails = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(reservationDetailsJson);

            if (reservationDetails!.ContainsKey("Date") && DateTime.TryParse(reservationDetails["Date"].ToString(), out var reservationDate))
            {
                var formattedDate = reservationDate.ToString("yyyy-MM-dd (ddd)", new CultureInfo("zh-TW"));
                reservationDetails["Date"] = formattedDate;
            }

            return View(reservationDetails);
        }

        // 輔助方法: 載入 ViewModel
        private async Task<BookingPageViewModel> GetBookingPageViewModel(int RestaurantId)
        {
            var reservations = await _context.Reservations.ToListAsync();
            var settings = await _context.ReservationControlSettings.ToListAsync();
            var restaurantAvailabilities = await _context.RestaurantAvailabilities.ToListAsync();
            var restaurantUsers = await _context.RestaurantUsers.ToListAsync();

            var reviews = await _context.Reviews
                .Where(r => r.RestaurantId == RestaurantId)
                .Select(r => new ReviewViewModel
                {
                    ReviewID = r.ReviewId,
                    RestaurantID = (int)r.RestaurantId,
                    ReviewText = r.ReviewText ?? "無評論",
                    Rating = r.Rating ?? 0,
                    IsReviewLocked = r.IsReviewLocked ?? "No"
                })
                .ToListAsync();

            var users = await _context.Users.ToListAsync();
            var photos = await _context.Photos.ToListAsync();
            var announcements = await _context.Announcements.ToListAsync();
            var favorites = await _context.Favorites.ToListAsync();
            var passwordResetRequests = await _context.PasswordResetRequests.ToListAsync();

            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantId == RestaurantId);
            var restaurants = RestaurantId > 0
                ? await _context.Restaurants.Where(r => r.RestaurantId == RestaurantId).ToListAsync()
                : new List<Restaurant>();

            var viewModel = new BookingPageViewModel
            {
                Reservations = reservations,
                ReservationControlSettings = settings,
                Restaurants = restaurants,
                RestaurantAvailabilities = restaurantAvailabilities,
                RestaurantUsers = restaurantUsers,
                Reviews = reviews,
                Users = users,
                Photos = photos,
                Announcements = announcements,
                Favorites = favorites,
                PasswordResetRequests = passwordResetRequests
            };

            if (restaurant != null)
            {
                ViewData["RestaurantName"] = restaurant.Name;
                ViewData["BusinessHoursStart"] = restaurant.BusinessHoursStart?.ToString("HH:mm") ?? "09:30";
                ViewData["LastCheckInTime"] = restaurant.LastCheckInTime?.ToString("HH:mm") ?? "23:30";
            }
            else
            {
                ViewData["RestaurantName"] = "無法找到餐廳";
                ViewData["BusinessHoursStart"] = "09:30";
                ViewData["LastCheckInTime"] = "23:30";
            }

            return viewModel;
        }
    }
}