using System;
using System.Collections.Generic;

namespace Big_Project_v3.Models;

public partial class RestaurantAvailability
{
    public int AvailabilityId { get; set; }

    public int RestaurantId { get; set; }

    public DateOnly? Date { get; set; }

    public TimeOnly? TimeSlot { get; set; }

    public int? MaxCapacity { get; set; }

    public int? AvailableSeats { get; set; }

    /// <summary>
    /// 資料更新時間（前台 + 後台）。此欄位記錄可用時段紀錄的最後更新時間，用於追蹤資料變更歷史。
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    public virtual Restaurant Restaurant { get; set; } = null!;
}
