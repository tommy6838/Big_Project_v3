using Big_Project_v3.Models;

namespace Big_Project_v3.ViewModels
{
    public class RestaurantViewModel
    {
        public required Restaurant Restaurant { get; set; } // 餐廳主要資料
        public required Photo MainPhoto { get; set; } // 首圖
        public required List<Photo> EnvironmentPhotos { get; set; } // 餐廳環境照片
        public required Photo MenuPhoto { get; set; } // 菜單照片                                                  
        public required List<string> AnnouncementParagraphs { get; set; }  // 分段公告

        public required List<Review> Reviews { get; set; }
        public bool IsFavorite { get; set; } // 用來判斷是否已收藏
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int RestaurantId { get; set; }

    }
}
