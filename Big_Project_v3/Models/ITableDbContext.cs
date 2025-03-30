using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Big_Project_v3.Models;

public partial class ITableDbContext : DbContext
{
    public ITableDbContext(DbContextOptions<ITableDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Announcement> Announcements { get; set; }

    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<PasswordResetRequest> PasswordResetRequests { get; set; }

    public virtual DbSet<Photo> Photos { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<ReservationControlSetting> ReservationControlSettings { get; set; }

    public virtual DbSet<Restaurant> Restaurants { get; set; }

    public virtual DbSet<RestaurantAvailability> RestaurantAvailabilities { get; set; }

    public virtual DbSet<RestaurantUser> RestaurantUsers { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.HasKey(e => e.AnnouncementId).HasName("PK__Announce__9DE44554BAAC7C0D");

            entity.Property(e => e.AnnouncementId)
                .HasComment("公告的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個公告紀錄，並在資料表之間建立關聯。")
                .HasColumnName("AnnouncementID");
            entity.Property(e => e.AnnouncementDate)
                .HasPrecision(0)
                .HasComment("公告日期（前台 + 後台）。此欄位記錄公告的發布日期，用於排序和過濾。");
            entity.Property(e => e.Content)
                .HasMaxLength(1000)
                .HasComment("公告內容（前台 + 後台）。此欄位存儲公告的詳細內容，供前台用戶閱讀和後台管理。");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasComment("公告建立時間（前台 + 後台）。此欄位記錄公告紀錄的建立時間，用於審計和管理。");
            entity.Property(e => e.EndDate).HasComment("公告結束日期（前台 + 後台）。此欄位指定公告的結束日期，決定公告何時停止顯示。");
            entity.Property(e => e.RestaurantId)
                .HasComment("餐廳ID，外鍵（FK）連結到 Restaurants 表（前台 + 後台）。此欄位指定該公告所屬的餐廳。")
                .HasColumnName("RestaurantID");
            entity.Property(e => e.StartDate).HasComment("公告開始日期（前台 + 後台）。此欄位指定公告的開始日期，決定公告何時開始顯示。");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasComment("公告標題（前台 + 後台）。此欄位存儲公告的標題，用於前台顯示和後台管理。");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasComment("公告更新時間（前台 + 後台）。此欄位記錄公告紀錄的最後更新時間，用於追蹤資料變更歷史。");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Announcements)
                .HasForeignKey(d => d.RestaurantId)
                .HasConstraintName("FK_Announcements_Restaurants");
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => e.FavoriteId).HasName("PK__Favorite__CE74FAF5C3B335F2");

            entity.Property(e => e.FavoriteId)
                .HasComment("收藏的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個收藏紀錄，並在資料表之間建立關聯。")
                .HasColumnName("FavoriteID");
            entity.Property(e => e.AddedAt)
                .HasPrecision(0)
                .HasComment("收藏加入時間（前台 + 後台）。此欄位記錄收藏紀錄的加入時間，用於排序和管理。");
            entity.Property(e => e.RestaurantId)
                .HasComment("餐廳ID，外鍵（FK）連結到 Restaurants 表（前台 + 後台）。此欄位指定收藏紀錄所屬的餐廳。")
                .HasColumnName("RestaurantID");
            entity.Property(e => e.UserId)
                .HasComment("使用者ID，外鍵（FK）連結到 Users 表（前台 + 後台）。此欄位指定收藏紀錄所屬的使用者。")
                .HasColumnName("UserID");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.RestaurantId)
                .HasConstraintName("FK_Favorites_Restaurants");

