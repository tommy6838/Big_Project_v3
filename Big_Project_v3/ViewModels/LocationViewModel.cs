namespace Big_Project_v3.ViewModels
{
    public class LocationViewModel
    {
        public double Latitude { get; set; } // 使用者的緯度
        public double Longitude { get; set; } // 使用者的經度
        public bool SortByDistance { get; set; } // 是否按距離排序
        public string? SearchKeyword { get; set; } // 儲存使用者的搜尋關鍵字
        public int Id { get; set; }
        public string? Name { get; set; }
        public double AverageRating { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public double Distance { get; set; } // 餐廳與使用者的距離
        public bool IsReservationOpen { get; set; }
        public string? PhotoURL { get; set; } // 新增的屬性

    }

}
