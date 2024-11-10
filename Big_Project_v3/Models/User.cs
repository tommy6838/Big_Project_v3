using System;
using System.Collections.Generic;

namespace Big_Project_v3.Models;

public partial class User
{
    /// <summary>
    /// 使用者的唯一識別碼，主鍵（PK），自動遞增
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 用於登入的帳號名稱
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 密碼的哈希值
    /// </summary>
    public string PasswordHash { get; set; } = null!;

    /// <summary>
    /// 使用者姓名
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 聯絡電話
    /// </summary>
    public string? ContactPhone { get; set; }

    /// <summary>
    /// 聯絡電子郵件
    /// </summary>
    public string? ContactEmail { get; set; }

    /// <summary>
    /// 帳號建立時間
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 帳號更新時間
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<PasswordResetRequest> PasswordResetRequests { get; set; } = new List<PasswordResetRequest>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
