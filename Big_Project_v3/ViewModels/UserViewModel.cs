namespace Big_Project_v3.ViewModels
{
    public class UserViewModel
    {
        //1.1 使用者表（Users）
        public int UserID { get; set; } // 使用者ID
        public string UserName { get; set; } // 使用者名稱
        public string Name { get; set; } // 姓名
        public string ContactPhone { get; set; } // 聯絡電話
        public string ContactEmail { get; set; } // 聯絡信箱

        //1.3 密碼重設請求表（PasswordResetRequests）
        public int RequestID { get; set; }

        //2.1 餐廳表（Restaurants）
        public int RestaurantID { get; set; }
        public string Address { get; set; }

        //2.3 餐廳圖片表（Photos）
        public int PhotoID { get; set; }
        public string PhotoURL { get; set; }

        //2.5 收藏表（Favorites）
        public DateTime AddedAt { get; set; }

        //3.1 訂位表（Reservations）
        public int ReservationID { get; set; }
        public DateOnly ReservationDate { get; set; } // 或使用 DateTime 
        public TimeSpan ReservationTime { get; set; }
        public int NumAdults { get; set; }
        public int NumChildren { get; set; }
        public string BookerName { get; set; }
        public string BookerPhone { get; set; }
        public string BookerEmail { get; set; }
        public string SpecialRequests { get; set; }
        public string ReservationStatus { get; set; }
        public DateTime CreatedAt { get; set; } // 建立時間

        //3.2 評論表（Reviews）
        public int Rating { get; set; }
        public string ReviewText { get; set; }
        public string ReviewDate { get; set; }

        // 新增屬性：用戶的訂位記錄
        public IEnumerable<ReservationViewModel> Reservations { get; set; }
    }
}
