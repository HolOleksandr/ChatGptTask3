using FinanceManagement.Models.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FinanceManagement.API.Configurations
{
    public static class JWTBearerExtensions
    {
        public static void ConfigureJWT(this IServiceCollection services, WebApplicationBuilder builder)
        {
            var jwtSettings = new JWTSettings();
            var configuration = builder.Configuration;

            if (builder.Environment.IsProduction())
            {
                configuration.GetSection("ProdJwt").Bind(jwtSettings);
            }
            else
            {
                configuration.GetSection("Jwt").Bind(jwtSettings);
            }

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });
        }
    }
}
