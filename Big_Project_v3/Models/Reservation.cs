using System;
using System.Collections.Generic;

namespace Big_Project_v3.Models;

public partial class Reservation
{
    /// <summary>
    /// 訂位的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個訂位紀錄，並在資料表之間建立關聯。
    /// </summary>
    public int ReservationId { get; set; }

    /// <summary>
    /// 餐廳ID，外鍵（FK）連結到 Restaurants 表（前台 + 後台）。此欄位指定訂位所屬的餐廳。
    /// </summary>
    public int RestaurantId { get; set; }

    /// <summary>
    /// 使用者ID，外鍵（FK）連結到 Users 表（前台 + 後台）。此欄位指定訂位的使用者。
    /// </summary>
    public int? UserId { get; set; }

    /// <summary>
    /// 訂位日期（前台 + 後台）。此欄位指定訂位的日期，用於安排和管理。
    /// </summary>
    public DateOnly? ReservationDate { get; set; }

    /// <summary>
    /// 訂位時間（前台 + 後台）。此欄位指定訂位的具體時間，用於安排和管理。
    /// </summary>
    public TimeOnly? ReservationTime { get; set; }

    /// <summary>
    /// 大人人數（前台 + 後台）。此欄位指定訂位時預定的大人人數，用於餐廳準備座位和資源。
    /// </summary>
    public int? NumAdults { get; set; }

    /// <summary>
    /// 小孩人數（前台 + 後台）。此欄位指定訂位時預定的小孩人數，用於餐廳準備座位和資源。
    /// </summary>
    public int? NumChildren { get; set; }

    /// <summary>
    /// 訂位人姓名（前台 + 後台）。此欄位存儲發起訂位的使用者姓名，用於確認和聯絡。
    /// </summary>
    public string? BookerName { get; set; }

    /// <summary>
    /// 訂位人電話（前台 + 後台）。此欄位存儲發起訂位的使用者電話號碼，用於確認和聯絡。
    /// </summary>
    public string? BookerPhone { get; set; }

    /// <summary>
    /// 訂位人電子郵件（前台 + 後台）。此欄位存儲發起訂位的使用者電子郵件地址，用於確認和聯絡。
    /// </summary>
    public string? BookerEmail { get; set; }

    /// <summary>
    /// 特殊要求備註（前台 + 後台）。此欄位存儲使用者對訂位的特殊要求或備註，如過敏資訊、座位偏好等。
    /// </summary>
    public string? SpecialRequests { get; set; }

    /// <summary>
    /// 訂位狀態（例如：已確認、已取消）（前台 + 後台）。此欄位指示訂位的當前狀態，方便管理和追蹤。
    /// </summary>
    public string? ReservationStatus { get; set; }

    /// <summary>
    /// 訂位建立時間（前台 + 後台）。此欄位記錄訂位紀錄的建立時間，用於審計和管理。
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// 訂位更新時間（前台 + 後台）。此欄位記錄訂位紀錄的最後更新時間，用於追蹤資料變更歷史。
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    public virtual Restaurant? Restaurant { get; set; }

    public virtual User? User { get; set; }
}
