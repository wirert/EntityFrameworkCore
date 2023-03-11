using Microsoft.EntityFrameworkCore;
using FastFood.Core.MappingConfiguration;
using FastFood.Data;
using FastFood.Services;
using FastFood.Services.Interfaces;
using FastFood.Services.Models.Categories;
using FastFood.Services.Models.Employees;
using FastFood.Services.Models.Items;
using FastFood.Services.Models.Positons;
using FastFood.Services.Models.Orders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<FastFoodContext>(options =>
                options.UseSqlServer(connectionString));

builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<FastFoodProfile>();
});

builder.Services.AddTransient<IService<CreateCategoryDto, ListCategoryDto>, CategoriesService> ();
builder.Services.AddTransient<IService<CreateItemDto, ListItemDto>, ItemsService>();
builder.Services.AddTransient<IService<CreatePositionDto, ListPositionDto>, PositionsService>();
builder.Services.AddTransient<IService<RegisterEmployeeDto, ListEmployeeDto>, EmployeesService>();
builder.Services.AddTransient<IService<CreateOrderDto, ListOrderDto>, OrdersService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
