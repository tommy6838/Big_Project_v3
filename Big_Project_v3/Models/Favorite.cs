using System;
using System.Collections.Generic;

namespace Big_Project_v3.Models;

public partial class Favorite
{
    /// <summary>
    /// 收藏的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個收藏紀錄，並在資料表之間建立關聯。
    /// </summary>
    public int FavoriteId { get; set; }

    /// <summary>
    /// 使用者ID，外鍵（FK）連結到 Users 表（前台 + 後台）。此欄位指定收藏紀錄所屬的使用者。
    /// </summary>
    public int? UserId { get; set; }

    /// <summary>
    /// 餐廳ID，外鍵（FK）連結到 Restaurants 表（前台 + 後台）。此欄位指定收藏紀錄所屬的餐廳。
    /// </summary>
    public int? RestaurantId { get; set; }

    /// <summary>
    /// 收藏加入時間（前台 + 後台）。此欄位記錄收藏紀錄的加入時間，用於排序和管理。
    /// </summary>
    public DateTime? AddedAt { get; set; }

    public virtual Restaurant? Restaurant { get; set; }

    public virtual User? User { get; set; }
}
