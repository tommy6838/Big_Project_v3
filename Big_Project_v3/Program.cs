using Big_Project_v3.Models;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// 添加服務到 DI 容器
builder.Services.AddDbContext<ITableDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("iTableDBConnection")));
// 設定使用 SQL Server 的 iTableDbContext，連接字串從 appsettings.json 中的 iTableDBConnection 讀取

builder.Services.AddControllersWithViews(); // 註冊 MVC 服務

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SearchPage}/{action=Index}/{id?}");

app.Run();
