using Big_Project_v3.Models;
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
        public IActionResult SearchRestaurants(string UesrSearch)
        {

            // 使用 Entity Framework 查詢資料庫中的餐廳資料
            var results = string.IsNullOrWhiteSpace(UesrSearch)
            ? _context.Restaurants.ToList() // 如果沒有關鍵字，回傳所有餐廳
            : _context.Restaurants
                    .Where(r => r.Name.Contains(UesrSearch) || r.Address.Contains(UesrSearch))
                    .ToList(); // 依據關鍵字篩選餐廳名稱或位置

            return PartialView("PartialView/_SearchRestaurant", results);
            // 將結果轉換為 JSON 格式回傳
        }
    }
}