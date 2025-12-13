using ECommerceAPI.Data; // AppDbContext'i bulmasý için
using Microsoft.EntityFrameworkCore; // UseSqlServer'ý bulmasý için

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabaný Baðlantýsýný Yapýlandýrýyoruz
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Servisleri ekliyoruz (OpenAPI/Swagger)
builder.Services.AddOpenApi();

var app = builder.Build();

// 3. HTTP Ýstek Hattýný Yapýlandýrýyoruz
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Swagger JSON üretir
    // .NET 9'da bazen SwaggerUI için ekstra kütüphane gerekir,
    // ama þimdilik sadece JSON üretmesi yeterli, o konuya sonra deðiniriz.
}

app.UseHttpsRedirection();

// Þimdilik test amaçlý basit bir endpoint býrakalým, çalýþtýðýný görelim
app.MapGet("/", () => "API Calisiyor!");

app.Run();