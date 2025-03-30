using System;
using System.Collections.Generic;

namespace Big_Project_v3.Models;

public partial class Photo
{
    /// <summary>
    /// 相片的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個相片紀錄，並在資料表之間建立關聯。
    /// </summary>
    public int PhotoId { get; set; }

    /// <summary>
    /// 餐廳ID，外鍵（FK）連結到 Restaurants 表（前台 + 後台）。此欄位指定該相片所屬的餐廳。
    /// </summary>
    public int? RestaurantId { get; set; }

    /// <summary>
    /// 相片的 URL 或存儲路徑（前台 + 後台）。此欄位存儲相片的網路地址或在伺服器上的存儲路徑，用於前台顯示和管理。
    /// </summary>
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// 相片類型（例如：餐廳環境、菜單）（前台 + 後台）。此欄位描述相片的類型，方便分類和管理。
    /// </summary>
    public string? PhotoType { get; set; }

    /// <summary>
    /// 相片描述（前台 + 後台）。此欄位提供對相片的詳細描述，幫助前台用戶理解相片內容。
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 相片上傳時間（前台 + 後台）。此欄位記錄相片的上傳時間，用於管理和排序相片。
    /// </summary>
    public DateTime? UploadedAt { get; set; }

    public string? ImagePath { get; set; }

    public virtual Restaurant? Restaurant { get; set; }
}
