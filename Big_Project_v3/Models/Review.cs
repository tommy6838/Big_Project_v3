using System;
using System.Collections.Generic;

namespace Big_Project_v3.Models;

public partial class Review
{
    /// <summary>
    /// 評論的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個評論紀錄，並在資料表之間建立關聯。
    /// </summary>
    public int ReviewId { get; set; }

    /// <summary>
    /// 使用者ID，外鍵（FK）連結到 Users 表（前台 + 後台）。此欄位指定發表評論的使用者。
    /// </summary>
    public int? UserId { get; set; }

    /// <summary>
    /// 餐廳ID，外鍵（FK）連結到 Restaurants 表（前台 + 後台）。此欄位指定評論所屬的餐廳。
    /// </summary>
    public int? RestaurantId { get; set; }

    /// <summary>
    /// 評分（1-5 星）（前台 + 後台）。此欄位存儲使用者對餐廳的評分，用於計算平均評分和顯示給其他用戶。
    /// </summary>
    public double? Rating { get; set; }

    /// <summary>
    /// 評論內容（前台 + 後台）。此欄位存儲使用者對餐廳的詳細評論，用於提供其他用戶參考。
    /// </summary>
    public string? ReviewText { get; set; }

    /// <summary>
    /// 評論日期（前台 + 後台）。此欄位記錄評論的發表日期，用於排序和顯示。
    /// </summary>
    public DateOnly? ReviewDate { get; set; }

    /// <summary>
    /// 評論建立時間（前台 + 後台）。此欄位記錄評論紀錄的建立時間，用於審計和管理。
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// 評論更新時間（前台 + 後台）。此欄位記錄評論紀錄的最後更新時間，用於追蹤資料變更歷史。
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    public string? IsReviewLocked { get; set; } // 新增欄位

    public virtual Restaurant? Restaurant { get; set; }

    public virtual User? User { get; set; }
}
