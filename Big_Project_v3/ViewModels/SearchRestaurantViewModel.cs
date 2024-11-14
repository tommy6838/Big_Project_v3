// 將此檔案放在 ViewModels 資料夾中，例如 Big_Project_v3/ViewModels/SearchRestaurantViewModel.cs
using System.Collections.Generic;
using Big_Project_v3.Models; // 假設 Restaurant 模型定義在 Models 資料夾

namespace Big_Project_v3.ViewModels
{
    public class SearchRestaurantViewModel
    {

        public IEnumerable<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
        //public IEnumerable<Restaurant> Restaurants { get; set; }
        // 儲存多個餐廳的搜尋結果
        public string ?SearchKeyword { get; set; } // 儲存使用者的搜尋關鍵字
    }
}
