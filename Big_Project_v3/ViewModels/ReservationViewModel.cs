namespace Big_Project_v3.ViewModels
{
    public class ReservationViewModel
    {
        public int RestaurantID { get; set; }
        public string Name { get; set; }
        public string ReservationStatus { get; set; }
        public int NumAdults { get; set; }
        public int NumChildren { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeSpan ReservationTime { get; set; }

        public string RestaurantName { get; set; } // 餐廳名稱
        public string AddedAt { get; set; }        // 收藏日期
        public string? PhotoUrl { get; set; }
        public string PhotoType { get; set; }
        public string SpecialRequests { get; set; }

        // 新增的屬性
        public int? ReviewID { get; set; }  // 新增的屬性來儲存 ReviewID
        public string IsReviewLocked { get; set; } // "可新增"、"可修改"、"不可改"
    }
}
