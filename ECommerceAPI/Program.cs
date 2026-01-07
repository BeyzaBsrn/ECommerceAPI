using ECommerceAPI.Data;
using ECommerceAPI.Services;
using ECommerceAPI.Controllers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Veritabaný Baðlantýsý
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servislerin Tanýmlanmasý (Dependency Injection)
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>(); // Yeni ekledik

// Swagger ve API Ayarlarý
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Geliþtirme Ortamý Ayarlarý
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Endpoint Haritalarý
app.MapCategoryEndpoints();
app.MapProductEndpoints();
app.MapUserEndpoints(); // Yeni ekledik

app.Run();