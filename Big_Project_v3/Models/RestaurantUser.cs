using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Big_Project_v3.Models;

public partial class RestaurantUser
{
	/// <summary>
	/// 餐廳管理者的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個餐廳管理者，並在資料表之間建立關聯。
	/// </summary>
	public int RestaurantUserId { get; set; }

	/// <summary>
	/// 餐廳ID，外鍵（FK）連結到 Restaurants 表（前台 + 後台）。此欄位指定該管理者所屬的餐廳。
	/// </summary>
	public int? RestaurantId { get; set; }

	/// <summary>
	/// 登入電子郵件（前台 + 後台）。此欄位存儲餐廳管理者的電子郵件地址，用於登入和通信。
	/// </summary>
	[Display(Name = "電子郵件")]
	public string? Email { get; set; }

	/// <summary>
	/// 密碼的哈希值（前台 + 後台）。此欄位存儲餐廳管理者密碼的加密哈希值，確保密碼的安全性。
	/// </summary>
	[Display(Name = "密碼")]
	public string? PasswordHash { get; set; }

	/// <summary>
	/// 手機號碼（前台 + 後台）。此欄位存儲餐廳管理者的手機號碼，用於緊急聯絡和通知。
	/// </summary>
	[Display(Name = "手機號碼")]
	public string? MobileNumber { get; set; }

	/// <summary>
	/// 管理人員的職位（前台 + 後台）。此欄位描述餐廳管理者的職位，如店長、經理等，用於角色識別和權限管理。
	/// </summary>
	[Display(Name = "職位")]
	public string? ManagerPosition { get; set; }

	/// <summary>
	/// 登入及顯示名稱（前台 + 後台）。此欄位用於餐廳管理者的登入名稱及在系統中的顯示名稱，方便識別和管理。
	/// </summary>
	[Display(Name = "帳號名稱")]
	public string? ManagerId { get; set; }

	/// <summary>
	/// 帳號建立時間（前台 + 後台）。此欄位記錄餐廳管理者帳號的建立時間，用於審計和管理。
	/// </summary>
	public DateTime? CreatedAt { get; set; }

	/// <summary>
	/// 帳號更新時間（前台 + 後台）。此欄位記錄餐廳管理者帳號的最後更新時間，用於追蹤帳號變更歷史。
	/// </summary>
	public DateTime? UpdatedAt { get; set; }

	public virtual ICollection<PasswordResetRequest> PasswordResetRequests { get; set; } = new List<PasswordResetRequest>();

	public virtual Restaurant? Restaurant { get; set; }
}