            entity.HasOne(d => d.User).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Favorites_Users");
        });

        modelBuilder.Entity<PasswordResetRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Password__33A8519A20229840");

            entity.Property(e => e.RequestId)
                .HasComment("請求的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個密碼重設請求，並在資料表之間建立關聯。")
                .HasColumnName("RequestID");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasComment("請求建立時間（前台 + 後台）。此欄位記錄密碼重設請求的建立時間，用於審計和管理。");
            entity.Property(e => e.ExpiresAt)
                .HasPrecision(0)
                .HasComment("令牌的過期時間（前台 + 後台）。此欄位指定密碼重設令牌的有效期限，超過此時間令牌將失效。");
            entity.Property(e => e.RestaurantUserId)
                .HasComment("餐廳管理者ID，外鍵（FK）連結到 RestaurantUsers 表（前台 + 後台）。此欄位指定發起密碼重設請求的餐廳管理者。")
                .HasColumnName("RestaurantUserID");
            entity.Property(e => e.Token)
                .HasMaxLength(100)
                .HasComment("密碼重設的唯一令牌（前台 + 後台）。此欄位存儲用於驗證和完成密碼重設的唯一標識符。");
            entity.Property(e => e.UserId)
                .HasComment("使用者ID，外鍵（FK）連結到 Users 表（前台 + 後台）。此欄位指定發起密碼重設請求的使用者。")
                .HasColumnName("UserID");

            entity.HasOne(d => d.RestaurantUser).WithMany(p => p.PasswordResetRequests)
                .HasForeignKey(d => d.RestaurantUserId)
                .HasConstraintName("FK_PasswordResetRequests_RestaurantUsers");

            entity.HasOne(d => d.User).WithMany(p => p.PasswordResetRequests)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_PasswordResetRequests_Users");
        });

        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(e => e.PhotoId).HasName("PK__Photos__21B7B582A613A938");

            entity.Property(e => e.PhotoId)
                .HasComment("相片的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個相片紀錄，並在資料表之間建立關聯。")
                .HasColumnName("PhotoID");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasComment("相片描述（前台 + 後台）。此欄位提供對相片的詳細描述，幫助前台用戶理解相片內容。");
            entity.Property(e => e.ImagePath).HasMaxLength(500);
            entity.Property(e => e.PhotoType)
                .HasMaxLength(50)
                .HasComment("相片類型（例如：餐廳環境、菜單）（前台 + 後台）。此欄位描述相片的類型，方便分類和管理。");
            entity.Property(e => e.PhotoUrl)
                .HasMaxLength(500)
                .HasComment("相片的 URL 或存儲路徑（前台 + 後台）。此欄位存儲相片的網路地址或在伺服器上的存儲路徑，用於前台顯示和管理。")
                .HasColumnName("PhotoURL");
            entity.Property(e => e.RestaurantId)
                .HasComment("餐廳ID，外鍵（FK）連結到 Restaurants 表（前台 + 後台）。此欄位指定該相片所屬的餐廳。")
                .HasColumnName("RestaurantID");
            entity.Property(e => e.UploadedAt)
                .HasPrecision(0)
                .HasComment("相片上傳時間（前台 + 後台）。此欄位記錄相片的上傳時間，用於管理和排序相片。");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Photos)
                .HasForeignKey(d => d.RestaurantId)
                .HasConstraintName("FK_Photos_Restaurants");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PK__Reservat__B7EE5F04056C88C4");

            entity.Property(e => e.ReservationId)
                .HasComment("訂位的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個訂位紀錄，並在資料表之間建立關聯。")
                .HasColumnName("ReservationID");
            entity.Property(e => e.BookerEmail)
                .HasMaxLength(100)
                .HasComment("訂位人電子郵件（前台 + 後台）。此欄位存儲發起訂位的使用者電子郵件地址，用於確認和聯絡。");
            entity.Property(e => e.BookerName)
                .HasMaxLength(100)
                .HasComment("訂位人姓名（前台 + 後台）。此欄位存儲發起訂位的使用者姓名，用於確認和聯絡。");
            entity.Property(e => e.BookerPhone)
                .HasMaxLength(20)
                .HasComment("訂位人電話（前台 + 後台）。此欄位存儲發起訂位的使用者電話號碼，用於確認和聯絡。");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasComment("訂位建立時間（前台 + 後台）。此欄位記錄訂位紀錄的建立時間，用於審計和管理。");
            entity.Property(e => e.NumAdults).HasComment("大人人數（前台 + 後台）。此欄位指定訂位時預定的大人人數，用於餐廳準備座位和資源。");
            entity.Property(e => e.NumChildren).HasComment("小孩人數（前台 + 後台）。此欄位指定訂位時預定的小孩人數，用於餐廳準備座位和資源。");
            entity.Property(e => e.ReservationDate).HasComment("訂位日期（前台 + 後台）。此欄位指定訂位的日期，用於安排和管理。");
            entity.Property(e => e.ReservationStatus)
                .HasMaxLength(50)
                .HasComment("訂位狀態（例如：已確認、已取消）（前台 + 後台）。此欄位指示訂位的當前狀態，方便管理和追蹤。");
            entity.Property(e => e.ReservationTime)
                .HasPrecision(0)
                .HasComment("訂位時間（前台 + 後台）。此欄位指定訂位的具體時間，用於安排和管理。");
            entity.Property(e => e.RestaurantId)
                .HasComment("餐廳ID，外鍵（FK）連結到 Restaurants 表（前台 + 後台）。此欄位指定訂位所屬的餐廳。")
                .HasColumnName("RestaurantID");
            entity.Property(e => e.SpecialRequests)
                .HasMaxLength(1000)
                .HasComment("特殊要求備註（前台 + 後台）。此欄位存儲使用者對訂位的特殊要求或備註，如過敏資訊、座位偏好等。");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasComment("訂位更新時間（前台 + 後台）。此欄位記錄訂位紀錄的最後更新時間，用於追蹤資料變更歷史。");
            entity.Property(e => e.UserId)
                .HasComment("使用者ID，外鍵（FK）連結到 Users 表（前台 + 後台）。此欄位指定訂位的使用者。")
                .HasColumnName("UserID");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.RestaurantId)
                .HasConstraintName("FK_Reservations_Restaurants");

            entity.HasOne(d => d.User).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Reservations_Users");
        });

        modelBuilder.Entity<ReservationControlSetting>(entity =>
        {
            entity.HasKey(e => e.SettingId).HasName("PK__Reservat__54372AFD70E74078");

            entity.Property(e => e.SettingId)
                .HasComment("設定的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個訂位控管設定，並在資料表之間建立關聯。")
                .HasColumnName("SettingID");
            entity.Property(e => e.AdvanceBookingDays).HasComment("開放預訂的天數（後台）。此欄位指定餐廳允許提前預訂的天數範圍，用於限制預訂的時間跨度。");
            entity.Property(e => e.AvailableTimeSlots).HasComment("可預訂的時段區間（JSON 格式）（後台）。此欄位存儲餐廳可接受預訂的具體時段，以 JSON 格式表示，方便靈活管理。");
            entity.Property(e => e.CloseBookingBeforeHours).HasComment("關閉預訂的提前小時數（後台）。此欄位指定餐廳在預訂前需提前關閉訂位的時間，用於確保餐廳有足夠的準備時間。");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasComment("設定建立時間。此欄位記錄訂位控管設定紀錄的建立時間，用於審計和管理。");
            entity.Property(e => e.MaxPeoplePerReservation).HasComment("每次訂位最多人數（後台）。此欄位指定每次訂位所允許的最大人數，用於控制訂位的規模。");
            entity.Property(e => e.MaxReservationsPerAccount).HasComment("每個帳號限訂位次數（後台）。此欄位指定每個使用者帳號可進行的訂位次數上限，用於防止濫用預訂系統。");
            entity.Property(e => e.MinPeoplePerReservation).HasComment("每次訂位最少人數（後台）。此欄位指定每次訂位所需的最少人數，用於控制訂位的規模。");
            entity.Property(e => e.RestaurantId)
                .HasComment("餐廳ID，外鍵（FK）連結到 Restaurants 表（後台）。此欄位指定訂位控管設定所屬的餐廳。")
                .HasColumnName("RestaurantID");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasComment("設定更新時間。此欄位記錄訂位控管設定紀錄的最後更新時間，用於追蹤資料變更歷史。");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.ReservationControlSettings)
                .HasForeignKey(d => d.RestaurantId)
                .HasConstraintName("FK_ReservationControlSettings_Restaurants");
        });

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(e => e.RestaurantId).HasName("PK__Restaura__87454CB5BA430447");

            entity.Property(e => e.RestaurantId)
                .HasComment("餐廳的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個餐廳，並在資料表之間建立關聯。")
                .HasColumnName("RestaurantID");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasComment("餐廳地址（前台 + 後台）。此欄位存儲餐廳的實體地址，用於定位和導航。");
            entity.Property(e => e.AverageRating).HasComment("餐廳的平均評分（前台 + 後台）。此欄位存儲餐廳的平均客戶評分，供客戶參考和比較。");
            entity.Property(e => e.BusinessHoursEnd)
                .HasPrecision(0)
                .HasComment("營業結束時間");
            entity.Property(e => e.BusinessHoursStart)
                .HasPrecision(0)
                .HasComment("營業開始時間");
            entity.Property(e => e.ContactPhone)
                .HasMaxLength(20)
                .HasComment("聯絡電話（前台 + 後台）。此欄位存儲餐廳的聯絡電話號碼，用於客戶諮詢和緊急聯絡。");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasComment("餐廳資料建立時間（前台 + 後台）。此欄位記錄餐廳資料的建立時間，用於審計和管理。");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasComment("餐廳簡介（前台 + 後台）。此欄位存儲餐廳的詳細介紹，包括特色、歷史等資訊，供客戶參考。");
            entity.Property(e => e.GoogleMapAddress).HasMaxLength(500);
            entity.Property(e => e.HasParking).HasComment("是否有停車場（1：有，0：無）（前台 + 後台）。此欄位指示餐廳是否提供停車設施，供客戶參考。");
            entity.Property(e => e.IsReservationOpen).HasComment("是否開放預訂（1：開放，0：關閉）（前台 + 後台）。此欄位指示餐廳是否接受預訂，供客戶決定是否進行預約。");
            entity.Property(e => e.LastCheckInTime)
                .HasPrecision(0)
                .HasComment("最後收客時間（可為空）");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasComment("餐廳名稱（前台 + 後台）。此欄位存儲餐廳的名稱，用於顯示和管理。");
            entity.Property(e => e.PriceRange).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasComment("餐廳資料更新時間（前台 + 後台）。此欄位記錄餐廳資料的最後更新時間，用於追蹤資料變更歷史。");
        });

        modelBuilder.Entity<RestaurantAvailability>(entity =>
        {
            entity.HasKey(e => e.AvailabilityId).HasName("PK__Restaura__DA397991CBB3FACB");

            entity.ToTable("RestaurantAvailability");

            entity.Property(e => e.AvailabilityId).HasColumnName("AvailabilityID");
            entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");
            entity.Property(e => e.TimeSlot).HasPrecision(0);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasComment("資料更新時間（前台 + 後台）。此欄位記錄可用時段紀錄的最後更新時間，用於追蹤資料變更歷史。");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.RestaurantAvailabilities)
                .HasForeignKey(d => d.RestaurantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RestaurantAvailability_Restaurants");
        });

        modelBuilder.Entity<RestaurantUser>(entity =>
        {
            entity.HasKey(e => e.RestaurantUserId).HasName("PK__Restaura__380CADFAF95FB251");

            entity.Property(e => e.RestaurantUserId)
                .HasComment("餐廳管理者的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個餐廳管理者，並在資料表之間建立關聯。")
                .HasColumnName("RestaurantUserID");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasComment("帳號建立時間（前台 + 後台）。此欄位記錄餐廳管理者帳號的建立時間，用於審計和管理。");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasComment("登入電子郵件（前台 + 後台）。此欄位存儲餐廳管理者的電子郵件地址，用於登入和通信。");
            entity.Property(e => e.ManagerId)
                .HasMaxLength(50)
                .HasComment("登入及顯示名稱（前台 + 後台）。此欄位用於餐廳管理者的登入名稱及在系統中的顯示名稱，方便識別和管理。")
                .HasColumnName("ManagerID");
            entity.Property(e => e.ManagerPosition)
                .HasMaxLength(50)
                .HasComment("管理人員的職位（前台 + 後台）。此欄位描述餐廳管理者的職位，如店長、經理等，用於角色識別和權限管理。");
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(20)
                .HasComment("手機號碼（前台 + 後台）。此欄位存儲餐廳管理者的手機號碼，用於緊急聯絡和通知。");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(256)
                .HasComment("密碼的哈希值（前台 + 後台）。此欄位存儲餐廳管理者密碼的加密哈希值，確保密碼的安全性。");
            entity.Property(e => e.RestaurantId)
                .HasComment("餐廳ID，外鍵（FK）連結到 Restaurants 表（前台 + 後台）。此欄位指定該管理者所屬的餐廳。")
                .HasColumnName("RestaurantID");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasComment("帳號更新時間（前台 + 後台）。此欄位記錄餐廳管理者帳號的最後更新時間，用於追蹤帳號變更歷史。");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.RestaurantUsers)
                .HasForeignKey(d => d.RestaurantId)
                .HasConstraintName("FK_RestaurantUsers_Restaurants");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Reviews__74BC79AE826B3A68");

            entity.Property(e => e.ReviewId)
                .HasComment("評論的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個評論紀錄，並在資料表之間建立關聯。")
                .HasColumnName("ReviewID");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasComment("評論建立時間（前台 + 後台）。此欄位記錄評論紀錄的建立時間，用於審計和管理。");
            entity.Property(e => e.Rating).HasComment("評分（1-5 星）（前台 + 後台）。此欄位存儲使用者對餐廳的評分，用於計算平均評分和顯示給其他用戶。");
            entity.Property(e => e.RestaurantId)
                .HasComment("餐廳ID，外鍵（FK）連結到 Restaurants 表（前台 + 後台）。此欄位指定評論所屬的餐廳。")
                .HasColumnName("RestaurantID");
            entity.Property(e => e.ReviewDate).HasComment("評論日期（前台 + 後台）。此欄位記錄評論的發表日期，用於排序和顯示。");
            entity.Property(e => e.ReviewText)
                .HasMaxLength(1000)
                .HasComment("評論內容（前台 + 後台）。此欄位存儲使用者對餐廳的詳細評論，用於提供其他用戶參考。");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasComment("評論更新時間（前台 + 後台）。此欄位記錄評論紀錄的最後更新時間，用於追蹤資料變更歷史。");
            entity.Property(e => e.UserId)
                .HasComment("使用者ID，外鍵（FK）連結到 Users 表（前台 + 後台）。此欄位指定發表評論的使用者。")
                .HasColumnName("UserID");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.RestaurantId)
                .HasConstraintName("FK_Reviews_Restaurants");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Reviews_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC6D3159D6");

            entity.Property(e => e.UserId)
                .HasComment("使用者的唯一識別碼，主鍵（PK），自動遞增")
                .HasColumnName("UserID");
            entity.Property(e => e.ContactEmail)
                .HasMaxLength(100)
                .HasComment("聯絡電子郵件");
            entity.Property(e => e.ContactPhone)
                .HasMaxLength(20)
                .HasComment("聯絡電話");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasComment("帳號建立時間");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasComment("使用者姓名");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(256)
                .HasComment("密碼的哈希值");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasComment("帳號更新時間");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasComment("用於登入的帳號名稱");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
