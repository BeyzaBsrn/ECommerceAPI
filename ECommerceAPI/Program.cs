using ECommerceAPI;
using ECommerceAPI.Data;
using ECommerceAPI.Services;
using ECommerceAPI.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.IdentityModel.Tokens; 
using System.Text; 
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Veritabaný Baðlantýsý
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// JSON Ayarlarý
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Exception Handler (Hata Yakalayýcý)
builder.Services.AddExceptionHandler<GlobalExceptionHandler>(); // Burada eski usul Custom Middleware yazmak yerine IExceptionHandler arayüzünü ve ProblemDetails servisini kullandým.Bu yapý, .NET 9 ile gelen modern Minimal API mimarisine çok daha uygun.
builder.Services.AddProblemDetails();

//Jwt kullanacaðýmýzý belirtiyoruz
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true, 
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("JwtSettings:SecretKey").Value!)), 
            ValidateIssuer = false, 
            ValidateAudience = false 
        };
    });

builder.Services.AddAuthorization();

// Dependency Injection (Servisler)
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();  

app.MapCategoryEndpoints();
app.MapProductEndpoints();
app.MapUserEndpoints();
app.MapOrderEndpoints();

app.Run();