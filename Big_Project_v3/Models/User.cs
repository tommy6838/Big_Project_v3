using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Big_Project_v3.Models;

public partial class User
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "使用者名稱是必填的。")]
    [StringLength(50, ErrorMessage = "使用者名稱不能超過 50 個字元。")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "密碼是必填的。")]
    [DataType(DataType.Password)]
    public string PasswordHash { get; set; }

    [Required(ErrorMessage = "姓名是必填的。")]
    [StringLength(100, ErrorMessage = "姓名不能超過 100 個字元。")]
    public string Name { get; set; }

    [Required(ErrorMessage = "聯絡電話是必填的。")]
    [Phone(ErrorMessage = "請輸入有效的聯絡電話。")]
    public string ContactPhone { get; set; }

    [Required(ErrorMessage = "電子郵件是必填的。")]
    [EmailAddress(ErrorMessage = "請輸入有效的電子郵件地址。")]
    public string ContactEmail { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<PasswordResetRequest> PasswordResetRequests { get; set; } = new List<PasswordResetRequest>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
