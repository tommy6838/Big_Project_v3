using Big_Project_v3.Views.Shared.PartialView;

namespace Big_Project_v3.Models
{
	public class HomePageViewModel
	{
		//public List<RestaurantCard>? Restaurants { get; set; }
		//public List<NearbyRestaurantCard>? NearbyRestaurants { get; set; }
		//public List<ReviewCard>? Reviews { get; set; }

		public List<HomePageItems>? Restaurants { get; set; }
		public List<HomePageItems>? NearbyRestaurants { get; set; }
		public List<HomePageItems>? Reviews { get; set; }
		public List<HomePageItems>? ActiveRestaurants { get; set; }
		public _SearchBarModel? SearchModel { get; set; }

	}
	public class HomePageItems
	{
		public int RestaurantID { get; set; }
		public string? RestaurantName { get; set; }  //修改過
		public string? Description { get; set; }
		public string? ImageUrl { get; set; }
		public string? Url { get; set; } // 跳轉到餐廳頁面
		public double RestaurantRating { get; set; }
		public int PriceRange { get; set; }
		public string? RestaurantAddress { get; set; }
		// 顧客資料
		public string? CustomerName { get; set; }
		public int ReviewScore { get; set; }
		// 評論資料
		public DateTime ReviewDate { get; set; }
		public string? ReviewContent { get; set; }
		public int ReviewID { get; set; }
		public int AnnouncementID { get; set; }
		public string? Title { get; set; }
		public string? Content { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public TimeOnly? BusinessHoursStart { get; set; }
		public bool IsReservationOpen { get; set; }
		public string? PhotoType { get; set; }

	}
}

