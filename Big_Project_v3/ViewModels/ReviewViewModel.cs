namespace Big_Project_v3.ViewModels
{
    public class ReviewViewModel
    {
        public int ReviewID { get; set; }
        public double Rating { get; set; }
        public string ?ReviewText { get; set; }
        public DateTime ReviewDate { get; set; }
        public string ?RestaurantName { get; set; } // 顯示評論所屬的餐廳名稱
        public string ?PhotoURL { get; set; }
        public int RestaurantID { get; set; }
        public string ?IsReviewLocked { get; set; }
    }
}
