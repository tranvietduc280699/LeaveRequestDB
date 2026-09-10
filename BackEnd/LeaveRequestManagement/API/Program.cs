using BL;
using BL.Interfaces;
using DL;
using DL.Database;
using DL.Interfaces;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // 1. Đăng ký CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Frontend", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:5500")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            builder.Services.AddSingleton<DatabaseConnection>();

            builder.Services.AddScoped<IUserDL, UserDL>();
            builder.Services.AddScoped<IDepartmentDL, DepartmentDL>();

            builder.Services.AddScoped<IAuthBL, AuthBL>();
            builder.Services.AddScoped<IDepartmentBL, DepartmentBL>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // 2. Sử dụng CORS
            app.UseCors("Frontend");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}