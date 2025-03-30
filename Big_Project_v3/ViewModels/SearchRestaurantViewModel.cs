using System.Collections.Generic;
using Big_Project_v3.Models;

namespace Big_Project_v3.ViewModels
{
    public class SearchRestaurantViewModel
    {
        public IEnumerable<RestaurantSearchResultViewModel> Restaurants { get; set; } = new List<RestaurantSearchResultViewModel>();
        public string? SearchKeyword { get; set; }
        public string? PhotoURL { get; set; } // 新增屬性

        public int Id { get; set; }
        public string? Name { get; set; }
        public double AverageRating { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public bool IsReservationOpen { get; set; }
    }
}
