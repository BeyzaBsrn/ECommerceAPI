using ECommerceAPI.Models; 
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace ECommerceAPI
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // Loglama 
            _logger.LogError(exception, "Sunucuda beklenmedik bir hata oluştu: {Message}", exception.Message);

            // response ayarları
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500 Hatası
            httpContext.Response.ContentType = "application/json";

            var response = new ServiceResponse<string>
            {
                Success = false,
                Message = "Sunucu hatası! Lütfen daha sonra tekrar deneyiniz.",
                Data = null // Güvenlik için 
            };

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            return true; 
        }
    }
}