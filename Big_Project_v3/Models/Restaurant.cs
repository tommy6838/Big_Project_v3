using System;
using System.Collections.Generic;

namespace Big_Project_v3.Models;

public partial class Restaurant
{
    /// <summary>
    /// 餐廳的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個餐廳，並在資料表之間建立關聯。
    /// </summary>
    public int RestaurantId { get; set; }

    /// <summary>
    /// 餐廳名稱（前台 + 後台）。此欄位存儲餐廳的名稱，用於顯示和管理。
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 餐廳地址（前台 + 後台）。此欄位存儲餐廳的實體地址，用於定位和導航。
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// 聯絡電話（前台 + 後台）。此欄位存儲餐廳的聯絡電話號碼，用於客戶諮詢和緊急聯絡。
    /// </summary>
    public string? ContactPhone { get; set; }

    /// <summary>
    /// 餐廳簡介（前台 + 後台）。此欄位存儲餐廳的詳細介紹，包括特色、歷史等資訊，供客戶參考。
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 是否有停車場（1：有，0：無）（前台 + 後台）。此欄位指示餐廳是否提供停車設施，供客戶參考。
    /// </summary>
    public bool? HasParking { get; set; }

    /// <summary>
    /// 是否開放預訂（1：開放，0：關閉）（前台 + 後台）。此欄位指示餐廳是否接受預訂，供客戶決定是否進行預約。
    /// </summary>
    public bool IsReservationOpen { get; set; }

    /// <summary>
    /// 餐廳的平均評分（前台 + 後台）。此欄位存儲餐廳的平均客戶評分，供客戶參考和比較。
    /// </summary>
    public double? AverageRating { get; set; }

    /// <summary>
    /// 餐廳資料建立時間（前台 + 後台）。此欄位記錄餐廳資料的建立時間，用於審計和管理。
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// 餐廳資料更新時間（前台 + 後台）。此欄位記錄餐廳資料的最後更新時間，用於追蹤資料變更歷史。
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 營業開始時間
    /// </summary>
    public TimeOnly? BusinessHoursStart { get; set; }

    /// <summary>
    /// 營業結束時間
    /// </summary>
    public TimeOnly? BusinessHoursEnd { get; set; }

    /// <summary>
    /// 最後收客時間（可為空）
    /// </summary>
    public TimeOnly? LastCheckInTime { get; set; }

    public string? GoogleMapAddress { get; set; }

    public string? PriceRange { get; set; }

    //設定價格區間-子欣
    public (string Symbol, string Description) GetPriceRange()
    {
        // 若 PriceRange 為 null，直接回傳預設值
        if (string.IsNullOrEmpty(PriceRange))
            return ("N/A", "未知價格區間");

        // 移除非數字符號，並嘗試轉換為數值
        string cleanPrice = new string(PriceRange.Where(char.IsDigit).ToArray());
        if (decimal.TryParse(cleanPrice, out decimal price))
        {
            if (price < 500)
                return ("$", "$ 0-500");
            if (price < 1000)
                return ("$$", "$$ 500-1000");
            if (price < 1500)
                return ("$$$", "$$$ 1000-1500");
            return ("$$$$", "$$$$ 1500以上");
        }
        // 如果無法轉換，回傳預設值
        return ("N/A", "未知價格區間");
    }

    public virtual ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();

    public virtual ICollection<ReservationControlSetting> ReservationControlSettings { get; set; } = new List<ReservationControlSetting>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<RestaurantAvailability> RestaurantAvailabilities { get; set; } = new List<RestaurantAvailability>();

    public virtual ICollection<RestaurantUser> RestaurantUsers { get; set; } = new List<RestaurantUser>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
