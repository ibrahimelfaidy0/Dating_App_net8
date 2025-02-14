using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace api.Extenstions
{
    public static class IdentityIserviceCollection
    {
        public static IServiceCollection AddIdentiyServices(this IServiceCollection services, IConfiguration confg)
        {
             services.AddAuthentication(
                    JwtBearerDefaults.AuthenticationScheme
                    ).AddJwtBearer(options =>
                    {
                    var tokenkey = confg["TokenKey"] ?? throw new Exception("token key not found");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenkey)),
                    ValidateIssuer = false, // Set to true and provide valid issuer if needed
                    ValidateAudience = false, // Set to true and provide valid audience if needed
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // Prevents token expiration delays
                    };
                    });



                    return services;



        }













    }






}