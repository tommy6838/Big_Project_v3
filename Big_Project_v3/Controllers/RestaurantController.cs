using Big_Project_v3.Models;
using Big_Project_v3.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Drawing.Printing;


namespace Big_Project_v3.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly ITableDbContext _context;
        private const int PageSize = 2;

        public RestaurantController(ITableDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int Id, int? page)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            var restaurant = await _context.Restaurants
                .Include(r => r.Announcements)
                .Include(r => r.Reviews).ThenInclude(rw => rw.User)
                .Include(r => r.Favorites).ThenInclude(f => f.User)
                .Include(r => r.Photos)
                .FirstOrDefaultAsync(m => m.RestaurantId == Id);

            if (restaurant == null)
            {
                return NotFound();
            }

            var isFavorite = userId.HasValue && await _context.Favorites
                .AnyAsync(f => f.UserId == userId && f.RestaurantId == Id);

            var announcementContent = restaurant.Announcements.FirstOrDefault()?.Content ?? string.Empty;
            var announcementParagraphs = new List<string>();

            if (!string.IsNullOrEmpty(announcementContent))
            {
                announcementParagraphs = announcementContent
                    .Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
            }

            int currentPage = page ?? 1;
            var totalReviews = restaurant.Reviews.Count();
            var reviews = restaurant.Reviews
                .OrderByDescending(r => r.ReviewDate)
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var viewModel = new RestaurantViewModel
            {
                Restaurant = restaurant,
                MainPhoto = restaurant.Photos.FirstOrDefault(p => p.PhotoType == "首圖")
                     ?? new Photo { PhotoUrl = "https://via.placeholder.com/1200x400?text=No+Image+Available" },
                EnvironmentPhotos = restaurant.Photos.Where(p => p.PhotoType == "餐廳環境").ToList(),
                MenuPhoto = restaurant.Photos.FirstOrDefault(p => p.PhotoType == "菜單")
                     ?? new Photo { PhotoUrl = "https://inline.app/get-printed-menus?cid=-LARHRYjmf_PDvmeH_2U&bid=-LARHRYjmf_PDvmeH_2V" },
                AnnouncementParagraphs = announcementParagraphs,
                Reviews = reviews,
                IsFavorite = isFavorite,  // 判斷使用者是否已收藏
                CurrentPage = currentPage,
                TotalPages = (int)Math.Ceiling(totalReviews / (double)PageSize)
            };

            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = (int)Math.Ceiling(totalReviews / (double)PageSize);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFavorite([FromBody] int Id)
        {
            // 取得登入用戶的 ID
            var userId = HttpContext.Session.GetInt32("UserId");  // 從 Session 中取得 userId
            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "請先登入會員" });  // 如果未登入
            }

            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null)
            {
                return Json(new { success = false, message = "使用者不存在" });
            }

            // 檢查該使用者是否已經收藏該餐廳
            var existingFavorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == user.UserId && f.RestaurantId == Id);

            if (existingFavorite != null)
            {
                // 移除收藏
                _context.Favorites.Remove(existingFavorite);
                await _context.SaveChangesAsync();
                return Json(new { success = true, isFavorite = false });
            }
            else
            {
                // 新增收藏
                var newFavorite = new Favorite
                {
                    UserId = user.UserId,
                    RestaurantId = Id,
                    AddedAt = DateTime.Now
                };
                _context.Favorites.Add(newFavorite);
                await _context.SaveChangesAsync();
                return Json(new { success = true, isFavorite = true });
            }
        }

        [HttpGet]
        public IActionResult GetReviews(int id, int page = 1)
        {
            int pageSize = 2; // 每頁顯示的評論數量

            // 從資料庫中獲取餐廳資訊和相關資料
            var restaurant = _context.Restaurants
                .Include(r => r.Photos)
                .Include(r => r.Announcements)
                .Include(r => r.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefault(r => r.RestaurantId == id);

            if (restaurant == null)
            {
                return NotFound();
            }

            // 獲取主要照片（MainPhoto），假設類型為 Photo
            var mainPhoto = restaurant.Photos.FirstOrDefault(p => p.PhotoType == "MainPhoto");
            if (mainPhoto == null)
            {
                // 如果沒有主要照片，您可以創建一個默認的 Photo 實例或處理此情況
                mainPhoto = new Photo { /* 設置默認值 */ };
            }

            // 獲取環境照片列表（EnvironmentPhotos）
            var environmentPhotos = restaurant.Photos.Where(p => p.PhotoType == "EnvironmentPhoto").ToList();

            // 獲取菜單照片（MenuPhoto）
            var menuPhoto = restaurant.Photos.FirstOrDefault(p => p.PhotoType == "MenuPhoto");
            if (menuPhoto == null)
            {
                // 如果沒有菜單照片，創建默認值或處理此情況
                menuPhoto = new Photo { /* 設置默認值 */ };
            }

            // 獲取公告內容，將公告內容分段
            var announcementParagraphs = restaurant.Announcements
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => a.Content)
                .ToList();

            // 獲取總評論數量
            var totalReviews = restaurant.Reviews.Count();

            // 計算總頁數
            int totalPages = (int)Math.Ceiling((double)totalReviews / pageSize);

            // 確保頁碼在有效範圍內
            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            // 根據頁碼和每頁大小取得當前頁面的評論
            var reviews = restaurant.Reviews
                .OrderByDescending(r => r.ReviewDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // 構建視圖模型，為所有 required 屬性提供值
            var model = new RestaurantViewModel
            {
                Restaurant = restaurant,
                MainPhoto = mainPhoto,
                EnvironmentPhotos = environmentPhotos,
                MenuPhoto = menuPhoto,
                AnnouncementParagraphs = announcementParagraphs,
                Reviews = reviews,
                CurrentPage = page,
                TotalPages = totalPages,
                RestaurantId = id,
                // 如果有需要，可以設置 IsFavorite
            };

            return PartialView("_ReviewsPartial", model);
        }

    }
}
