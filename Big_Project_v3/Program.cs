using Big_Project_v3.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 添加服務到 DI 容器
builder.Services.AddDbContext<ITableDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("iTableDBConnection"));
});
// 設定使用 SQL Server 的 iTableDbContext，連接字串從 appsettings.json 中的 iTableDBConnection 讀取

builder.Services.AddControllersWithViews(); // 註冊 MVC 服務

// 配置 Session 支援
builder.Services.AddDistributedMemoryCache(); // 添加內存緩存（Session 的基礎存儲）
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // 設置 Session 的過期時間
    options.Cookie.HttpOnly = true; // 只能通過 HTTP 訪問 Session
    options.Cookie.IsEssential = true; // 標記為必需的 Cookie
});

builder.Services.AddHttpContextAccessor(); // 添加 HttpContextAccessor 支援，方便在控制器中訪問 HttpContext

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.Use(async (context, next) =>
{
    // 強制設置 Content-Type 為 UTF-8，避免亂碼
    context.Response.Headers.Append("Content-Type", "text/html; charset=UTF-8");
    await next.Invoke();
});

app.UseRouting();

// 啟用 Session 中介軟體
app.UseSession(); // 必須在 app.UseAuthorization() 之前

app.UseAuthorization();

//app.MapControllerRoute(
//    name: "home",
//    pattern: "{controller=HomePage}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "booking",
//    pattern: "{controller=Booking}/{action=BookingPage}/{restaurantID?}");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller}/{action}/{id?}");

//app.MapControllerRoute(
//    name: "booking",
//    pattern: "Booking/{action=BookingPage}/{RestaurantId?}",
//    defaults: new { controller = "Booking" });

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=HomePage}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areaRoute",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HomePage}/{action=Index}/{id?}");


// 啟用屬性路由
app.MapControllers();

app.Run();
