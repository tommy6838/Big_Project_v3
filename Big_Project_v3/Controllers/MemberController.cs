using Big_Project_v3.Models;
using Big_Project_v3.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Big_Project_v3.Controllers
{
    public class MemberController : Controller
    {
        private readonly ITableDbContext _context;

        public MemberController(ITableDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "User");
            }

            // 查詢用戶資料
            var user = await _context.Users
                .Where(u => u.UserId == userId)
                .Select(u => new UserViewModel
                {
                    UserID = u.UserId,
                    UserName = u.UserName,
                    Name = u.Name,
                    Reservations = _context.Reservations
                        .Where(r => r.UserId == userId)
                        .OrderByDescending(r => r.ReservationDate) // 第一排序條件：日期由近到遠
                        .ThenByDescending(r => r.ReservationTime)  // 第二排序條件：時間由早到晚
                        .Select(r => new ReservationViewModel
                        {
                            Name = r.Restaurant != null ? r.Restaurant.Name : "未知餐廳",
                            RestaurantID = r.RestaurantId,
                            ReservationStatus = r.ReservationStatus,
                            NumAdults = r.NumAdults ?? 0,
                            NumChildren = r.NumChildren ?? 0,
                            SpecialRequests = r.SpecialRequests,
                            ReservationDate = r.ReservationDate.HasValue
                                ? r.ReservationDate.Value.ToDateTime(TimeOnly.MinValue)
                                : DateTime.MinValue,
                            ReservationTime = r.ReservationTime.HasValue
                                ? r.ReservationTime.Value.ToTimeSpan()
                                : TimeSpan.Zero,
                            PhotoUrl = _context.Photos              // 從 Photos 表中查詢餐廳圖片
                                .Where(p => p.RestaurantId == r.RestaurantId && p.PhotoType == "LOGO")
                                .Select(p => p.PhotoUrl)
                                .FirstOrDefault(),                  // 僅取第一張圖片

                            // 查詢 ReviewID 和 IsReviewLocked，確保兩者對應同一條記錄
                            ReviewID = _context.Reviews
                                .Where(review => review.RestaurantId == r.RestaurantId && review.UserId == userId)
                                .OrderByDescending(review => review.ReviewId) // 以 ReviewID 進行降序排序，獲取最新的評論
                                .Select(review => review.ReviewId)
                                .FirstOrDefault(),

                            IsReviewLocked = _context.Reviews
                           .Where(review => review.ReviewId == _context.Reviews
                                .Where(innerReview => innerReview.RestaurantId == r.RestaurantId && innerReview.UserId == userId)
                                .OrderByDescending(innerReview => innerReview.ReviewId)
                                .Select(innerReview => innerReview.ReviewId)
                                .FirstOrDefault())
                    .Select(review => review.IsReviewLocked)
                    .FirstOrDefault()
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();


            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            return View(user);

        }



        [HttpGet]
        public async Task<IActionResult> LoadPartialView(string partialViewName)
        {
            // 從 Session 取得目前使用者的 UserId
            var userId = HttpContext.Session.GetInt32("UserId");

            // 如果未登錄，重定向到登入頁面
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "User");
            }

            // 訂位邏輯
            if (partialViewName == "_Reservation")
            {
                var reservations = await _context.Reservations
                    .Include(r => r.Restaurant) // 手動載入與餐廳的關聯資料
                    .Where(r => r.UserId == userId)
                    .OrderByDescending(r => r.ReservationDate) // 第一排序條件：日期由近到遠
                    .ThenByDescending(r => r.ReservationTime)  // 第二排序條件：時間由早到晚
                    .Select(r => new ReservationViewModel
                    {
                        RestaurantID = r.RestaurantId,
                        Name = r.Restaurant != null ? r.Restaurant.Name : "未知餐廳",
                        ReservationStatus = r.ReservationStatus,
                        NumAdults = r.NumAdults ?? 0,
                        NumChildren = r.NumChildren ?? 0,
                        SpecialRequests = r.SpecialRequests,
                        ReservationDate = r.ReservationDate.HasValue
                            ? r.ReservationDate.Value.ToDateTime(TimeOnly.MinValue)
                            : DateTime.MinValue,
                        ReservationTime = r.ReservationTime.HasValue
                            ? r.ReservationTime.Value.ToTimeSpan()
                            : TimeSpan.Zero,
                        PhotoUrl = _context.Photos              // 從 Photos 表中查詢餐廳圖片
                            .Where(p => p.RestaurantId == r.RestaurantId && p.PhotoType == "LOGO")
                            .Select(p => p.PhotoUrl)
                            .FirstOrDefault(),                  // 僅取第一張圖片

                        // 查詢 ReviewID 和 IsReviewLocked，確保兩者對應同一條記錄
                        ReviewID = _context.Reviews
                                .Where(review => review.RestaurantId == r.RestaurantId && review.UserId == userId)
                                .OrderByDescending(review => review.ReviewId) // 以 ReviewID 進行降序排序，獲取最新的評論
                                .Select(review => review.ReviewId)
                                .FirstOrDefault(),

                        IsReviewLocked = _context.Reviews
                           .Where(review => review.ReviewId == _context.Reviews
                                .Where(innerReview => innerReview.RestaurantId == r.RestaurantId && innerReview.UserId == userId)
                                .OrderByDescending(innerReview => innerReview.ReviewId)
                                .Select(innerReview => innerReview.ReviewId)
                                .FirstOrDefault())
                    .Select(review => review.IsReviewLocked)
                    .FirstOrDefault()
                    })
                    .ToListAsync();

                // 如果使用者沒有訂位記錄，返回搜尋欄的部分視圖
                if (!reservations.Any())
                {
                    return Content("<h1>您目前沒有任何訂位記錄</h1>", "text/html");
                }

                // 傳回訂位記錄部分視圖
                return PartialView("PartialView/_MemberFolder/_Reservation", reservations);
            }

            // 珍藏餐廳邏輯
            if (partialViewName == "_FavoriteRestaurants")
            {
                // 如果未登錄，重定向到登入頁面
                if (!userId.HasValue)
                {
                    return RedirectToAction("Login", "User");
                }

                var favoriteRestaurants = await _context.Favorites
                    .Where(f => f.UserId == userId) // 過濾目前的使用者
                    .Include(f => f.Restaurant)    // 載入關聯的餐廳資料
                    .OrderByDescending(f => f.AddedAt)  // 根據 AddedAt 降序排序
                    .ThenByDescending(f => f.FavoriteId) // 根據 FavoriteId 降序排序
                    .Select(f => new FavoriteViewModel
                    {
                        Name = f.Restaurant.Name,
                        Description = f.Restaurant.Description,
                        PhotoUrl = _context.Photos
                            .Where(p => p.RestaurantId == f.RestaurantId && p.PhotoType == "LOGO")
                            .Select(p => p.PhotoUrl)
                            .FirstOrDefault(),
                        AddedAt = f.AddedAt.HasValue ? f.AddedAt.Value.ToString("yyyy/MM/dd") : "未知日期",
                        AverageRating = f.Restaurant.AverageRating ?? 0,
                        RestaurantId = f.RestaurantId // 確保設置 RestaurantId
                    })
                    .ToListAsync();

                // 如果使用者沒有收藏任何餐廳，回傳提示訊息
                if (!favoriteRestaurants.Any())
                {
                    return Content("<h1>您目前沒有收藏任何餐廳</h1>", "text/html");
                }

                // 傳回收藏餐廳部分視圖
                return PartialView("PartialView/_MemberFolder/_FavoriteRestaurants", favoriteRestaurants);
            }

            if (partialViewName == "_Comment")
            {
                var reviews = await _context.Reviews
                    .Where(r => r.UserId == userId)      // 過濾目前的使用者
                    .Include(r => r.Restaurant)          // 顯式載入關聯的餐廳資料
                    .OrderByDescending(r => r.ReviewDate) // 依 ReviewDate 降序排序
                    .ThenByDescending(r => r.ReviewId)   // 若日期相同則依 ReviewID 降序排序
                    .Select(r => new ReviewViewModel
                    {
                        ReviewID = r.ReviewId,           // 評論的 ID
                        Rating = r.Rating ?? 0,          // 評分
                        ReviewText = r.ReviewText,       // 評論文字
                        RestaurantID = r.RestaurantId ?? 0,   // 餐廳的 ID
                        ReviewDate = r.ReviewDate.HasValue
                            ? r.ReviewDate.Value.ToDateTime(TimeOnly.MinValue)
                            : DateTime.MinValue,         // 評論日期
                        RestaurantName = r.Restaurant != null
                            ? r.Restaurant.Name
                            : "未知餐廳",                // 餐廳名稱
                        PhotoURL = _context.Photos
                            .Where(p => p.RestaurantId == r.RestaurantId && p.PhotoType == "LOGO")
                            .Select(p => p.PhotoUrl)
                            .FirstOrDefault()            // 餐廳圖片
                    })
                    .ToListAsync();

                // 如果沒有評論記錄，顯示空評論訊息
                if (!reviews.Any())
                {
                    return Content("<h4>你的評論</h4><p>目前沒有評論記錄。</p>", "text/html");
                }

                // 返回評論部分視圖
                return PartialView("PartialView/_MemberFolder/_Comment", reviews);
            }

            // 如果請求的是其他部分視圖
            if (!string.IsNullOrEmpty(partialViewName))
            {
                return PartialView($"PartialView/_MemberFolder/{partialViewName}");
            }

            // 預設回傳錯誤頁面或空白視圖（避免缺少回傳值路徑）
            return BadRequest("Invalid partial view name.");
        }

        // 取消收藏
        // 取消收藏
        [HttpPost]
        [Route("Member/RemoveFavorite")]
        public async Task<IActionResult> RemoveFavorite([FromBody] FavoriteViewModel model)
        {
            if (model.RestaurantId == null)
            {
                return Json(new { success = false, message = "餐廳ID未提供" });
            }

            // 從 Session 獲取當前使用者的 UserID
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "使用者未登入" }); // 未登入
            }

            // 從資料庫中查詢收藏的餐廳資料
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.RestaurantId == model.RestaurantId);

            if (favorite == null)
            {
                return Json(new { success = false, message = "找不到對應的收藏資料" }); // 資料不存在
            }

            // 刪除收藏
            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "已取消珍藏" }); // 成功取消
        }

        // AddComment 方法
        public IActionResult AddComment(int restaurantId)
        {
            try
            {
                if (restaurantId <= 0)
                {
                    return BadRequest("無效的餐廳 ID");
                }

                var viewModel = new CreateReviewViewModel
                {
                    RestaurantID = restaurantId,
                    Rating = 0.0, // 預設為 0.0
                    ReviewText = string.Empty // 預設為空字串
                };

                // 返回 Shared 文件夾中的部分視圖
                return PartialView("~/Views/Shared/PartialView/_MemberFolder/_AddComment.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                // 錯誤日誌
                Console.WriteLine($"Error in AddComment: {ex.Message}");
                return StatusCode(500, "伺服器內部錯誤");
            }
        }


        // SubmitComment 方法
        [HttpPost]
        public async Task<IActionResult> SubmitComment([FromBody] CreateReviewViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "使用者未登入" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var review = new Review
                    {
                        UserId = userId.Value,
                        RestaurantId = model.RestaurantID,
                        Rating = model.Rating,
                        ReviewText = model.ReviewText,
                        ReviewDate = DateOnly.FromDateTime(DateTime.Now),
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsReviewLocked = "不可改" // 設置為 "不可改"
                    };

                    _context.Reviews.Add(review);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "評論已送出" });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in SubmitComment: {ex.Message}");
                    return Json(new { success = false, message = "評論提交失敗，請稍後再試。" });
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = string.Join("; ", errors) });
        }

        [HttpGet]
        public async Task<IActionResult> EditComment(int reviewId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "使用者未登入" });
            }

            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.ReviewId == reviewId && r.UserId == userId.Value);
            if (review == null)
            {
                return Json(new { success = false, message = "找不到評論資料" });
            }

            var viewModel = new EditReviewViewModel
            {
                ReviewID = review.ReviewId,
                RestaurantID = review.RestaurantId ?? 0,
                Rating = review.Rating ?? 0,
                ReviewText = review.ReviewText ?? string.Empty
            };

            return PartialView("~/Views/Shared/PartialView/_MemberFolder/_EditComment.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditComment([FromBody] EditReviewViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "使用者未登入" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var review = await _context.Reviews.FirstOrDefaultAsync(r => r.ReviewId == model.ReviewID && r.UserId == userId.Value);
                    if (review == null)
                    {
                        return Json(new { success = false, message = "找不到評論資料" });
                    }

                    review.Rating = model.Rating;
                    review.ReviewText = model.ReviewText;
                    review.UpdatedAt = DateTime.Now;
                    // IsReviewLocked 已經設置為 "不可改"，無需更改

                    _context.Reviews.Update(review);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "評論已更新" });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in EditComment: {ex.Message}");
                    return Json(new { success = false, message = "評論更新失敗，請稍後再試。" });
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = string.Join("; ", errors) });
        }


        //    [HttpGet]
        //    public async Task<IActionResult> LoadFavoriteRestaurants()
        //    {
        //        // 從 Session 中取得 UserId
        //        var userId = HttpContext.Session.GetInt32("UserId");

        //        // 如果未登入，重定向到登入頁
        //        if (!userId.HasValue)
        //        {
        //            return RedirectToAction("Login", "User");
        //        }

        //        // 查詢目前使用者的收藏餐廳
        //        var favoriteRestaurants = await _context.Favorites
        //            .Where(f => f.UserId == userId) // 過濾目前的使用者
        //            .Include(f => f.Restaurant)    // 顯式載入關聯的餐廳資料
        //            .Select(f => new FavoriteViewModel
        //            {
        //                Name = f.Restaurant.Name,
        //                AddedAt = f.AddedAt.HasValue ? f.AddedAt.Value.ToString("yyyy/MM/dd") : "未知日期",
        //                AverageRating = f.Restaurant.AverageRating ?? 0, // 餐廳評分
        //                Description = f.Restaurant.Description, // 餐廳描述
        //                RestaurantId = f.RestaurantId,      // 傳遞 Restaurant ID
        //            })
        //            .ToListAsync();

        //        var test = await _context.Favorites
        //.Where(f => f.UserId == userId)
        //.Include(f => f.Restaurant)
        //.ToListAsync();

        //        foreach (var favorite in test)
        //        {
        //            Console.WriteLine($"餐廳名稱: {favorite.Restaurant?.Name}, 描述: {favorite.Restaurant?.Description}");
        //        }


        //        foreach (var item in favoriteRestaurants)
        //        {
        //            Console.WriteLine($"餐廳名稱: {item.Name}, 評分: {item.AverageRating}, 收藏日期: {item.AddedAt}");
        //        }


        //        // 如果沒有收藏餐廳，顯示訊息
        //        if (!favoriteRestaurants.Any())
        //        {
        //            return Content("<h1>您目前沒有收藏任何餐廳</h1>", "text/html");
        //        }

        //        // 返回收藏餐廳的部分視圖
        //        return PartialView("PartialView/_MemberFolder/_FavoriteRestaurants", favoriteRestaurants);
        //    }

        //[HttpGet]
        //public async Task<IActionResult> TestFavoriteDescriptions()
        //{
        //    var userId = HttpContext.Session.GetInt32("UserId");

        //    if (!userId.HasValue)
        //    {
        //        return Content("尚未登入，無法測試。");
        //    }

        //    // 查詢收藏餐廳並載入關聯的餐廳資料
        //    var test = await _context.Favorites
        //        .Where(f => f.UserId == userId)
        //        .Include(f => f.Restaurant)
        //        .Select(f => new
        //        {
        //            FavoriteID = f.FavoriteId,
        //            RestaurantName = f.Restaurant.Name,
        //            RestaurantDescription = f.Restaurant.Description,
        //            AddedAt = f.AddedAt,
        //            RestaurantId = f.RestaurantId,      // 傳遞 Restaurant ID
        //        })
        //        .ToListAsync();
        //    Console.WriteLine($"查詢結果：找到 {test.Count} 筆記錄");
        //    foreach (var item in test)
        //    {
        //        Console.WriteLine($"FavoriteID: {item.FavoriteID}, 餐廳名稱: {item.RestaurantName}, 描述: {item.RestaurantDescription}");
        //    }
        //    // 將結果轉換成 JSON 返回
        //    return Json(test);
        //}

    }
}
