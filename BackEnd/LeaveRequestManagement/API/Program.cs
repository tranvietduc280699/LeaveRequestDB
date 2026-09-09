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
            
            // Controllers
            builder.Services.AddControllers();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

       
            // đăng ký
            // Database
            builder.Services.AddSingleton<DatabaseConnection>();

            // DL
            builder.Services.AddScoped<IUserDL, UserDL>();
            builder.Services.AddScoped<IDepartmentDL, DepartmentDL>();

            // BL
            builder.Services.AddScoped<IAuthBL, AuthBL>();
            builder.Services.AddScoped<IDepartmentBL, DepartmentBL>();

            var app = builder.Build();
            // Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
          

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}