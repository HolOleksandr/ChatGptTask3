using FinanceManagement.DAL.Data;
using FinanceManagement.Models.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FinanceManagement.API.Configurations
{
    public static class BuilderExtentions
    {
        public static void ConfigureDbContext(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<FinanceDbContext>(options =>
            {
                if (connectionString != null)
                    options.UseSqlServer(connectionString);
            });
        }

        public static void ConfigureJWTSettings(this WebApplicationBuilder builder)
        {
            if (builder.Environment.IsProduction())
            {
                builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("ProdJwt"));
            }
            else
            {
                builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("Jwt"));
            }
        }

        public static void ConfigurePlaidSettings(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<PlaidOptions>(builder.Configuration.GetSection("Plaid"));
            builder.Services.AddPlaid(builder.Configuration);

        }
    }
}
