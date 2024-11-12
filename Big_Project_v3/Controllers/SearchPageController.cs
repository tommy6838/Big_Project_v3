using Big_Project_v3.Models;
using Big_Project_v3.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Big_Project_v3.Controllers
{
    public class SearchPageController : Controller // 繼承 Controller 類別，才能使用 MVC 的功能
    {
        private readonly ITableDbContext _context;

        public SearchPageController(ITableDbContext context)
        {
            _context = context;
        }
        // 定義一個 Index 方法，返回視圖
        public IActionResult Index()
        {
            return View(); // 返回對應的視圖，預設會尋找 Views/SearchPage/Index.cshtml
        }

        [HttpPost]
        public IActionResult SearchRestaurants(string keyword)
        {
            // 查詢資料庫中符合關鍵字的餐廳
            var results = string.IsNullOrWhiteSpace(keyword)
                ? _context.Restaurants.ToList()
                : _context.Restaurants
                    .Where(r => r.Name.Contains(keyword) || r.Address.Contains(keyword))
                    .ToList();

            // 使用 ViewModel 傳遞資料
            var viewModel = new SearchRestaurantViewModel
            {   
                Restaurants = results,   // 將搜尋結果傳遞到 Restaurants
                SearchKeyword = keyword  // 傳遞使用者輸入的關鍵字
            };

            // 回傳部分檢視 `_SearchRestaurant` 並傳入 ViewModel
            return PartialView("PartialView/_SearchRestaurant", viewModel);
        }

        public IActionResult CheckDatabaseConnection()
        {
            try
            {
                // 試著取得資料庫中任意一筆資料，確保資料庫連線正常
                var isConnected = _context.Restaurants.Any();
                return Content(isConnected ? "Connected to Database" : "No Data Found in Database");
            }
            catch (Exception ex)
            {
                // 如果出現例外情況，回傳例外的訊息
                return Content($"Database connection failed: {ex.Message}");
            }
        }

    }
}