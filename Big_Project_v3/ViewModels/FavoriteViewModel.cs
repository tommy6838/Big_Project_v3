using Big_Project_v3.Models;

namespace Big_Project_v3.ViewModels
{
    public class FavoriteViewModel
    {
        public int FavoriteId { get; set; } // 主鍵
        public int? UserId { get; set; }    // 使用者ID
        public int? RestaurantId { get; set; } // 餐廳ID

        public double AverageRating { get; set; } // 評分
        public string? Description { get; set; } // 餐廳ID
        //public DateTime? AddedAt { get; set; } // 收藏的日期時間

        public string Name { get; set; } // 餐廳名稱
        public string AddedAt { get; set; }        // 收藏日期

        public string ?PhotoUrl { get; set; }
        public string PhotoType { get; set; }
        //public virtual Restaurant? Restaurant { get; set; } // 關聯到餐廳表
    }

}
