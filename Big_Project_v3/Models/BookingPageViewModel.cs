using Big_Project_v3.ViewModels;

namespace Big_Project_v3.Models
{
    public class BookingPageViewModel
    {
        public List<Announcement>? Announcements { get; set; }
        public List<Favorite>? Favorites { get; set; }
        public List<Reservation>? Reservations { get; set; }
        public List<ReservationControlSetting>? ReservationControlSettings { get; set; }
        public List<Restaurant>? Restaurants { get; set; }
        public List<RestaurantAvailability>? RestaurantAvailabilities { get; set; }
        //public List<RestaurantBusinessHour>? RestaurantBusinessHours { get; set; }
        public List<RestaurantUser>? RestaurantUsers { get; set; }
        public List<ReviewViewModel> Reviews { get; set; } = new List<ReviewViewModel>();
        public List<User>? Users { get; set; }
        public List<Photo>? Photos { get; set; }
        public List<PasswordResetRequest>? PasswordResetRequests { get; set; }


        // 新增 UserID 屬性
        public int? UserId { get; set; }
        // 新增 PhotoUrl 屬性
        public string? PhotoUrl { get; set; }
    }
}