using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CarRentalSystem.Filters;
using CarRentalSystem.Repositories;
using CarRentalSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext for CarRentalContext with transient error resiliency
builder.Services.AddDbContext<CarRentalContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(3) // Retry up to 3 times
    )
);

// Add Scoped services for repositories and services
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICarRentalService, CarRentalService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKeyHere")),
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = "YourIssuer",  // Set your issuer here
        ValidAudience = "YourAudience"  // Set your audience here
    };
});

// Add services for controllers and filters
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ValidateModelStateFilter()); // Add custom validation filter globally
});

// Add services for Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Exclude Swagger from JWT Authentication (don't require JWT token for Swagger UI)
app.UseWhen(context => !context.Request.Path.StartsWithSegments("/swagger"), appBuilder =>
{
    appBuilder.UseAuthentication();
    appBuilder.UseAuthorization();
});

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable Authentication and Authorization middlewares (applies only to non-Swagger routes)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();  // Map controller endpoints

app.Run();
