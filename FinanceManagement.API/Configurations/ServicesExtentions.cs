using AutoMapper;
using FinanceManagement.BLL.Automapper;
using FinanceManagement.BLL.Services.Implementations;
using FinanceManagement.BLL.Services.Interfaces;
using FinanceManagement.Core;
using FinanceManagement.DAL.Data;
using FinanceManagement.DAL.Repositories.Implementations;
using FinanceManagement.DAL.Repositories.Interfaces;
using FinanceManagement.DAL.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using System.Reflection;
using FinanceManagement.Core.Enums;
using FluentValidation;
using FinanceManagement.Validation;
using Microsoft.Extensions.Configuration;
using FinanceManagement.Models.Helpers;
using FinanceManagement.BLL.Integrations;

namespace FinanceManagement.API.Configurations
{
    public static class ServicesExtentions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsDefault", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("content-disposition"));
            });
        }

        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBudgetRepository, BudgetRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IUnitOfWork, IUnitOfWork>();
        }

        public static void RegisterServices(this IServiceCollection services) 
        {
            services.AddScoped<IBudgetService, BudgetService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IFinancialTransactionService, FinancialTransactionService>();
            services.AddScoped<IUserAuthService, UserAuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<PlaidTransactionService>();
            services.AddSingleton<PlaidIntegration>();
        }

        public static void ConfigureMapping(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<FinanceDbContext>()
            .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
            });
        }

        public static void ConfigureValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<BudgetDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<CategoryDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<ExpenseDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<FinancialTransactionDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<UserRegistrationModelValidator>();
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "0auth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Chat300API.Api", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            });
        }

        public static void ConfigureAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AllUsers",
                    policy => policy.RequireRole(
                        ApplicationRoles.User.ToString(),
                        ApplicationRoles.Admin.ToString(),
                        ApplicationRoles.SuperAdmin.ToString()));

                options.AddPolicy("UserRole",
                    policy => policy.RequireRole(ApplicationRoles.User.ToString()));

                options.AddPolicy("AdministratorRole",
                    policy => policy.RequireRole(ApplicationRoles.Admin.ToString()));

                options.AddPolicy("SuperAdminRole",
                    policy => policy.RequireRole(ApplicationRoles.SuperAdmin.ToString()));
            });
        }
    }
}