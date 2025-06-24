using Code_API_Mobile.Models;
using Microsoft.EntityFrameworkCore;

namespace Code_API_Mobile
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ✅ Cấu hình kết nối DB
            builder.Services.AddDbContext<MobileDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("myCnn")));

            // ✅ Cấu hình controller & JSON (KHÔNG dùng Preserve)
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                // ✅ Loại bỏ Preserve để không có $id, $values
                options.JsonSerializerOptions.ReferenceHandler = null;
                options.JsonSerializerOptions.WriteIndented = true;
            });

            // ✅ Cấu hình Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ✅ Cấu hình CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // ✅ Middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
