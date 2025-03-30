using System.ComponentModel.DataAnnotations;

namespace Big_Project_v3.ViewModels
{
    public class EditReviewViewModel
    {
        public int ReviewID { get; set; }

        public int RestaurantID { get; set; }

        public double Rating { get; set; }

        public string ReviewText { get; set; }
    }
}
