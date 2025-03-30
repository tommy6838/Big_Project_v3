using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Big_Project_v3.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRatingToDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    RestaurantID = table.Column<int>(type: "int", nullable: false, comment: "餐廳的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個餐廳，並在資料表之間建立關聯。")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "餐廳名稱（前台 + 後台）。此欄位存儲餐廳的名稱，用於顯示和管理。"),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "餐廳地址（前台 + 後台）。此欄位存儲餐廳的實體地址，用於定位和導航。"),
                    ContactPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "聯絡電話（前台 + 後台）。此欄位存儲餐廳的聯絡電話號碼，用於客戶諮詢和緊急聯絡。"),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "餐廳簡介（前台 + 後台）。此欄位存儲餐廳的詳細介紹，包括特色、歷史等資訊，供客戶參考。"),
                    HasParking = table.Column<bool>(type: "bit", nullable: true, comment: "是否有停車場（1：有，0：無）（前台 + 後台）。此欄位指示餐廳是否提供停車設施，供客戶參考。"),
                    IsReservationOpen = table.Column<bool>(type: "bit", nullable: false, comment: "是否開放預訂（1：開放，0：關閉）（前台 + 後台）。此欄位指示餐廳是否接受預訂，供客戶決定是否進行預約。"),
                    AverageRating = table.Column<double>(type: "float", nullable: true, comment: "餐廳的平均評分（前台 + 後台）。此欄位存儲餐廳的平均客戶評分，供客戶參考和比較。"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, comment: "餐廳資料建立時間（前台 + 後台）。此欄位記錄餐廳資料的建立時間，用於審計和管理。"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, comment: "餐廳資料更新時間（前台 + 後台）。此欄位記錄餐廳資料的最後更新時間，用於追蹤資料變更歷史。"),
                    BusinessHoursStart = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true, comment: "營業開始時間"),
                    BusinessHoursEnd = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true, comment: "營業結束時間"),
                    LastCheckInTime = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true, comment: "最後收客時間（可為空）"),
                    GoogleMapAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PriceRange = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Restaura__87454CB5BA430447", x => x.RestaurantID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false, comment: "使用者的唯一識別碼，主鍵（PK），自動遞增")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "用於登入的帳號名稱"),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true, comment: "密碼的哈希值"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "使用者姓名"),
                    ContactPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "聯絡電話"),
                    ContactEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "聯絡電子郵件"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, comment: "帳號建立時間"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, comment: "帳號更新時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__1788CCAC6D3159D6", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    AnnouncementID = table.Column<int>(type: "int", nullable: false, comment: "公告的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個公告紀錄，並在資料表之間建立關聯。")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantID = table.Column<int>(type: "int", nullable: true, comment: "餐廳ID，外鍵（FK）連結到 Restaurants 表（前台 + 後台）。此欄位指定該公告所屬的餐廳。"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "公告標題（前台 + 後台）。此欄位存儲公告的標題，用於前台顯示和後台管理。"),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "公告內容（前台 + 後台）。此欄位存儲公告的詳細內容，供前台用戶閱讀和後台管理。"),
                    AnnouncementDate = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, comment: "公告日期（前台 + 後台）。此欄位記錄公告的發布日期，用於排序和過濾。"),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true, comment: "公告開始日期（前台 + 後台）。此欄位指定公告的開始日期，決定公告何時開始顯示。"),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true, comment: "公告結束日期（前台 + 後台）。此欄位指定公告的結束日期，決定公告何時停止顯示。"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, comment: "公告建立時間（前台 + 後台）。此欄位記錄公告紀錄的建立時間，用於審計和管理。"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, comment: "公告更新時間（前台 + 後台）。此欄位記錄公告紀錄的最後更新時間，用於追蹤資料變更歷史。")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Announce__9DE44554BAAC7C0D", x => x.AnnouncementID);
                    table.ForeignKey(
                        name: "FK_Announcements_Restaurants",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantID");
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    PhotoID = table.Column<int>(type: "int", nullable: false, comment: "相片的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個相片紀錄，並在資料表之間建立關聯。")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantID = table.Column<int>(type: "int", nullable: true, comment: "餐廳ID，外鍵（FK）連結到 Restaurants 表（前台 + 後台）。此欄位指定該相片所屬的餐廳。"),
                    PhotoURL = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "相片的 URL 或存儲路徑（前台 + 後台）。此欄位存儲相片的網路地址或在伺服器上的存儲路徑，用於前台顯示和管理。"),
                    PhotoType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "相片類型（例如：餐廳環境、菜單）（前台 + 後台）。此欄位描述相片的類型，方便分類和管理。"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "相片描述（前台 + 後台）。此欄位提供對相片的詳細描述，幫助前台用戶理解相片內容。"),
                    UploadedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, comment: "相片上傳時間（前台 + 後台）。此欄位記錄相片的上傳時間，用於管理和排序相片。"),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Photos__21B7B582A613A938", x => x.PhotoID);
                    table.ForeignKey(
                        name: "FK_Photos_Restaurants",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantID");
                });

            migrationBuilder.CreateTable(
                name: "ReservationControlSettings",
                columns: table => new
                {
                    SettingID = table.Column<int>(type: "int", nullable: false, comment: "設定的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個訂位控管設定，並在資料表之間建立關聯。")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantID = table.Column<int>(type: "int", nullable: true, comment: "餐廳ID，外鍵（FK）連結到 Restaurants 表（後台）。此欄位指定訂位控管設定所屬的餐廳。"),
                    AdvanceBookingDays = table.Column<int>(type: "int", nullable: true, comment: "開放預訂的天數（後台）。此欄位指定餐廳允許提前預訂的天數範圍，用於限制預訂的時間跨度。"),
                    CloseBookingBeforeHours = table.Column<int>(type: "int", nullable: true, comment: "關閉預訂的提前小時數（後台）。此欄位指定餐廳在預訂前需提前關閉訂位的時間，用於確保餐廳有足夠的準備時間。"),
                    AvailableTimeSlots = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "可預訂的時段區間（JSON 格式）（後台）。此欄位存儲餐廳可接受預訂的具體時段，以 JSON 格式表示，方便靈活管理。"),
                    MinPeoplePerReservation = table.Column<int>(type: "int", nullable: true, comment: "每次訂位最少人數（後台）。此欄位指定每次訂位所需的最少人數，用於控制訂位的規模。"),
                    MaxPeoplePerReservation = table.Column<int>(type: "int", nullable: true, comment: "每次訂位最多人數（後台）。此欄位指定每次訂位所允許的最大人數，用於控制訂位的規模。"),
                    MaxReservationsPerAccount = table.Column<int>(type: "int", nullable: true, comment: "每個帳號限訂位次數（後台）。此欄位指定每個使用者帳號可進行的訂位次數上限，用於防止濫用預訂系統。"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, comment: "設定建立時間。此欄位記錄訂位控管設定紀錄的建立時間，用於審計和管理。"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, comment: "設定更新時間。此欄位記錄訂位控管設定紀錄的最後更新時間，用於追蹤資料變更歷史。")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reservat__54372AFD70E74078", x => x.SettingID);
                    table.ForeignKey(
                        name: "FK_ReservationControlSettings_Restaurants",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantID");
                });

            migrationBuilder.CreateTable(
                name: "RestaurantAvailability",
                columns: table => new
                {
                    AvailabilityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: true),
                    TimeSlot = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true),
                    MaxCapacity = table.Column<int>(type: "int", nullable: true),
                    AvailableSeats = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, comment: "資料更新時間（前台 + 後台）。此欄位記錄可用時段紀錄的最後更新時間，用於追蹤資料變更歷史。")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Restaura__DA397991CBB3FACB", x => x.AvailabilityID);
                    table.ForeignKey(
                        name: "FK_RestaurantAvailability_Restaurants",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantID");
                });

            migrationBuilder.CreateTable(
                name: "RestaurantUsers",
                columns: table => new
                {
                    RestaurantUserID = table.Column<int>(type: "int", nullable: false, comment: "餐廳管理者的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個餐廳管理者，並在資料表之間建立關聯。")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantID = table.Column<int>(type: "int", nullable: true, comment: "餐廳ID，外鍵（FK）連結到 Restaurants 表（前台 + 後台）。此欄位指定該管理者所屬的餐廳。"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "登入電子郵件（前台 + 後台）。此欄位存儲餐廳管理者的電子郵件地址，用於登入和通信。"),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true, comment: "密碼的哈希值（前台 + 後台）。此欄位存儲餐廳管理者密碼的加密哈希值，確保密碼的安全性。"),
                    MobileNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "手機號碼（前台 + 後台）。此欄位存儲餐廳管理者的手機號碼，用於緊急聯絡和通知。"),
                    ManagerPosition = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "管理人員的職位（前台 + 後台）。此欄位描述餐廳管理者的職位，如店長、經理等，用於角色識別和權限管理。"),
                    ManagerID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "登入及顯示名稱（前台 + 後台）。此欄位用於餐廳管理者的登入名稱及在系統中的顯示名稱，方便識別和管理。"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, comment: "帳號建立時間（前台 + 後台）。此欄位記錄餐廳管理者帳號的建立時間，用於審計和管理。"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, comment: "帳號更新時間（前台 + 後台）。此欄位記錄餐廳管理者帳號的最後更新時間，用於追蹤帳號變更歷史。")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Restaura__380CADFAF95FB251", x => x.RestaurantUserID);
                    table.ForeignKey(
                        name: "FK_RestaurantUsers_Restaurants",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantID");
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    FavoriteID = table.Column<int>(type: "int", nullable: false, comment: "收藏的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個收藏紀錄，並在資料表之間建立關聯。")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true, comment: "使用者ID，外鍵（FK）連結到 Users 表（前台 + 後台）。此欄位指定收藏紀錄所屬的使用者。"),
                    RestaurantID = table.Column<int>(type: "int", nullable: true, comment: "餐廳ID，外鍵（FK）連結到 Restaurants 表（前台 + 後台）。此欄位指定收藏紀錄所屬的餐廳。"),
                    AddedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, comment: "收藏加入時間（前台 + 後台）。此欄位記錄收藏紀錄的加入時間，用於排序和管理。")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Favorite__CE74FAF5C3B335F2", x => x.FavoriteID);
                    table.ForeignKey(
                        name: "FK_Favorites_Restaurants",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantID");
                    table.ForeignKey(
                        name: "FK_Favorites_Users",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ReservationID = table.Column<int>(type: "int", nullable: false, comment: "訂位的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個訂位紀錄，並在資料表之間建立關聯。")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantID = table.Column<int>(type: "int", nullable: true, comment: "餐廳ID，外鍵（FK）連結到 Restaurants 表（前台 + 後台）。此欄位指定訂位所屬的餐廳。"),
                    UserID = table.Column<int>(type: "int", nullable: true, comment: "使用者ID，外鍵（FK）連結到 Users 表（前台 + 後台）。此欄位指定訂位的使用者。"),
                    ReservationDate = table.Column<DateOnly>(type: "date", nullable: true, comment: "訂位日期（前台 + 後台）。此欄位指定訂位的日期，用於安排和管理。"),
                    ReservationTime = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true, comment: "訂位時間（前台 + 後台）。此欄位指定訂位的具體時間，用於安排和管理。"),
                    NumAdults = table.Column<int>(type: "int", nullable: true, comment: "大人人數（前台 + 後台）。此欄位指定訂位時預定的大人人數，用於餐廳準備座位和資源。"),
                    NumChildren = table.Column<int>(type: "int", nullable: true, comment: "小孩人數（前台 + 後台）。此欄位指定訂位時預定的小孩人數，用於餐廳準備座位和資源。"),
                    BookerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "訂位人姓名（前台 + 後台）。此欄位存儲發起訂位的使用者姓名，用於確認和聯絡。"),
                    BookerPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "訂位人電話（前台 + 後台）。此欄位存儲發起訂位的使用者電話號碼，用於確認和聯絡。"),
                    BookerEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "訂位人電子郵件（前台 + 後台）。此欄位存儲發起訂位的使用者電子郵件地址，用於確認和聯絡。"),
                    SpecialRequests = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "特殊要求備註（前台 + 後台）。此欄位存儲使用者對訂位的特殊要求或備註，如過敏資訊、座位偏好等。"),
                    ReservationStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "訂位狀態（例如：已確認、已取消）（前台 + 後台）。此欄位指示訂位的當前狀態，方便管理和追蹤。"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, comment: "訂位建立時間（前台 + 後台）。此欄位記錄訂位紀錄的建立時間，用於審計和管理。"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, comment: "訂位更新時間（前台 + 後台）。此欄位記錄訂位紀錄的最後更新時間，用於追蹤資料變更歷史。")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reservat__B7EE5F04056C88C4", x => x.ReservationID);
                    table.ForeignKey(
                        name: "FK_Reservations_Restaurants",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantID");
                    table.ForeignKey(
                        name: "FK_Reservations_Users",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewID = table.Column<int>(type: "int", nullable: false, comment: "評論的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個評論紀錄，並在資料表之間建立關聯。")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true, comment: "使用者ID，外鍵（FK）連結到 Users 表（前台 + 後台）。此欄位指定發表評論的使用者。"),
                    RestaurantID = table.Column<int>(type: "int", nullable: true, comment: "餐廳ID，外鍵（FK）連結到 Restaurants 表（前台 + 後台）。此欄位指定評論所屬的餐廳。"),
                    Rating = table.Column<double>(type: "float", nullable: true, comment: "評分（1-5 星）（前台 + 後台）。此欄位存儲使用者對餐廳的評分，用於計算平均評分和顯示給其他用戶。"),
                    ReviewText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "評論內容（前台 + 後台）。此欄位存儲使用者對餐廳的詳細評論，用於提供其他用戶參考。"),
                    ReviewDate = table.Column<DateOnly>(type: "date", nullable: true, comment: "評論日期（前台 + 後台）。此欄位記錄評論的發表日期，用於排序和顯示。"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, comment: "評論建立時間（前台 + 後台）。此欄位記錄評論紀錄的建立時間，用於審計和管理。"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, comment: "評論更新時間（前台 + 後台）。此欄位記錄評論紀錄的最後更新時間，用於追蹤資料變更歷史。")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reviews__74BC79AE826B3A68", x => x.ReviewID);
                    table.ForeignKey(
                        name: "FK_Reviews_Restaurants",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantID");
                    table.ForeignKey(
                        name: "FK_Reviews_Users",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetRequests",
                columns: table => new
                {
                    RequestID = table.Column<int>(type: "int", nullable: false, comment: "請求的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個密碼重設請求，並在資料表之間建立關聯。")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true, comment: "使用者ID，外鍵（FK）連結到 Users 表（前台 + 後台）。此欄位指定發起密碼重設請求的使用者。"),
                    RestaurantUserID = table.Column<int>(type: "int", nullable: true, comment: "餐廳管理者ID，外鍵（FK）連結到 RestaurantUsers 表（前台 + 後台）。此欄位指定發起密碼重設請求的餐廳管理者。"),
                    Token = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "密碼重設的唯一令牌（前台 + 後台）。此欄位存儲用於驗證和完成密碼重設的唯一標識符。"),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, comment: "令牌的過期時間（前台 + 後台）。此欄位指定密碼重設令牌的有效期限，超過此時間令牌將失效。"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, comment: "請求建立時間（前台 + 後台）。此欄位記錄密碼重設請求的建立時間，用於審計和管理。")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Password__33A8519A20229840", x => x.RequestID);
                    table.ForeignKey(
                        name: "FK_PasswordResetRequests_RestaurantUsers",
                        column: x => x.RestaurantUserID,
                        principalTable: "RestaurantUsers",
                        principalColumn: "RestaurantUserID");
                    table.ForeignKey(
                        name: "FK_PasswordResetRequests_Users",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_RestaurantID",
                table: "Announcements",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_RestaurantID",
                table: "Favorites",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserID",
                table: "Favorites",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetRequests_RestaurantUserID",
                table: "PasswordResetRequests",
                column: "RestaurantUserID");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetRequests_UserID",
                table: "PasswordResetRequests",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_RestaurantID",
                table: "Photos",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationControlSettings_RestaurantID",
                table: "ReservationControlSettings",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_RestaurantID",
                table: "Reservations",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserID",
                table: "Reservations",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantAvailability_RestaurantID",
                table: "RestaurantAvailability",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantUsers_RestaurantID",
                table: "RestaurantUsers",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RestaurantID",
                table: "Reviews",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserID",
                table: "Reviews",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "PasswordResetRequests");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "ReservationControlSettings");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "RestaurantAvailability");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "RestaurantUsers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Restaurants");
        }
    }
}
