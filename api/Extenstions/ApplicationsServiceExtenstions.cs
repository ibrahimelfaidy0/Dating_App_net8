using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Services;
using Microsoft.EntityFrameworkCore;

namespace api.Extenstions
{
    public static class ApplicationsServiceExtenstions
    {

        public static IServiceCollection AddserviceCollection(this IServiceCollection services,
                  IConfiguration config  )
        {


            services.AddControllers();
            services.AddDbContext<AppDbcontext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<ItokenService, TokenService>();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();


            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                    policy.AllowAnyHeader()
                        .AllowAnyOrigin()
                        .AllowAnyMethod());
            });



            return services;
        }

















    }
}