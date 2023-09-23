using FinanceManagement.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureAuthorizationPolicies();
builder.Services.ConfigureCors();
builder.ConfigureDbContext();
builder.Services.ConfigureIdentity();
builder.ConfigureJWTSettings();
builder.ConfigurePlaidSettings();
builder.Services.ConfigureJWT(builder);
builder.Services.ConfigureSwagger();
builder.Services.ConfigureValidators();
builder.Services.RegisterRepositories();
builder.Services.RegisterServices();
builder.Services.ConfigureMapping();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors("CorsDefault");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
