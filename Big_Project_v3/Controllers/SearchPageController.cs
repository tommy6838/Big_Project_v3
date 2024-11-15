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

        [HttpPost] // 指定此方法只接受 POST 請求
        //[Route("SearchPage/SearchRestaurants")]
        public IActionResult SearchRestaurants(string keyword) // 定義搜尋餐廳的方法，接受關鍵字參數
        {
            // 使用 Console.WriteLine 確認接收到的 keyword 是否正確
            Console.WriteLine("搜尋關鍵字: " + keyword);

            // 初始化查詢變數，將其設為 IQueryable，以便延遲查詢到資料庫層級
            //IQueryable<Restaurant> query = _context.Restaurants;

            // 若關鍵字為空白，直接查詢所有餐廳
            var results = string.IsNullOrWhiteSpace(keyword)
                ? _context.Restaurants.ToList()
                : _context.Restaurants
                    .Where(r => r.Name.Contains(keyword))
                    .ToList();
             

            //----------測試回傳數據----------
            Console.WriteLine("搜尋結果數量: " + results.Count);
            foreach (   var restaurant in results)
            {
                Console.WriteLine("符合條件的餐廳名稱: " + restaurant.Name);
            }
            //----------測試回傳數據----------

            // 建立 ViewModel，將篩選結果和關鍵字傳遞給部分檢視
            var viewModel = new SearchRestaurantViewModel
            {
                Restaurants = results,   // 將搜尋結果賦值給 ViewModel 中的 Restaurants 屬性
                SearchKeyword = keyword  // 將關鍵字賦值給 ViewModel 中的 SearchKeyword 屬性
            };

            // 返回部分檢視 `_SearchRestaurant`，並傳遞 ViewModel
            return PartialView("PartialView/_SearchRestaurant", viewModel); // 指定部分檢視路徑並提供資料
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