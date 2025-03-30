using System;
using System.Collections.Generic;

namespace Big_Project_v3.Models;

public partial class ReservationControlSetting
{
    /// <summary>
    /// 設定的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個訂位控管設定，並在資料表之間建立關聯。
    /// </summary>
    public int SettingId { get; set; }

    /// <summary>
    /// 餐廳ID，外鍵（FK）連結到 Restaurants 表（後台）。此欄位指定訂位控管設定所屬的餐廳。
    /// </summary>
    public int? RestaurantId { get; set; }

    /// <summary>
    /// 開放預訂的天數（後台）。此欄位指定餐廳允許提前預訂的天數範圍，用於限制預訂的時間跨度。
    /// </summary>
    public int? AdvanceBookingDays { get; set; }

    /// <summary>
    /// 關閉預訂的提前小時數（後台）。此欄位指定餐廳在預訂前需提前關閉訂位的時間，用於確保餐廳有足夠的準備時間。
    /// </summary>
    public int? CloseBookingBeforeHours { get; set; }

    /// <summary>
    /// 可預訂的時段區間（JSON 格式）（後台）。此欄位存儲餐廳可接受預訂的具體時段，以 JSON 格式表示，方便靈活管理。
    /// </summary>
    public string? AvailableTimeSlots { get; set; }

    /// <summary>
    /// 每次訂位最少人數（後台）。此欄位指定每次訂位所需的最少人數，用於控制訂位的規模。
    /// </summary>
    public int? MinPeoplePerReservation { get; set; }

    /// <summary>
    /// 每次訂位最多人數（後台）。此欄位指定每次訂位所允許的最大人數，用於控制訂位的規模。
    /// </summary>
    public int? MaxPeoplePerReservation { get; set; }

    /// <summary>
    /// 每個帳號限訂位次數（後台）。此欄位指定每個使用者帳號可進行的訂位次數上限，用於防止濫用預訂系統。
    /// </summary>
    public int? MaxReservationsPerAccount { get; set; }

    /// <summary>
    /// 設定建立時間。此欄位記錄訂位控管設定紀錄的建立時間，用於審計和管理。
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// 設定更新時間。此欄位記錄訂位控管設定紀錄的最後更新時間，用於追蹤資料變更歷史。
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    public virtual Restaurant? Restaurant { get; set; }
}
