using System;
using System.Collections.Generic;

namespace Big_Project_v3.Models;

public partial class Announcement
{
    /// <summary>
    /// 公告的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個公告紀錄，並在資料表之間建立關聯。
    /// </summary>
    public int AnnouncementId { get; set; }

    /// <summary>
    /// 餐廳ID，外鍵（FK）連結到 Restaurants 表（前台 + 後台）。此欄位指定該公告所屬的餐廳。
    /// </summary>
    public int? RestaurantId { get; set; }

    /// <summary>
    /// 公告標題（前台 + 後台）。此欄位存儲公告的標題，用於前台顯示和後台管理。
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 公告內容（前台 + 後台）。此欄位存儲公告的詳細內容，供前台用戶閱讀和後台管理。
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// 公告日期（前台 + 後台）。此欄位記錄公告的發布日期，用於排序和過濾。
    /// </summary>
    public DateTime? AnnouncementDate { get; set; }

    /// <summary>
    /// 公告開始日期（前台 + 後台）。此欄位指定公告的開始日期，決定公告何時開始顯示。
    /// </summary>
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// 公告結束日期（前台 + 後台）。此欄位指定公告的結束日期，決定公告何時停止顯示。
    /// </summary>
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// 公告建立時間（前台 + 後台）。此欄位記錄公告紀錄的建立時間，用於審計和管理。
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// 公告更新時間（前台 + 後台）。此欄位記錄公告紀錄的最後更新時間，用於追蹤資料變更歷史。
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    public virtual Restaurant? Restaurant { get; set; }
}
