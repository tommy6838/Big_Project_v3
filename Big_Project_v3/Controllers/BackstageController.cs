using Big_Project_v3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Big_Project_v3.Controllers
{
	public class BackstageController : Controller
	{
		private readonly ITableDbContext _context;

		public BackstageController(ITableDbContext context)
		{
			_context = context;
		}

		// GET: (查詢新資料) 在控制器中加入 API，返回目前的新資料數量。
		[HttpGet]
		public async Task<IActionResult> GetNewMessagesCount()
		{
            int LogIn_restaurantId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantId"));

            // 查詢新訊息數量
            int newMessagesCount = await _context.Reservations
				.Where(r => r.RestaurantId == LogIn_restaurantId && r.ReservationStatus == "未確認")
				.CountAsync();

            return Json(new { count = newMessagesCount });
		}

		// GET: 首頁/List
		public async Task<IActionResult> Home()
		{
			DateOnly today = DateOnly.FromDateTime(DateTime.Now); // 取得今天的日期（不包含時間）

			int? LogIn_restaurantUserId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantUserId"));
			int LogIn_restaurantId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantId"));

			if (LogIn_restaurantUserId == 0)
			{
				// 跳轉到登入頁面
				return RedirectToAction("LogIn", "Backstage");
			}

			var data = await _context.Restaurants
				.Where(r => r.RestaurantId == LogIn_restaurantId)  // 篩選餐廳
				.Include(r => r.Reservations)  // 包含訂位資料
				.Include(r => r.Reviews)       // 包含評論資料
				.ToListAsync();

			var priceString = data[0].PriceRange; // 找到餐廳價錢

			// 移除 "NT$" 和 "," 字元
			string numericString = priceString!.Replace("NT$", "").Replace(",", "");

			// 直接轉換為整數
			int price = int.Parse(numericString);

			ViewBag.priceRange = price; // 餐廳價錢

			// 篩選出今天的訂位資料
			var todayReservations = data
				.SelectMany(r => r.Reservations)  // 將所有餐廳的訂位資料展平
				.Where(res => res.ReservationDate == today) // 篩選日期為今天的訂位
				.ToList();

			ViewBag.todayReservations = todayReservations.Count; // 今天訂位數量

			// 篩選出今天訂位的大人數量
			var todayNumAdults = data
				.SelectMany(r => r.Reservations)  // 將所有餐廳的訂位資料展平
				.Where(res => res.ReservationDate == today) // 篩選日期為今天的訂位
				.Sum(res => res.NumAdults); // 計算今天所有訂位的大人數量總和

			ViewBag.todayNumAdults = todayNumAdults; // 今天訂位的大人數量總和

			// 篩選出今天訂位的小孩數量
			var todayNumChildren = data
				.SelectMany(r => r.Reservations)  // 將所有餐廳的訂位資料展平
				.Where(res => res.ReservationDate == today) // 篩選日期為今天的訂位
				.Sum(res => res.NumChildren); // 計算今天所有訂位的小孩數量總和

			ViewBag.todayNumChildren = todayNumChildren; // 今天訂位的小孩數量總和

			// 篩選出今天的評論資料
			var todayReviews = data
				.SelectMany(r => r.Reviews)  // 將所有餐廳的評論資料展平
				.Where(res => res.ReviewDate == today) // 篩選日期為今天的評論
				.ToList();

			ViewBag.todayReviews = todayReviews.Count; // 今天評論數量

            // 定義昨天的日期
            var yesterday = today.AddDays(-1);

            // 篩選出昨天的訂位資料
            var yesterdayReservations = data
                .SelectMany(r => r.Reservations)  // 將所有餐廳的訂位資料展平
                .Where(res => res.ReservationDate == yesterday) // 篩選日期為昨天的訂位
                .ToList();

            ViewBag.yesterdayReservations = yesterdayReservations.Count; // 昨天訂位數量

            // 篩選出昨天訂位的大人數量
            var yesterdayNumAdults = data
                .SelectMany(r => r.Reservations)  // 將所有餐廳的訂位資料展平
                .Where(res => res.ReservationDate == yesterday) // 篩選日期為昨天的訂位
                .Sum(res => res.NumAdults); // 計算昨天所有訂位的大人數量總和

            ViewBag.yesterdayNumAdults = yesterdayNumAdults; // 昨天訂位的大人數量總和

            // 篩選出昨天訂位的小孩數量
            var yesterdayNumChildren = data
                .SelectMany(r => r.Reservations)  // 將所有餐廳的訂位資料展平
                .Where(res => res.ReservationDate == yesterday) // 篩選日期為昨天的訂位
                .Sum(res => res.NumChildren); // 計算昨天所有訂位的小孩數量總和

            ViewBag.yesterdayNumChildren = yesterdayNumChildren; // 昨天訂位的小孩數量總和

            // 篩選出昨天的評論資料
            var yesterdayReviews = data
                .SelectMany(r => r.Reviews)  // 將所有餐廳的評論資料展平
                .Where(res => res.ReviewDate == yesterday) // 篩選日期為昨天的評論
                .ToList();

            ViewBag.yesterdayReviews = yesterdayReviews.Count; // 昨天評論數量

            return View(data);
		}

        public IActionResult Restaurant(string? action)
        {
            string LogIn_managerPosition = HttpContext.Session.GetString("ManagerPosition")!.ToString();
            if (LogIn_managerPosition == "財務")
            {
                // 跳轉到首頁
                return RedirectToAction("Home", "Backstage");
            }

            // 從 Session 中獲取 RestaurantId
            var restaurantIdString = HttpContext.Session.GetString("RestaurantId");
            if (string.IsNullOrEmpty(restaurantIdString) || !int.TryParse(restaurantIdString, out int restaurantId))
            {
                // 如果沒有找到 RestaurantId 或無法轉換，則重定向到登入頁面或顯示錯誤
                return RedirectToAction("LogIn", "Backstage"); // 導回登入頁面
            }

            // 根據 action 參數來設定哪個 Partial View 要顯示
            if (action == "RestaurantInfo")
            {
                ViewBag.LoadPartial = "PartialRestaurantInfo";
            }
            else if (action == "announcement")
            {
                ViewBag.LoadPartial = "PartialAnnouncement";
            }

            return View();
        }

        //-----------------------Restaurant的公告partial------------------------------
        // 公告管理的 Partial View
        public IActionResult PartialAnnouncement()
        {
            var restaurantIdString = HttpContext.Session.GetString("RestaurantId");
            if (string.IsNullOrEmpty(restaurantIdString) || !int.TryParse(restaurantIdString, out int restaurantId))
            {
                return RedirectToAction("LogIn", "Backstage");
            }

            var announcement = _context.Announcements.FirstOrDefault(a => a.RestaurantId == restaurantId);
            return PartialView("BO_PartialView/PartialAnnouncement", announcement);
        }

        //-----------------------Restaurant的公告上傳------------------------------

        [HttpPost]
        public async Task<IActionResult> UpdateAnnouncement([FromBody] Announcement announcement)
        {
            if (announcement == null)
            {
                Console.WriteLine("announcement 為 null");
                return BadRequest("無法解析公告數據");
            }

            // 確認所有的屬性是否正確接收
            Console.WriteLine($"Received AnnouncementId: {announcement.AnnouncementId}");
            Console.WriteLine($"Received Title: {announcement.Title}");
            Console.WriteLine($"Received Content: {announcement.Content}");
            Console.WriteLine($"Received AnnouncementDate: {announcement.AnnouncementDate}");
            Console.WriteLine($"Received StartDate: {announcement.StartDate}");
            Console.WriteLine($"Received EndDate: {announcement.EndDate}");

            var restaurantIdString = HttpContext.Session.GetString("RestaurantId");
            if (string.IsNullOrEmpty(restaurantIdString) || !int.TryParse(restaurantIdString, out int restaurantId))
            {
                Console.WriteLine("RestaurantId 無法取得或轉換失敗");
                return RedirectToAction("LogIn", "Backstage");
            }

            var existingAnnouncement = await _context.Announcements
                .FirstOrDefaultAsync(a => a.AnnouncementId == announcement.AnnouncementId && a.RestaurantId == restaurantId);

            if (existingAnnouncement == null)
            {
                Console.WriteLine("未找到對應的公告");
                return NotFound("未找到對應的公告");
            }

            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine($"驗證錯誤: {error.ErrorMessage}");
                    }
                }
                return BadRequest("資料驗證失敗");
            }

            try
            {
                existingAnnouncement.Title = announcement.Title;
                existingAnnouncement.Content = announcement.Content;
                existingAnnouncement.AnnouncementDate = announcement.AnnouncementDate;
                existingAnnouncement.StartDate = announcement.StartDate;
                existingAnnouncement.EndDate = announcement.EndDate;
                existingAnnouncement.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                Console.WriteLine("公告更新成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新公告時發生錯誤: {ex.Message}");
                return StatusCode(500, "更新公告時發生錯誤");
            }

            return Ok("公告更新成功");
        }


        // -----------------------Restaurant的圖片新view------------------------------

        public IActionResult RestaurantPhoto()
        {
            // 從 Session 中獲取 RestaurantId
            var restaurantIdString = HttpContext.Session.GetString("RestaurantId");
            if (string.IsNullOrEmpty(restaurantIdString) || !int.TryParse(restaurantIdString, out int restaurantId))
            {
                // 如果沒有找到 RestaurantId 或無法轉換，則重定向到登入頁面或顯示錯誤
                return RedirectToAction("LogIn", "Backstage"); // 導回登入頁面
            }

            // 從資料庫中查詢屬於這個餐廳的所有圖片
            var photos = _context.Photos.Where(p => p.RestaurantId == restaurantId).ToList();

            if (photos == null || photos.Count == 0)
            {
                Console.WriteLine("沒有找到對應的圖片資料");
            }

            // 將圖片資料作為模型傳遞給 View
            return View(photos);
        }

        // -----------------------Restaurant的圖片新view上傳------------------------------
        public class PhotoUpdateRequest
        {
            public List<Photo> UpdatedPhotos { get; set; } = new List<Photo>();
            public List<Photo> NewPhotos { get; set; } = new List<Photo>();
            public List<int> DeletedPhotoIds { get; set; } = new List<int>();
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePhoto([FromBody] List<Photo> photos)
        {
            // 確認傳入的圖片資料是否為空
            if (photos == null || photos.Count == 0)
            {
                Console.WriteLine("photos 為 null 或空");
                return BadRequest("無法解析圖片數據");
            }

            // 從 Session 中獲取 RestaurantId，確保只有登入的使用者可以修改資料
            var restaurantIdString = HttpContext.Session.GetString("RestaurantId");
            if (string.IsNullOrEmpty(restaurantIdString) || !int.TryParse(restaurantIdString, out int restaurantId))
            {
                Console.WriteLine("RestaurantId 無法取得或轉換失敗");
                return RedirectToAction("LogIn", "Backstage");
            }

            try
            {
                // 遍歷每個傳入的圖片資料，並進行更新
                foreach (var photo in photos)
                {
                    var existingPhoto = await _context.Photos
                        .FirstOrDefaultAsync(p => p.PhotoId == photo.PhotoId && p.RestaurantId == restaurantId);

                    if (existingPhoto == null)
                    {
                        Console.WriteLine($"未找到對應的圖片: PhotoId = {photo.PhotoId}");
                        return NotFound($"未找到對應的圖片: PhotoId = {photo.PhotoId}");
                    }

                    // 更新圖片的屬性
                    existingPhoto.PhotoUrl = photo.PhotoUrl;
                    existingPhoto.PhotoType = photo.PhotoType;
                    existingPhoto.Description = photo.Description;
                    existingPhoto.UploadedAt = DateTime.UtcNow.AddHours(8); // 更新為當前時間
                }

                // 保存變更
                await _context.SaveChangesAsync();
                Console.WriteLine("圖片資料更新成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新圖片資料時發生錯誤: {ex.Message}");
                return StatusCode(500, "更新圖片資料時發生錯誤");
            }

            return Ok("圖片資料更新成功");
        }

        // -----------------------Restaurant的圖片新view新增------------------------------
        [HttpPost]
        public async Task<IActionResult> AddPhoto([FromBody] Photo photo)
        {
            if (photo == null)
            {
                Console.WriteLine("photo 為 null");
                return BadRequest("無法解析圖片數據");
            }

            // 從 Session 中獲取 RestaurantId，確保只有登入的使用者可以修改資料
            var restaurantIdString = HttpContext.Session.GetString("RestaurantId");
            if (string.IsNullOrEmpty(restaurantIdString) || !int.TryParse(restaurantIdString, out int restaurantId))
            {
                Console.WriteLine("RestaurantId 無法取得或轉換失敗");
                return RedirectToAction("LogIn", "Backstage");
            }

            try
            {
                // 設定餐廳 ID
                photo.RestaurantId = restaurantId;
                photo.UploadedAt = DateTime.UtcNow.AddHours(8);

                // 將新圖片添加到資料庫
                _context.Photos.Add(photo);
                await _context.SaveChangesAsync();
                Console.WriteLine("圖片新增成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"新增圖片時發生錯誤: {ex.Message}");
                return StatusCode(500, "新增圖片時發生錯誤");
            }

            return Ok("圖片新增成功");
        }

        // -----------------------Restaurant的圖片新view刪除------------------------------

        [HttpPost]
        public async Task<IActionResult> DeletePhotos([FromBody] List<int> photoIds)
        {
            // 確認傳入的圖片ID列表是否為空
            if (photoIds == null || photoIds.Count == 0)
            {
                Console.WriteLine("photoIds 為 null 或空");
                return BadRequest("無法解析圖片ID數據");
            }

            // 從 Session 中獲取 RestaurantId，確保只有登入的使用者可以修改資料
            var restaurantIdString = HttpContext.Session.GetString("RestaurantId");
            if (string.IsNullOrEmpty(restaurantIdString) || !int.TryParse(restaurantIdString, out int restaurantId))
            {
                Console.WriteLine("RestaurantId 無法取得或轉換失敗");
                return RedirectToAction("LogIn", "Backstage");
            }

            try
            {
                // 查找並刪除對應的圖片
                var photosToDelete = await _context.Photos
                    .Where(p => photoIds.Contains(p.PhotoId) && p.RestaurantId == restaurantId)
                    .ToListAsync();

                if (photosToDelete == null || photosToDelete.Count == 0)
                {
                    Console.WriteLine("未找到對應的圖片資料");
                    return NotFound("未找到對應的圖片資料");
                }

                // 移除找到的圖片
                _context.Photos.RemoveRange(photosToDelete);

                // 保存變更
                await _context.SaveChangesAsync();
                Console.WriteLine("圖片資料刪除成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"刪除圖片資料時發生錯誤: {ex.Message}");
                return StatusCode(500, "刪除圖片資料時發生錯誤");
            }

            return Ok("圖片資料刪除成功");
        }

        //-----------------------Restaurant的餐廳資訊partial------------------------------

        public IActionResult PartialRestaurantInfo()
        {
            Console.WriteLine("PartialRestaurantInfo() 方法開始執行");

            var restaurantIdString = HttpContext.Session.GetString("RestaurantId");
            if (string.IsNullOrEmpty(restaurantIdString) || !int.TryParse(restaurantIdString, out int restaurantId))
            {
                Console.WriteLine("未能取得 RestaurantId，重定向到登入頁面");
                return RedirectToAction("LogIn", "Backstage");
            }

            var restaurant = _context.Restaurants.FirstOrDefault(r => r.RestaurantId == restaurantId);
            if (restaurant == null)
            {
                Console.WriteLine("未找到對應的餐廳資料");
                return NotFound("未找到對應的餐廳");
            }

            Console.WriteLine("餐廳資料加載成功");
            return PartialView("BO_PartialView/PartialRestaurantInfo", restaurant);
        }

        //-----------------------Restaurant的餐廳資訊partial上傳------------------------------
        [HttpPost]
        public async Task<IActionResult> UpdateRestaurantInfo([FromBody] Restaurant restaurant)
        {


            // 確認傳入的餐廳資料是否為空
            if (restaurant == null)
            {
                Console.WriteLine("restaurant 為 null");
                return BadRequest("無法解析餐廳數據");
            }

            // 確認所有的屬性是否正確接收
            Console.WriteLine($"Received RestaurantId: {restaurant.RestaurantId}");
            Console.WriteLine($"Received Name: {restaurant.Name}");
            Console.WriteLine($"Received Address: {restaurant.Address}");
            Console.WriteLine($"Received ContactPhone: {restaurant.ContactPhone}");
            Console.WriteLine($"Received Description: {restaurant.Description}");
            Console.WriteLine($"Received PriceRange: {restaurant.PriceRange}");
            Console.WriteLine($"Received HasParking: {restaurant.HasParking}");

            // 從 Session 中獲取 RestaurantId，確保只有登入的使用者可以修改資料
            var restaurantIdString = HttpContext.Session.GetString("RestaurantId");
            if (string.IsNullOrEmpty(restaurantIdString) || !int.TryParse(restaurantIdString, out int restaurantId))
            {
                Console.WriteLine("RestaurantId 無法取得或轉換失敗");
                return RedirectToAction("LogIn", "Backstage");
            }

            // 查詢現有的餐廳資料
            var existingRestaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantId == restaurantId);
            if (existingRestaurant == null)
            {
                Console.WriteLine("未找到對應的餐廳");
                return NotFound("未找到對應的餐廳");
            }

            // 檢查 ModelState 是否有效
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine($"驗證錯誤: {error.ErrorMessage}");
                    }
                }
                return BadRequest("資料驗證失敗");
            }

            try
            {
                // 更新餐廳的屬性
                existingRestaurant.Name = restaurant.Name;
                existingRestaurant.Address = restaurant.Address;
                existingRestaurant.ContactPhone = restaurant.ContactPhone;
                existingRestaurant.Description = restaurant.Description;
                existingRestaurant.PriceRange = restaurant.PriceRange;
                existingRestaurant.HasParking = restaurant.HasParking;

                existingRestaurant.UpdatedAt = DateTime.UtcNow; // 更新最後修改時間

                // 保存變更
                await _context.SaveChangesAsync();
                Console.WriteLine("餐廳資料更新成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新餐廳資料時發生錯誤: {ex.Message}");
                return StatusCode(500, "更新餐廳資料時發生錯誤");
            }

            return Ok("餐廳資料更新成功");
        }


        //-----------------------Booking------------------------------

        public IActionResult Booking()
        {
            string LogIn_managerPosition = HttpContext.Session.GetString("ManagerPosition")!.ToString();
            if (LogIn_managerPosition == "財務")
            {
                // 跳轉到首頁
                return RedirectToAction("Home", "Backstage");
            }

            // 從 Session 中獲取 RestaurantId
            var restaurantIdString = HttpContext.Session.GetString("RestaurantId");
            if (string.IsNullOrEmpty(restaurantIdString) || !int.TryParse(restaurantIdString, out int restaurantId))
            {
                // 如果沒有找到 RestaurantId 或無法轉換，則重定向到登入頁面或顯示錯誤
                return RedirectToAction("LogIn", "Backstage"); // 導回登入頁面
            }

            // 從資料庫中取得是否開放訂位的狀態
            var restaurant = _context.Restaurants.FirstOrDefault(r => r.RestaurantId == restaurantId);
            var isReservationOpen = restaurant?.IsReservationOpen ?? false;

            // 將資料傳遞給視圖（可以用 ViewBag 或者 data- 屬性）
            ViewBag.IsReservationOpen = isReservationOpen;

            // 只取得屬於這個餐廳的訂單
            var reservations = _context.Reservations
                .Where(r => r.RestaurantId == restaurantId)
                .OrderByDescending(r => r.ReservationId) // 這裡倒序排列
                .ToList();

            // 將訂單資料傳遞給視圖
            return View(reservations);
        }


        //------------------------booking的修改功能------------------------------
        [HttpPost]
        public async Task<IActionResult> UpdateReservations([FromBody] List<Reservation> updatedReservations)
        {
            if (updatedReservations == null || !updatedReservations.Any())
            {
                return BadRequest("無可更新的資料");
            }

            foreach (var updatedReservation in updatedReservations)
            {
                var existingReservation = await _context.Reservations.FindAsync(updatedReservation.ReservationId);
                if (existingReservation != null)
                {
                    existingReservation.BookerName = updatedReservation.BookerName;
                    existingReservation.BookerPhone = updatedReservation.BookerPhone;
                    existingReservation.BookerEmail = updatedReservation.BookerEmail;
                    existingReservation.NumAdults = updatedReservation.NumAdults;
                    existingReservation.NumChildren = updatedReservation.NumChildren;
                    existingReservation.ReservationDate = updatedReservation.ReservationDate;
                    existingReservation.ReservationTime = updatedReservation.ReservationTime;
                    existingReservation.SpecialRequests = updatedReservation.SpecialRequests;
                    existingReservation.ReservationStatus = updatedReservation.ReservationStatus;
                    existingReservation.UpdatedAt = DateTime.UtcNow; // 更新時間
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
        //------------------------booking 的訂單鎖------------------------------
        [HttpPost]
        public IActionResult ToggleReservationStatus([FromBody] UpdateReservationStatusRequest request)
        {
            try
            {
                // 根據傳來的 RestaurantId 查找餐廳
                var restaurant = _context.Restaurants.FirstOrDefault(r => r.RestaurantId == request.RestaurantId);
                if (restaurant != null)
                {
                    // 更新開放狀態，確保 0 或 1 被轉換為 true 或 false
                    restaurant.IsReservationOpen = request.IsReservationOpen == 1;
                    _context.SaveChanges(); // 保存更改
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"發生錯誤: {ex.Message}");
            }
        }

        // 用於接收 AJAX 請求的模型
        public class UpdateReservationStatusRequest
        {
            public int RestaurantId { get; set; }
            public int IsReservationOpen { get; set; } // 0 或 1
        }
        //---------------------------------------------------------------------

        // GET: 報告 - 訂單數量/List
        public async Task<IActionResult> Report()
		{
			DateOnly today = DateOnly.FromDateTime(DateTime.Now); // 取得今天的日期（不包含時間）

			int? LogIn_restaurantUserId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantUserId"));
			int LogIn_restaurantId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantId"));

			if (LogIn_restaurantUserId == 0)
			{
				// 跳轉到登入頁面
				return RedirectToAction("LogIn", "Backstage");
			}

			var data = await _context.Restaurants
				.Where(r => r.RestaurantId == LogIn_restaurantId)  // 篩選餐廳
				.Include(r => r.Reservations)  // 包含訂位資料
				.ToListAsync();

			var priceString = data[0].PriceRange; // 找到餐廳價錢

			// 移除 "NT$" 和 "," 字元                                         
			string numericString = priceString!.Replace("NT$", "").Replace(",", "");

			// 直接轉換為整數
			int price = int.Parse(numericString);

			ViewBag.priceRange = price; // 餐廳價錢

			// 篩選今天的訂位資料並按小時分組
			var reservationsToday = data
				.SelectMany(r => r.Reservations)
				.Where(res => res.ReservationDate == today) // 直接比較日期
				.GroupBy(res => res.ReservationTime.HasValue ? res.ReservationTime.Value.Hour : (int?)null) // 按小時分組
				.ToDictionary(g => g.Key, g => g.Count()); // 每小時的訂位數			

            // 計算今天所有小時的訂單數量總和
            var total_Today = reservationsToday.Values.Sum();

            // 傳遞今日訂單數量總和到 View
            ViewBag.Total_Today = total_Today;

            // 生成 24 小時的資料 (0~23 時)
            var hourlyData = Enumerable.Range(0, 24)
				.Select(hour => reservationsToday.ContainsKey(hour) ? reservationsToday[hour] : 0) // 如果該小時沒有數據則為 0
				.ToArray();

            // 傳遞資料到 ViewCount
            ViewBag.HourlyData = hourlyData;

			return View(data);
		}

        /*// 篩選出今天訂位的大人數量
			var todayNumAdults = data
				.SelectMany(r => r.Reservations)  // 將所有餐廳的訂位資料展平
				.Where(res => res.ReservationDate == today) // 篩選日期為今天的訂位
				.Sum(res => res.NumAdults); // 計算今天所有訂位的大人數量總和

			ViewBag.todayNumAdults = todayNumAdults; // 今天訂位的大人數量總和

			// 篩選出本週訂位的大人數量
			var weekNumAdults = data
				.SelectMany(r => r.Reservations)  // 將所有餐廳的訂位資料展平
				.Where(res => res.ReservationDate >= weekStart && res.ReservationDate <= weekEnd) // 篩選日期為本週的訂位
				.Sum(res => res.NumAdults); // 計算本週所有訂位的大人數量總和

			ViewBag.weekNumAdults = weekNumAdults; // 本週訂位的大人數量總和

			// 篩選出本月訂位的大人數量
			var monthNumAdults = data
				.SelectMany(r => r.Reservations)  // 將所有餐廳的訂位資料展平
				.Where(res => res.ReservationDate >= monthStart && res.ReservationDate <= monthEnd) // 篩選日期為本月的訂位
				.Sum(res => res.NumAdults); // 計算本月所有訂位的大人數量總和

			ViewBag.monthNumAdults = monthNumAdults; // 本月訂位的大人數量總和


			// 篩選出今天訂位的小孩數量
			var todayNumChildren = data
				.SelectMany(r => r.Reservations)  // 將所有餐廳的訂位資料展平
				.Where(res => res.ReservationDate == today) // 篩選日期為今天的訂位
				.Sum(res => res.NumChildren); // 計算今天所有訂位的小孩數量總和

			ViewBag.todayNumChildren = todayNumChildren; // 今天訂位的小孩數量總和

			// 篩選出本週訂位的小孩數量
			var weekNumChildren = data
				.SelectMany(r => r.Reservations)  // 將所有餐廳的訂位資料展平
				.Where(res => res.ReservationDate >= weekStart && res.ReservationDate <= weekEnd) // 篩選日期為本週的訂位
				.Sum(res => res.NumChildren); // 計算本週所有訂位的小孩數量總和

			ViewBag.weekNumChildren = weekNumChildren; // 本週訂位的小孩數量總和

			// 篩選出本月訂位的小孩數量
			var monthNumChildren = data
				.SelectMany(r => r.Reservations)  // 將所有餐廳的訂位資料展平
				.Where(res => res.ReservationDate >= monthStart && res.ReservationDate <= monthEnd) // 篩選日期為本月的訂位
				.Sum(res => res.NumChildren); // 計算本月所有訂位的小孩數量總和

			ViewBag.monthNumChildren = monthNumChildren; // 本月訂位的小孩數量總和

			// 篩選出今天的評論資料
			var todayReviews = data
				.SelectMany(r => r.Reviews)  // 將所有餐廳的評論資料展平
				.Where(res => res.ReviewDate == today) // 篩選日期為今天的評論
				.ToList();

			ViewBag.todayReviews = todayReviews.Count; // 今天評論數量*/

        public async Task<IActionResult> ReportWeek()
		{
			DateOnly today = DateOnly.FromDateTime(DateTime.Now); // 取得今天的日期（不包含時間）
			DayOfWeek todayWeek = today.DayOfWeek; // 今天是星期幾

			// 取得本週的日期範圍（週一到週日）
			var weekStart = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday); // 本週週一
			var weekEnd = weekStart.AddDays(6); // 本週週日

			int? LogIn_restaurantUserId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantUserId"));
			int LogIn_restaurantId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantId"));

			if (LogIn_restaurantUserId == 0)
			{
				// 跳轉到登入頁面
				return RedirectToAction("LogIn", "Backstage");
			}

			var data = await _context.Restaurants
				.Where(r => r.RestaurantId == LogIn_restaurantId)  // 篩選餐廳
				.Include(r => r.Reservations)  // 包含訂位資料
				.ToListAsync();

			// 篩選本週的訂位資料並按星期分組
			var weeklyReservations = data
				.SelectMany(r => r.Reservations)
				.Where(res => res.ReservationDate >= weekStart && res.ReservationDate <= weekEnd) // 篩選日期在本週範圍內
				.GroupBy(res => res.ReservationDate.HasValue ? res.ReservationDate.Value.DayOfWeek : (DayOfWeek?)null)// 按星期分組
				.Select(g => new
				{
					DayOfWeek = g.Key,
					Count = g.Count()
				})
				.ToList();

            // 計算本週的訂單數量總和
            var total_Week = weeklyReservations.Sum(r => r.Count);

            // 傳遞每週訂單數量總和到 View
            ViewBag.Total_Week = total_Week;

            // 將分組結果轉換成圖表資料格式
            var weeklyData = Enumerable.Range(0, 7) // 包括週日到週六
				.Select(i => new
				{
					Day = (DayOfWeek)i,
					Count = weeklyReservations.FirstOrDefault(r => r.DayOfWeek == (DayOfWeek)i)?.Count ?? 0
				})
				.ToList();

			// 傳遞至 View
			ViewBag.WeeklyData = weeklyData;

			return View(data);
		}

		public async Task<IActionResult> ReportMonth()
		{
			// 取得今天的日期
			DateOnly today = DateOnly.FromDateTime(DateTime.Now);

			// 取得當月的第一天和最後一天
			var monthStart = new DateOnly(today.Year, today.Month, 1);
			var monthEnd = monthStart.AddMonths(1).AddDays(-1);

			// 生成當月的所有日期
			var allDatesInMonth = Enumerable.Range(0, (monthEnd.Day - monthStart.Day) + 1)
				.Select(d => monthStart.AddDays(d))
				.ToList();

			int? LogIn_restaurantUserId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantUserId"));
			int LogIn_restaurantId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantId"));

			if (LogIn_restaurantUserId == 0)
			{
				// 跳轉到登入頁面
				return RedirectToAction("LogIn", "Backstage");
			}

			// 查詢餐廳的資料，包括訂位資料
			var data = await _context.Restaurants
				.Where(r => r.RestaurantId == LogIn_restaurantId)  // 篩選餐廳
				.Include(r => r.Reservations)  // 包含訂位資料
				.ToListAsync();

			// 篩選出本月的訂位資料，並按日期分組
			var monthlyReservations = data
				.SelectMany(r => r.Reservations)
				.Where(res => res.ReservationDate >= monthStart && res.ReservationDate <= monthEnd) // 篩選本月的訂位
				.GroupBy(res => res.ReservationDate) // 按日期分組
				.Select(g => new
				{
					Date = g.Key,
					Count = g.Count()
				})
				.ToDictionary(x => x.Date, x => x.Count); // 將結果轉為字典

			// 計算當月的訂單數量總和
			var total_Month = monthlyReservations.Values.Sum();

			// 傳遞每月訂單數量總和到 View
			ViewBag.Total_Month = total_Month;

			// 將所有日期補全，並將日期格式化為 "dd"
			var completeMonthlyReservations = allDatesInMonth
				.Select(date => new
				{
					Date = date.ToString("dd"), // 格式化為 "dd"
					Count = monthlyReservations.ContainsKey(date) ? monthlyReservations[date] : 0
				})
				.ToList();

			// 傳遞補全的資料到 View
			ViewBag.MonthlyReservations = completeMonthlyReservations;

			return View(data);
		}

        // GET: 報告 - 人數/List
        public async Task<IActionResult> PeopleToday()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now); // 取得今天的日期（不包含時間）

            int? LogIn_restaurantUserId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantUserId"));
            int LogIn_restaurantId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantId"));

            if (LogIn_restaurantUserId == 0)
            {
                // 跳轉到登入頁面
                return RedirectToAction("LogIn", "Backstage");
            }

            var data = await _context.Restaurants
                .Where(r => r.RestaurantId == LogIn_restaurantId)  // 篩選餐廳
                .Include(r => r.Reservations)  // 包含訂位資料
                .ToListAsync();

            // 篩選今天的訂位資料並按小時分組
            var reservationsToday = data
                .SelectMany(r => r.Reservations)
                .Where(res => res.ReservationDate == today) // 只選取今天的訂位資料
				.GroupBy(res => res.ReservationTime.HasValue ? res.ReservationTime.Value.Hour : (int?)null)// 按小時分組
				.ToDictionary(g => g.Key, g => g.ToList()); // 每小時的訂位資料列表

            // 計算今天所有小時的成人數量和兒童數量的總和
            var total_Today = reservationsToday.Values
                .SelectMany(res => res) // 拍平每小時的預訂資料
                .Sum(res => res.NumAdults + res.NumChildren); // 計算 NumAdults 和 NumChildren 的總和

            // 傳遞今日成人和兒童數量總和到 View
            ViewBag.Total_Today = total_Today;

            // 生成每小時成人和兒童數量的資料
            var hourlyAdults = Enumerable.Range(0, 24)
                .Select(hour => reservationsToday.ContainsKey(hour)
                    ? reservationsToday[hour].Sum(res => res.NumAdults)
                    : 0)
                .ToArray();

            var hourlyChildren = Enumerable.Range(0, 24)
                .Select(hour => reservationsToday.ContainsKey(hour)
                    ? reservationsToday[hour].Sum(res => res.NumChildren)
                    : 0)
                .ToArray();

            // 傳遞折線圖的資料
            ViewBag.HourlyAdults = hourlyAdults;
            ViewBag.HourlyChildren = hourlyChildren;

            return View(data);
        }

        public async Task<IActionResult> PeopleWeek()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now); // 取得今天的日期（不包含時間）

            // 計算本週的星期一和星期日
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday); // 本週的第一天（星期一）
            var endOfWeek = startOfWeek.AddDays(6); // 本週的最後一天（星期天）

            int? LogIn_restaurantUserId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantUserId"));
            int LogIn_restaurantId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantId"));

            if (LogIn_restaurantUserId == 0)
            {
                // 跳轉到登入頁面
                return RedirectToAction("LogIn", "Backstage");
            }

            var data = await _context.Restaurants
                .Where(r => r.RestaurantId == LogIn_restaurantId)  // 篩選餐廳
                .Include(r => r.Reservations)  // 包含訂位資料
                .ToListAsync();

            // 篩選本週的訂位資料並按星期分組
            var reservationsThisWeek = data
                .SelectMany(r => r.Reservations)
                .Where(res => res.ReservationDate >= startOfWeek && res.ReservationDate <= endOfWeek) // 篩選本週的資料
				.GroupBy(res => res.ReservationDate.HasValue ? res.ReservationDate.Value.DayOfWeek : (DayOfWeek?)null) // 按星期分組
				.ToDictionary(g => g.Key, g => g.ToList()); // 每天的訂位資料列表

            // 計算本週所有天數的成人數量和兒童數量的總和
            var total_Week = reservationsThisWeek.Values
                .SelectMany(res => res) // 拍平每個日期的預訂資料
                .Sum(res => res.NumAdults + res.NumChildren); // 計算 NumAdults 和 NumChildren 的總和

            // 傳遞本週成人和兒童數量總和到 View
            ViewBag.Total_Week = total_Week;

            // 生成每個星期幾的成人和兒童數量的資料
            var weeklyAdults = Enum.GetValues<DayOfWeek>().Cast<DayOfWeek>()
                .Select(dayOfWeek => reservationsThisWeek.ContainsKey(dayOfWeek)
                    ? reservationsThisWeek[dayOfWeek].Sum(res => res.NumAdults)
                    : 0)
                .ToArray();

            var weeklyChildren = Enum.GetValues<DayOfWeek>().Cast<DayOfWeek>()
                .Select(dayOfWeek => reservationsThisWeek.ContainsKey(dayOfWeek)
                    ? reservationsThisWeek[dayOfWeek].Sum(res => res.NumChildren)
                    : 0)
                .ToArray();

            // 傳遞折線圖的資料
            ViewBag.WeeklyAdults = weeklyAdults;
            ViewBag.WeeklyChildren = weeklyChildren;

            return View(data);
        }


        public async Task<IActionResult> PeopleMonth()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now); // 取得今天的日期（不包含時間）

            // 計算當月的第一天和最後一天
            var monthStart = new DateOnly(today.Year, today.Month, 1);
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);

            int? LogIn_restaurantUserId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantUserId"));
            int LogIn_restaurantId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantId"));

            if (LogIn_restaurantUserId == 0)
            {
                // 跳轉到登入頁面
                return RedirectToAction("LogIn", "Backstage");
            }

            var data = await _context.Restaurants
                .Where(r => r.RestaurantId == LogIn_restaurantId)  // 篩選餐廳
                .Include(r => r.Reservations)  // 包含訂位資料
                .ToListAsync();

            // 篩選當月的訂位資料並按日期分組
            var reservationsThisMonth = data
                .SelectMany(r => r.Reservations)
                .Where(res => res.ReservationDate >= monthStart && res.ReservationDate <= monthEnd) // 篩選當月的資料
                .GroupBy(res => res.ReservationDate) // 按日期分組
                .ToDictionary(g => g.Key, g => g.ToList()); // 每天的訂位資料列表

            // 計算本月所有日期的成人數量和兒童數量的總和
            var total_Month = reservationsThisMonth.Values
                .SelectMany(res => res) // 拍平每個日期的預訂資料
                .Sum(res => res.NumAdults + res.NumChildren); // 計算 NumAdults 和 NumChildren 的總和

            // 傳遞本月成人和兒童數量總和到 View
            ViewBag.Total_Month = total_Month;

            // 生成每個日期成人和兒童數量的資料
            var dailyAdults = Enumerable.Range(1, monthEnd.Day)
                .Select(day => reservationsThisMonth.ContainsKey(monthStart.AddDays(day - 1))
                    ? reservationsThisMonth[monthStart.AddDays(day - 1)].Sum(res => res.NumAdults)
                    : 0)
                .ToArray();

            var dailyChildren = Enumerable.Range(1, monthEnd.Day)
                .Select(day => reservationsThisMonth.ContainsKey(monthStart.AddDays(day - 1))
                    ? reservationsThisMonth[monthStart.AddDays(day - 1)].Sum(res => res.NumChildren)
                    : 0)
                .ToArray();

            // 傳遞折線圖的資料
            ViewBag.DailyAdults = dailyAdults;
            ViewBag.DailyChildren = dailyChildren;

            // 傳遞日期標籤（1 到 31）
            ViewBag.Days = Enumerable.Range(1, monthEnd.Day).ToArray();

            return View(data);
        }

        public async Task<IActionResult> MoneyToday()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now); // 取得今天的日期（不包含時間）

            int? LogIn_restaurantUserId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantUserId"));
            int LogIn_restaurantId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantId"));

            if (LogIn_restaurantUserId == 0)
            {
                // 跳轉到登入頁面
                return RedirectToAction("LogIn", "Backstage");
            }

            var data = await _context.Restaurants
                .Where(r => r.RestaurantId == LogIn_restaurantId)  // 篩選餐廳
                .Include(r => r.Reservations)  // 包含訂位資料
                .ToListAsync();

            var priceString = data[0].PriceRange; // 找到餐廳價錢

            // 移除 "NT$" 和 "," 字元
            string numericString = priceString!.Replace("NT$", "").Replace(",", "");

            // 直接轉換為整數
            int price = int.Parse(numericString);

            // 篩選今天的訂位資料並按小時分組
            var reservationsToday = data
                .SelectMany(r => r.Reservations)
                .Where(res => res.ReservationDate == today) // 只選取今天的訂位資料
				.GroupBy(res => res.ReservationTime.HasValue ? res.ReservationTime.Value.Hour : (int?)null) // 按小時分組
				.ToDictionary(g => g.Key, g => g.ToList()); // 每小時的訂位資料列表

            // 生成每小時成人和兒童數量的資料
            var hourlyAdults = Enumerable.Range(0, 24)
                .Select(hour => reservationsToday.ContainsKey(hour)
                    ? reservationsToday[hour].Sum(res => res.NumAdults)
                    : 0)
                .ToArray();

            var hourlyChildren = Enumerable.Range(0, 24)
                .Select(hour => reservationsToday.ContainsKey(hour)
                    ? reservationsToday[hour].Sum(res => res.NumChildren)
                    : 0)
                .ToArray();

            // 計算每小時的收入（成人 * price * 2 + 小孩 * price）
            var hourlyRevenue = Enumerable.Range(0, 24)
                .Select(hour => (hourlyAdults[hour] * price * 2) + (hourlyChildren[hour] * price))
                .ToArray();

            // 計算總收入
            var totalRevenue = hourlyRevenue.Sum();

            // 傳遞折線圖的資料
            ViewBag.HourlyRevenue = hourlyRevenue; // 每小時收入
            ViewBag.Total_Money = totalRevenue; // 今日總收入

            return View(data);
        }

        public async Task<IActionResult> MoneyWeek()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now); // 取得今天的日期（不包含時間）

            // 計算本週的起始日期（週一）和結束日期（週日）
            var dayOfWeek = (int)today.DayOfWeek; // 取得今天是星期幾（0 = Sunday, 1 = Monday, ..., 6 = Saturday）
            var startOfWeek = today.AddDays(-dayOfWeek + 1); // 本週的第一天（星期一）
            var endOfWeek = startOfWeek.AddDays(6); // 本週的最後一天（星期日）

            int? LogIn_restaurantUserId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantUserId"));
            int LogIn_restaurantId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantId"));

            if (LogIn_restaurantUserId == 0)
            {
                // 跳轉到登入頁面
                return RedirectToAction("LogIn", "Backstage");
            }

            var data = await _context.Restaurants
                .Where(r => r.RestaurantId == LogIn_restaurantId)  // 篩選餐廳
                .Include(r => r.Reservations)  // 包含訂位資料
                .ToListAsync();

            var priceString = data[0].PriceRange; // 找到餐廳價錢

            // 移除 "NT$" 和 "," 字元
            string numericString = priceString!.Replace("NT$", "").Replace(",", "");

            // 直接轉換為整數
            int price = int.Parse(numericString);

            // 篩選本週的訂位資料並按日期分組
            var reservationsThisWeek = data
                .SelectMany(r => r.Reservations)
                .Where(res => res.ReservationDate >= startOfWeek && res.ReservationDate <= endOfWeek) // 篩選本週的資料
                .GroupBy(res => res.ReservationDate.HasValue ? res.ReservationDate.Value.DayOfWeek : (DayOfWeek?)null) // 按星期分組
				.ToDictionary(g => g.Key, g => g.ToList()); // 每星期的訂位資料列表

            // 生成每週每天成人和兒童數量的資料
            var dailyAdults = Enum.GetValues<DayOfWeek>()
                .Select(day => reservationsThisWeek.ContainsKey(day)
                    ? reservationsThisWeek[day].Sum(res => res.NumAdults)
                    : 0)
                .ToArray();

            var dailyChildren = Enum.GetValues<DayOfWeek>()
                .Select(day => reservationsThisWeek.ContainsKey(day)
                    ? reservationsThisWeek[day].Sum(res => res.NumChildren)
                    : 0)
                .ToArray();

            // 計算每週每天的收入（成人 * price * 2 + 小孩 * price）
            var dailyRevenue = Enum.GetValues<DayOfWeek>()
                .Select(day => (dailyAdults[(int)day] * price * 2) + (dailyChildren[(int)day] * price))
                .ToArray();

            // 計算本週總收入
            var totalRevenue = dailyRevenue.Sum();

            // 傳遞折線圖的資料
            ViewBag.DailyRevenue = dailyRevenue; // 每天收入
            ViewBag.Total_Money = totalRevenue; // 本週總收入
            
            return View(data);
        }

		public async Task<IActionResult> MoneyMonth()
		{
			DateOnly today = DateOnly.FromDateTime(DateTime.Now); // 取得今天的日期（不包含時間）

			// 計算本月的起始日期（1日）和結束日期（當月最後一天）
			var startOfMonth = new DateOnly(today.Year, today.Month, 1); // 本月的第一天
			var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1); // 本月的最後一天

			int? LogIn_restaurantUserId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantUserId"));
			int LogIn_restaurantId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantId"));

			if (LogIn_restaurantUserId == 0)
			{
				// 跳轉到登入頁面
				return RedirectToAction("LogIn", "Backstage");
			}

			var data = await _context.Restaurants
				.Where(r => r.RestaurantId == LogIn_restaurantId)  // 篩選餐廳
				.Include(r => r.Reservations)  // 包含訂位資料
				.ToListAsync();

			var priceString = data[0].PriceRange; // 找到餐廳價錢

			// 移除 "NT$" 和 "," 字元
			string numericString = priceString!.Replace("NT$", "").Replace(",", "");

			// 直接轉換為整數
			int price = int.Parse(numericString);

			// 篩選本月的訂位資料並按日期分組
			var reservationsThisMonth = data
				.SelectMany(r => r.Reservations)
				.Where(res => res.ReservationDate >= startOfMonth && res.ReservationDate <= endOfMonth) // 篩選本月的資料
				.GroupBy(res => res.ReservationDate.HasValue ? res.ReservationDate.Value.Day : (int?)null) // 按日期分組
				.ToDictionary(g => g.Key, g => g.ToList()); // 每天的訂位資料列表

			// 生成每月每天成人和兒童數量的資料
			var dailyAdults = Enumerable.Range(1, endOfMonth.Day)
				.Select(day => reservationsThisMonth.ContainsKey(day)
					? reservationsThisMonth[day].Sum(res => res.NumAdults)
					: 0)
				.ToArray();

			var dailyChildren = Enumerable.Range(1, endOfMonth.Day)
				.Select(day => reservationsThisMonth.ContainsKey(day)
					? reservationsThisMonth[day].Sum(res => res.NumChildren)
					: 0)
				.ToArray();

			// 計算每月每天的收入（成人 * price * 2 + 小孩 * price）
			var dailyRevenue = Enumerable.Range(1, endOfMonth.Day)
				.Select(day => (dailyAdults[day - 1] * price * 2) + (dailyChildren[day - 1] * price))
				.ToArray();

			// 計算本月總收入
			var totalRevenue = dailyRevenue.Sum();

			// 傳遞折線圖的資料
			ViewBag.DailyRevenue = dailyRevenue; // 每天收入
			ViewBag.Total_Money = totalRevenue; // 本月總收入

			return View(data);
		}

        public async Task<IActionResult> ReviewWeek()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now); // 取得今天的日期（不包含時間）
            DayOfWeek todayWeek = today.DayOfWeek; // 今天是星期幾

            // 取得本週的日期範圍（週一到週日）
            var weekStart = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday); // 本週週一
            var weekEnd = weekStart.AddDays(6); // 本週週日

            int? LogIn_restaurantUserId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantUserId"));
            int LogIn_restaurantId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantId"));

            if (LogIn_restaurantUserId == 0)
            {
                // 跳轉到登入頁面
                return RedirectToAction("LogIn", "Backstage");
            }

            var data = await _context.Restaurants
                .Where(r => r.RestaurantId == LogIn_restaurantId)  // 篩選餐廳
                .Include(r => r.Reviews)  // 包含評論資料
                .ToListAsync();

            // 篩選本週的評論資料並按星期分組
            var weeklyReviews = data
                .SelectMany(r => r.Reviews)  // 展開餐廳的所有評論資料
                .Where(review => review.ReviewDate >= weekStart && review.ReviewDate <= weekEnd) // 篩選日期在本週範圍內
                .GroupBy(review => review.ReviewDate.HasValue ? review.ReviewDate.Value.DayOfWeek : (DayOfWeek?)null) // 按星期分組
				.Select(g => new
                {
                    DayOfWeek = g.Key,
                    Count = g.Count()  // 計算每個星期的評論數量
                })
                .ToList();

            // 計算本週的評論總數
            var total_Week = weeklyReviews.Sum(r => r.Count);

            // 傳遞每週評論數量總和到 View
            ViewBag.Total_Week = total_Week;

            // 將分組結果轉換成圖表資料格式
            var weeklyData = Enumerable.Range(0, 7) // 包括週日到週六
                .Select(i => new
                {
                    Day = (DayOfWeek)i,
                    Count = weeklyReviews.FirstOrDefault(r => r.DayOfWeek == (DayOfWeek)i)?.Count ?? 0
                })
                .ToList();

            // 傳遞至 View
            ViewBag.WeeklyData = weeklyData;

            return View(data);
        }

        public async Task<IActionResult> ReviewMonth()
        {
            // 取得今天的日期
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            // 取得當月的第一天和最後一天
            var monthStart = new DateOnly(today.Year, today.Month, 1);
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);

            // 生成當月的所有日期
            var allDatesInMonth = Enumerable.Range(0, (monthEnd.Day - monthStart.Day) + 1)
                .Select(d => monthStart.AddDays(d))
                .ToList();

            int? LogIn_restaurantUserId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantUserId"));
            int LogIn_restaurantId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantId"));

            if (LogIn_restaurantUserId == 0)
            {
                // 跳轉到登入頁面
                return RedirectToAction("LogIn", "Backstage");
            }

            // 查詢餐廳的資料，包括評論資料
            var data = await _context.Restaurants
                .Where(r => r.RestaurantId == LogIn_restaurantId)  // 篩選餐廳
                .Include(r => r.Reviews)  // 包含評論資料
                .ToListAsync();

            // 篩選出本月的評論資料，並按日期分組
            var monthlyReviews = data
                .SelectMany(r => r.Reviews)  // 展開餐廳的所有評論資料
                .Where(review => review.ReviewDate >= monthStart && review.ReviewDate <= monthEnd) // 篩選本月的評論
                .GroupBy(review => review.ReviewDate) // 按日期分組
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()  // 計算每天的評論數量
                })
                .ToDictionary(x => x.Date, x => x.Count); // 將結果轉為字典

            // 計算當月的評論總數
            var total_Month = monthlyReviews.Values.Sum();

            // 傳遞每月評論數量總和到 View
            ViewBag.Total_Month = total_Month;

            // 將所有日期補全，並將日期格式化為 "dd"
            var completeMonthlyReviews = allDatesInMonth
                .Select(date => new
                {
                    Date = date.ToString("dd"), // 格式化為 "dd"
                    Count = monthlyReviews.ContainsKey(date) ? monthlyReviews[date] : 0
                })
                .ToList();

            // 傳遞補全的資料到 View
            ViewBag.MonthlyReviews = completeMonthlyReviews;

            return View(data);
        }

		// GET: 評分與評論/List
		public async Task<IActionResult> CustomerFeedback()
		{
			int? LogIn_restaurantUserId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantUserId"));
			int LogIn_restaurantId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantId"));

			if (LogIn_restaurantUserId == 0)
			{
				// 跳轉到登入頁面
				return RedirectToAction("LogIn", "Backstage");
			}

			var reviews = await _context.Reviews
				.Where(data => data.RestaurantId == LogIn_restaurantId)
				.Include(data => data.User)
				.ToListAsync();

			// 計算每個星級的評論數
			var ratingsCount = reviews
				.Where(r => r.Rating.HasValue) // 過濾掉 Rating 為 null 的記錄
				.GroupBy(r => r.Rating!.Value)  // 使用 Value 獲取非 null 的值
				.ToDictionary(g => g.Key, g => g.Count());

			// 確保 1 到 5 星都有數據，沒有的補 0
			for (int i = 1; i <= 5; i++)
			{
				if (!ratingsCount.ContainsKey(i))
				{
					ratingsCount[i] = 0;
				}
			}

			// 總評論數
			int totalReviews = reviews.Count;

			// 計算總分（若無評論時避免為 0）
			double totalScore = totalReviews > 0
				? reviews.Sum(r => r.Rating!.Value) // 計算所有評論的總分
				: 0;

			// 傳遞數據到前端
			ViewBag.RatingsCount = ratingsCount;
			ViewBag.TotalReviews = totalReviews;
			ViewBag.TotalScore = totalScore;

			if (reviews == null)
			{
				// 跳轉到登入頁面
				return RedirectToAction("LogIn", "Backstage");
			}

            if (reviews == null || totalReviews == 0)
            {
                // 如果沒有評論資料，設為 0
                ViewBag.AverageRating = 0;
            }
            else
            {
                // 自動執行餐廳分數更新
                double averageRating = Math.Round((double)totalScore / (double)totalReviews, 1);

                try
                {
                    var existingRestaurant = await _context.Restaurants.FindAsync(LogIn_restaurantId);
                    if (existingRestaurant == null)
                    {
                        return NotFound();
                    }

                    existingRestaurant.AverageRating = averageRating;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }
            return View(reviews);
        }

            // GET: 營業時間/Edit
            public async Task<IActionResult> BusinessHours()
		{
			int? LogIn_restaurantUserId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantUserId"));
			int LogIn_restaurantId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantId"));

			if (LogIn_restaurantUserId == 0)
			{
				// 跳轉到登入頁面
				return RedirectToAction("LogIn", "Backstage");
			}

            string LogIn_managerPosition = HttpContext.Session.GetString("ManagerPosition")!.ToString();
            if (LogIn_managerPosition == "財務")
            {
                // 跳轉到首頁
                return RedirectToAction("Home", "Backstage");
            }

            var ruid = await _context.Restaurants.FindAsync(LogIn_restaurantId);
			if (ruid == null)
			{
				// 跳轉到登入頁面
				return RedirectToAction("LogIn", "Backstage");
			}
			return View(ruid);
		}

		// POST: 營業時間/Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> BusinessHours([Bind("RestaurantId,BusinessHoursStart,BusinessHoursEnd,LastCheckInTime")] Restaurant restaurant)
		{
			// 取得目前登入餐廳 ID
			int LogIn_restaurantId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantId"));

			// 檢查是否符合 Session 中的餐廳 ID
			if (LogIn_restaurantId != restaurant.RestaurantId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					// 查詢現有資料
					var businessHours = await _context.Restaurants.FindAsync(LogIn_restaurantId);
					if (businessHours == null)
					{
						return NotFound();
					}

					// 更新需要的屬性
					businessHours.BusinessHoursStart = restaurant.BusinessHoursStart; // 更新開始營業時間
					businessHours.BusinessHoursEnd = restaurant.BusinessHoursEnd; // 更新結束營業時間
					businessHours.LastCheckInTime = restaurant.LastCheckInTime; // 更新最後訂位時間

					// 儲存變更
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					// 檢查資料是否已被刪除
					if (!BusinessHoursExists(LogIn_restaurantId))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(BusinessHours)); // 更新成功後返回設定頁面
			}
			return View(restaurant); // 驗證失敗時返回編輯頁面
		}

		private bool BusinessHoursExists(int restaurantId)
		{
			return _context.RestaurantUsers.Any(e => e.RestaurantUserId == restaurantId);
		}

		// GET: 用戶管理/List
		public async Task<IActionResult> UserManagement()
		{
			int? LogIn_restaurantUserId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantUserId"));
			int LogIn_restaurantId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantId"));
            string LogIn_managerPosition = HttpContext.Session.GetString("ManagerPosition")!.ToString();

            if (LogIn_restaurantUserId == 0)
			{
				// 跳轉到登入頁面
				return RedirectToAction("LogIn", "Backstage");
			}
            
            if (LogIn_managerPosition == "財務")
            {
                // 跳轉到首頁
                return RedirectToAction("Home", "Backstage");
            }

            var users = await _context.RestaurantUsers
				.Where(user => user.RestaurantId == LogIn_restaurantId).ToListAsync();

			if (users == null)
			{
				// 跳轉到登入頁面
				return RedirectToAction("LogIn", "Backstage");
			}
			return View(users);
		}

		// GET: 店長新增管理人員/Create
		public IActionResult CreateUM()
		{
			int? LogIn_restaurantUserId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantUserId"));

			if (LogIn_restaurantUserId == 0)
			{
				// 跳轉到登入頁面
				return RedirectToAction("LogIn", "Backstage");
			}

			return View();
		}

		// POST: 店長新增管理人員/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateUM([Bind("RestaurantId,Email,PasswordHash,MobileNumber,ManagerPosition,ManagerId,CreatedAt,UpdatedAt")] RestaurantUser restaurantUser)
		{
			int LogIn_restaurantId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantId"));

			// 設定預設值
			restaurantUser.RestaurantId = LogIn_restaurantId;  // 預設為與店長相同餐廳ID
			restaurantUser.CreatedAt = DateTime.Now;  // 預設為當下時間
			restaurantUser.UpdatedAt = DateTime.Now;  // 預設為當下時間

			if (ModelState.IsValid)
			{
				_context.Add(restaurantUser);
				await _context.SaveChangesAsync();

				return RedirectToAction(nameof(UserManagement));
			}
			return View(restaurantUser);
		}

		// GET: 店長編輯管理人員/Edit
		public async Task<IActionResult> EditUM(int? id)
		{
			int? LogIn_restaurantUserId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantUserId"));

			if (LogIn_restaurantUserId == 0)
			{
				// 跳轉到登入頁面
				return RedirectToAction("LogIn", "Backstage");
			}

			var editUM = await _context.RestaurantUsers.FindAsync(id);
			if (editUM == null)
			{
				// 跳轉到登入頁面
				return RedirectToAction("LogIn", "Backstage");
			}
			return View(editUM);
		}

		// POST: 店長編輯管理人員/Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditUM(int id, [Bind("RestaurantUserId,ManagerPosition")] RestaurantUser updatedUser)
		{

			// 檢查是否符合點擊編輯的管理人員ID
			if (id != updatedUser.RestaurantUserId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					// 查詢現有資料
					var existingUser = await _context.RestaurantUsers.FindAsync(id);
					if (existingUser == null)
					{
						return NotFound();
					}

					// 更新需要的屬性
					existingUser.ManagerPosition = updatedUser.ManagerPosition; // 更新職位

					// 儲存變更
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					// 檢查資料是否已被刪除
					if (!EditUMExists(id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(UserManagement)); // 更新成功後返回設定頁面
			}
			return View(updatedUser); // 驗證失敗時返回編輯頁面
		}

		private bool EditUMExists(int id)
		{
			return _context.RestaurantUsers.Any(e => e.RestaurantUserId == id);
		}

		// GET: 店長刪除管理人員/Delete
		public async Task<IActionResult> DeleteUM(int id)
		{
			int? LogIn_restaurantUserId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantUserId"));

			if (LogIn_restaurantUserId == 0)
			{
				// 跳轉到登入頁面
				return RedirectToAction("LogIn", "Backstage");
			}

			var deleteUM = await _context.RestaurantUsers.FindAsync(id);
			if (deleteUM == null)
			{
				// 跳轉到登入頁面
				return RedirectToAction("LogIn", "Backstage");
			}
			return View(deleteUM);
		}

		// POST: 店長刪除管理人員/Delete
		[HttpPost, ActionName("DeleteUM")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteUMConfirmed(int id)
		{
			var deleteUMConfirmed = await _context.RestaurantUsers.FindAsync(id);
			if (deleteUMConfirmed != null)
			{
				_context.RestaurantUsers.Remove(deleteUMConfirmed);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(UserManagement));
		}

		// GET: 設定/Edit
		public async Task<IActionResult> Settings()
		{
			int? LogIn_restaurantUserId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantUserId"));

			if (LogIn_restaurantUserId == 0)
			{
				// 跳轉到登入頁面
				return RedirectToAction("LogIn", "Backstage");
			}

			var ruid = await _context.RestaurantUsers.FindAsync(LogIn_restaurantUserId);
			if (ruid == null)
			{
				// 跳轉到登入頁面
				return RedirectToAction("LogIn", "Backstage");
			}
			return View(ruid);
		}

		// POST: 設定/Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Settings([Bind("RestaurantUserId,Email,MobileNumber,PasswordHash")] RestaurantUser updatedUser)
		{
			// 取得目前登入的使用者 ID
			int LogIn_restaurantUserId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantUserId"));

			// 檢查是否符合 Session 中的使用者 ID
			if (LogIn_restaurantUserId != updatedUser.RestaurantUserId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					// 查詢現有資料
					var existingUser = await _context.RestaurantUsers.FindAsync(LogIn_restaurantUserId);
					if (existingUser == null)
					{
						return NotFound();
					}

					// 更新需要的屬性
					existingUser.Email = updatedUser.Email; // 更新電子郵件
					existingUser.MobileNumber = updatedUser.MobileNumber; // 更新手機號碼
					existingUser.PasswordHash = updatedUser.PasswordHash; // 更新密碼

					// 儲存變更
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					// 檢查資料是否已被刪除
					if (!RestaurantUserExists(LogIn_restaurantUserId))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Settings)); // 更新成功後返回設定頁面
			}
			return View(updatedUser); // 驗證失敗時返回編輯頁面
		}

		private bool RestaurantUserExists(int restaurantUserId)
		{
			return _context.RestaurantUsers.Any(e => e.RestaurantUserId == restaurantUserId);
		}

		// GET: 管理人員註冊/Create
		public IActionResult SignUp()
		{
			return View();
		}

		// POST: 管理人員註冊/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SignUp([Bind("Email,PasswordHash,MobileNumber,ManagerPosition,ManagerId,CreatedAt,UpdatedAt")] RestaurantUser restaurantUser)
		{
			// 設定預設值
			restaurantUser.ManagerPosition = "店長";  // 預設為店長
			restaurantUser.CreatedAt = DateTime.Now;  // 預設為當下時間
			restaurantUser.UpdatedAt = DateTime.Now;  // 預設為當下時間

			if (ModelState.IsValid)
			{
				_context.Add(restaurantUser);
				await _context.SaveChangesAsync();

				// 驗證成功，處理登入邏輯，例如設定 Session 或 Cookie
				HttpContext.Session.SetString("ManagerId", restaurantUser.ManagerId!); // 儲存登入的帳號名稱
				HttpContext.Session.SetString("RestaurantUserId", restaurantUser.RestaurantUserId.ToString()!); // 儲存登入的帳號ID
				HttpContext.Session.SetString("ManagerPosition", restaurantUser.ManagerPosition.ToString()!); // 儲存登入的帳號職位

				return RedirectToAction(nameof(SignUp_Restaurant));
			}
			return View(restaurantUser);
		}

		// GET: 餐廳註冊/Create
		public IActionResult SignUp_Restaurant()
		{
			return View();
		}

		// POST: 餐廳註冊/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SignUp_Restaurant([Bind("Name,Address,ContactPhone,Description,HasParking,IsReservationOpen,AverageRating,CreatedAt,UpdatedAt,BusinessHoursStart,BusinessHoursEnd,LastCheckInTime,GoogleMapAddress,PriceRange")] Restaurant restaurant)
		{
			// 設定預設值
			restaurant.Description = "無";
			restaurant.HasParking = true;
			restaurant.IsReservationOpen = true;
			restaurant.AverageRating = 0.0;
			restaurant.CreatedAt = DateTime.Now;
			restaurant.UpdatedAt = DateTime.Now;
			restaurant.BusinessHoursStart = TimeOnly.FromDateTime(DateTime.Now);
			restaurant.BusinessHoursEnd = TimeOnly.FromDateTime(DateTime.Now);
			restaurant.LastCheckInTime = TimeOnly.FromDateTime(DateTime.Now);
			restaurant.GoogleMapAddress = "0";
			restaurant.PriceRange = "0";

			if (ModelState.IsValid)
			{
				_context.Add(restaurant);
				await _context.SaveChangesAsync();

				// 驗證成功，處理登入邏輯，例如設定 Session 或 Cookie
				HttpContext.Session.SetString("RestaurantId", restaurant.RestaurantId.ToString()!); // 儲存登入的店家ID
				HttpContext.Session.SetString("RestaurantName", restaurant.Name?.ToString()!); // 儲存登入的餐廳名稱

				return RedirectToAction(nameof(AutoEditRestaurantID));
			}
			return View(restaurant);
		}
            // GET: 自動執行餐廳ID更新
            public async Task<IActionResult> AutoEditRestaurantID()
		{
			int LogIn_restaurantUserId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantUserId"));
			int LogIn_restaurantId = Convert.ToInt32(HttpContext.Session.GetString("RestaurantId"));

			try
			{
				var existingUser = await _context.RestaurantUsers.FindAsync(LogIn_restaurantUserId);
				if (existingUser == null)
				{
					return NotFound();
				}

				existingUser.RestaurantId = LogIn_restaurantId;

				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				return NotFound();
			}

			// 執行完成後可選擇跳轉到其他頁面
			return RedirectToAction("Restaurant");
		}

		// GET: 登入/Login
		public IActionResult LogIn()
		{
			return View();
		}

		// POST: 登入/Login
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(string managerId, string password)
		{
			// 從資料庫中查詢對應的使用者
			var user = await _context.RestaurantUsers
				.Include(r => r.Restaurant)  // 包含餐廳資料
				.FirstOrDefaultAsync(u => u.ManagerId == managerId);

			if (user != null && user.PasswordHash == password) // 簡單驗證，建議使用密碼哈希驗證
			{
				// 驗證成功，處理登入邏輯，例如設定 Session 或 Cookie
				HttpContext.Session.SetString("ManagerId", user.ManagerId!); // 儲存登入的帳號ID
				HttpContext.Session.SetString("RestaurantUserId", user.RestaurantUserId.ToString()!); // 儲存登入的帳號ID
				HttpContext.Session.SetString("RestaurantId", user.RestaurantId.ToString()!); // 儲存登入的店家ID
				HttpContext.Session.SetString("ManagerPosition", user.ManagerPosition?.ToString()!); // 儲存登入的帳號職位
				HttpContext.Session.SetString("RestaurantName", user.Restaurant?.Name?.ToString()!); // 儲存登入的餐廳名稱

				
				return RedirectToAction("Home", "Backstage");
			}

			// 驗證失敗，顯示錯誤訊息
			/*ModelState.AddModelError(string.Empty, "無效的登入嘗試");*/
			return View();
		}

		public IActionResult Logout()
		{
			// 清除所有 Session 資料
			HttpContext.Session.Clear();

			// 跳轉到登入頁面
			return RedirectToAction("LogIn", "Backstage");
		}
	}
}
