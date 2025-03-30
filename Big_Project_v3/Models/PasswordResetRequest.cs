using System;
using System.Collections.Generic;

namespace Big_Project_v3.Models;

public partial class PasswordResetRequest
{
    /// <summary>
    /// 請求的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個密碼重設請求，並在資料表之間建立關聯。
    /// </summary>
    public int RequestId { get; set; }

    /// <summary>
    /// 使用者ID，外鍵（FK）連結到 Users 表（前台 + 後台）。此欄位指定發起密碼重設請求的使用者。
    /// </summary>
    public int? UserId { get; set; }

    /// <summary>
    /// 餐廳管理者ID，外鍵（FK）連結到 RestaurantUsers 表（前台 + 後台）。此欄位指定發起密碼重設請求的餐廳管理者。
    /// </summary>
    public int? RestaurantUserId { get; set; }

    /// <summary>
    /// 密碼重設的唯一令牌（前台 + 後台）。此欄位存儲用於驗證和完成密碼重設的唯一標識符。
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// 令牌的過期時間（前台 + 後台）。此欄位指定密碼重設令牌的有效期限，超過此時間令牌將失效。
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// 請求建立時間（前台 + 後台）。此欄位記錄密碼重設請求的建立時間，用於審計和管理。
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    public virtual RestaurantUser? RestaurantUser { get; set; }

    public virtual User? User { get; set; }
}
