using Big_Project_v3.Models;
using Big_Project_v3.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Net;
using Big_Project_v3.Views.Shared.PartialView;

namespace Big_Project_v3.Controllers
{
	public class HomePageController : Controller
	{
		private readonly ITableDbContext _context;
		public HomePageController(ITableDbContext context)
		{
			_context = context;
		}
		[HttpGet]
		public IActionResult Index(string district = "南屯區")
		{
			DateTime currentTime = DateTime.Now;
			var HPModel = _context.Restaurants
				.GroupJoin(_context.Reviews,
					restaurant => restaurant.RestaurantId,
					review => review.RestaurantId,
					(restaurant, reviews) => new { restaurant, reviews })
				.SelectMany(
					x => x.reviews.DefaultIfEmpty(),
					(x, review) => new { x.restaurant, review }
				)
				.GroupJoin(_context.Photos,
					rr => rr.restaurant.RestaurantId,
					photo => photo.RestaurantId,
					(rr, photos) => new { rr.restaurant, rr.review, photos })
				.SelectMany(
					x => x.photos.DefaultIfEmpty(),
					(x, photo) => new { x.restaurant, x.review, photo }
				)
				.GroupJoin(_context.Users,
					rrp => rrp.review != null ? rrp.review.UserId : 0,  // Use conditional for null reviews
					user => user.UserId,
					(rrp, users) => new { rrp.restaurant, rrp.review, rrp.photo, users })
				.SelectMany(
					x => x.users.DefaultIfEmpty(),
					(x, user) => new { x.restaurant, x.review, x.photo, user }
				)
				.ToList()  // Execute the query and switch to in-memory processing
				.Select(x => new HomePageItems
				{
					RestaurantID = x.restaurant.RestaurantId,
					RestaurantName = x.restaurant.Name,
					RestaurantAddress = x.restaurant.Address,
					RestaurantRating = (double)x.restaurant.AverageRating,
					IsReservationOpen = x.restaurant.IsReservationOpen,
					ReviewID = x.review != null ? x.review.ReviewId : 0,
					ReviewScore = (int)(x.review != null ? x.review.Rating : 0),
					ReviewContent = x.review != null ? x.review.ReviewText : "No Review",
					ReviewDate = x.review?.ReviewDate.HasValue ?? false
						? x.review.ReviewDate.Value.ToDateTime(TimeOnly.MinValue)
						: DateTime.MinValue,
					Description = "立即訂位🔥🔥", // x.restaurant.Description
					ImageUrl = x.photo != null ? x.photo.PhotoUrl : "/img/logo.png",    //x.photo != null ? x.photo.PhotoURL : "/img/default.jpg"
					CustomerName = x.user != null ? x.user.Name : "Anonymous",
					PriceRange = Convert.ToInt32(x.restaurant.PriceRange.Substring(x.restaurant.PriceRange.IndexOf('$') + 1).Replace(",", "")),
					Url = "#"
				})
				.GroupBy(x => x.RestaurantID)
				.Select(g => g.First())
				.ToList();


			var HPReviewModel = _context.Reviews
				.Join(_context.Restaurants,
					review => review.RestaurantId,
					restaurant => restaurant.RestaurantId,
					(review, restaurant) => new { review, restaurant })
				.Join(_context.Photos.Where(photo => photo.PhotoType == "首圖"), // 僅篩選 "首圖"
					  rr => rr.restaurant.RestaurantId,
					  photo => photo.RestaurantId,
					  (rr, photo) => new { rr, photo })
				.Join(_context.Users,
					  rrp => rrp.rr.review.UserId,
					  user => user.UserId,
					  (rrp, user) => new HomePageItems
					  {
						  ReviewID = rrp.rr.review.ReviewId,
						  ReviewScore = (int)rrp.rr.review.Rating,
						  ReviewContent = rrp.rr.review.ReviewText,
						  ReviewDate = rrp.rr.review.ReviewDate.HasValue
							? rrp.rr.review.ReviewDate.Value.ToDateTime(TimeOnly.MinValue)
							: DateTime.MinValue,
						  RestaurantID = rrp.rr.restaurant.RestaurantId,
						  RestaurantName = rrp.rr.restaurant.Name,
						  RestaurantAddress = rrp.rr.restaurant.Address,
						  RestaurantRating = (double)rrp.rr.restaurant.AverageRating,
						  Description = "立即訂位🔥🔥", // rrp.rr.restaurant.Description
						  ImageUrl = rrp.photo != null ? rrp.photo.PhotoUrl : "/img/logo.png", // 若有"首圖"，取其 URL，否則預設
						  CustomerName = user.Name,
						  PriceRange = Convert.ToInt32(rrp.rr.restaurant.PriceRange.Substring(rrp.rr.restaurant.PriceRange.IndexOf('$') + 1).Replace(",", "")),
						  Url = "#"
					  })
				.GroupBy(x => x.ReviewID) // 按評論ID分組
				.Select(g => g.First()) // 取得每個評論的第一筆記錄
				.ToList();

			var HPActiveModel = _context.Restaurants
				.GroupJoin(_context.Reviews,
					restaurant => restaurant.RestaurantId,
					review => review.RestaurantId,
					(restaurant, reviews) => new { restaurant, reviews })
				.SelectMany(
					x => x.reviews.DefaultIfEmpty(),
					(x, review) => new { x.restaurant, review }
				)
				.GroupJoin(_context.Photos,
					rr => rr.restaurant.RestaurantId,
					photo => photo.RestaurantId,
					(rr, photos) => new { rr.restaurant, rr.review, photos })
				.SelectMany(
					x => x.photos.DefaultIfEmpty(),
					(x, photo) => new { x.restaurant, x.review, photo }
				)
				.GroupJoin(_context.Announcements,
					rrp => rrp.restaurant.RestaurantId,  // Use conditional for null reviews
					announcement => announcement.RestaurantId,
					(rrp, announcements) => new { rrp.restaurant, rrp.review, rrp.photo, announcements })
				.SelectMany(
					x => x.announcements.DefaultIfEmpty(),
					(x, announcements) => new { x.restaurant, x.review, x.photo, announcements }
				)
				.ToList()  // Execute the query and switch to in-memory processing
				.Select(x => new HomePageItems
				{
					RestaurantID = x.restaurant.RestaurantId,
					RestaurantName = x.restaurant.Name,
					RestaurantAddress = x.restaurant.Address,
					RestaurantRating = (double)x.restaurant.AverageRating,
					IsReservationOpen = x.restaurant.IsReservationOpen,

					ReviewID = x.review != null ? x.review.ReviewId : 0,
					ReviewScore = (int)(x.review != null ? x.review.Rating : 0),
					ReviewContent = x.review != null ? x.review.ReviewText : "No Review",
					ReviewDate = x.review?.ReviewDate.HasValue ?? false
						? x.review.ReviewDate.Value.ToDateTime(TimeOnly.MinValue)
						: DateTime.MinValue,
					Description = x.announcements != null ? x.announcements.Title + "~" : "", // x.restaurant.Description
					ImageUrl = x.photo != null ? x.photo.PhotoUrl : "/img/logo.png",    //x.photo != null ? x.photo.PhotoURL : "/img/default.jpg"
					Title = x.announcements != null ? x.announcements.Title : "",
					Content = x.announcements != null ? x.announcements.Content : "",
					PriceRange = Convert.ToInt32(x.restaurant.PriceRange.Substring(x.restaurant.PriceRange.IndexOf('$') + 1).Replace(",", "")),
					Url = "#"
				})
				.GroupBy(x => x.RestaurantID)
				.Select(g => g.First())
				.ToList();


			TimeSpan comparisonTime = new TimeSpan(11, 0, 0);

			var restaurants = HPModel.Where(vm => vm.RestaurantRating > 4.5 && vm.IsReservationOpen == true).Select(vm => new HomePageItems
			{
				RestaurantID = vm.RestaurantID,
				RestaurantName = vm.RestaurantName,
				RestaurantAddress = vm.RestaurantAddress,
				Description = vm.Description,
				RestaurantRating = vm.RestaurantRating,
				ImageUrl = vm.ImageUrl,
				PriceRange = vm.PriceRange,
				Url = vm.Url
			}).ToList();

			var nearbyRestaurants = HPModel.Where(vm => vm.RestaurantAddress.Contains(district) && vm.IsReservationOpen == true).Select(vm => new HomePageItems
			{
				RestaurantID = vm.RestaurantID,
				RestaurantName = vm.RestaurantName,
				RestaurantAddress = vm.RestaurantAddress,
				Description = vm.Description,
				RestaurantRating = vm.RestaurantRating,
				ImageUrl = vm.ImageUrl,
				PriceRange = vm.PriceRange,
				Url = vm.Url
			}).ToList();
			var reviews = HPReviewModel.Select(vm => new HomePageItems
			{
				RestaurantID = vm.RestaurantID,
				RestaurantName = vm.RestaurantName,
				RestaurantAddress = vm.RestaurantAddress,
				RestaurantRating = vm.RestaurantRating,
				ImageUrl = vm.ImageUrl,
				PriceRange = vm.PriceRange,
				CustomerName = vm.CustomerName,
				ReviewScore = vm.ReviewScore,
				ReviewContent = vm.ReviewContent,
				ReviewDate = vm.ReviewDate,
			}).ToList();

			var activeRestaurants = HPActiveModel.Where(vm => !string.IsNullOrWhiteSpace(vm.Title) && vm.IsReservationOpen == true).Select(vm => new HomePageItems
			{
				RestaurantID = vm.RestaurantID,
				RestaurantName = vm.RestaurantName,
				RestaurantAddress = vm.RestaurantAddress,
				Description = vm.Description,
				RestaurantRating = vm.RestaurantRating,
				ImageUrl = vm.ImageUrl,
				PriceRange = vm.PriceRange,
				Url = vm.Url,
				Title = vm.Title,
				Content = vm.Content,
			}).ToList();


			var viewModel = new HomePageViewModel
			{
				Restaurants = restaurants,
				NearbyRestaurants = nearbyRestaurants,
				Reviews = reviews,
				ActiveRestaurants = activeRestaurants,
				SearchModel = new _SearchBarModel
				{

				}
			};


			return View(viewModel);
		}

	}
}
