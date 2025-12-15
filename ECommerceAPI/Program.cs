using ECommerceAPI.Services;
using ECommerceAPI.Controllers;
using ECommerceAPI.Data; // AppDbContext'i bulmasý için
using Microsoft.EntityFrameworkCore; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// (Dependency Injection)
builder.Services.AddScoped<ECommerceAPI.Services.ICategoryService, ECommerceAPI.Services.CategoryService>();
builder.Services.AddScoped<ECommerceAPI.Services.IProductService, ECommerceAPI.Services.ProductService>();

builder.Services.AddOpenApi(); // .NET 9 yeni özelliði : AddOpenApi arkada çalýþýyor ama Swagger UI ekranýný otomatik getirmiyor.Sadece veri üretiyor, görüntü üretmiyor.
//Bu yüzden manuel olarak Swagger UI endpointini eklememiz gerekiyor.

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Swagger JSON üretir
    app.UseSwagger();
    app.UseSwaggerUI();


}

app.UseHttpsRedirection();

// Endpointleri (Controller'larý) baðlýyoruz
app.MapCategoryEndpoints();
app.MapProductEndpoints();


app.MapGet("/", () => "API Calisiyor!");

app.Run();