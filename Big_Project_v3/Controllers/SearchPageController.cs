using Microsoft.AspNetCore.Mvc;

namespace Big_Project_v3.Controllers
{
    public class SearchPageController : Controller // 繼承 Controller 類別，才能使用 MVC 的功能
    {
        // 定義一個 Index 方法，返回視圖
        public IActionResult Index()
        {
            return View(); // 返回對應的視圖，預設會尋找 Views/SearchPage/Index.cshtml
        }
    }
}