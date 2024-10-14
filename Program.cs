using Backend.Data;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Configure DbContext with SQL Server connection
builder.Services.AddDbContext<ValetParkingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add JWT Service
builder.Services.AddScoped<JwtService>(sp =>
{
    var jwtSecret = builder.Configuration["JwtSettings:Secret"];
    if (string.IsNullOrEmpty(jwtSecret))
    {
        throw new ArgumentNullException(nameof(jwtSecret), "JWT Secret is not configured.");
    }

    var lifespanString = builder.Configuration["JwtSettings:Lifespan"];
    if (!int.TryParse(lifespanString, out int lifespan))
    {
        throw new ArgumentException("Invalid lifespan value.", nameof(lifespanString));
    }

    return new JwtService(jwtSecret, lifespan);
});

// Add Authentication Service
builder.Services.AddScoped<AuthService>();

// Configure CORS to allow specific origins (adjust to your frontend's URL)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:4200")  // Replace with your Angular app URL
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();  // Important for cookies
    });
});

// Configure Cookie policy
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true; // Ensure cookie is accessible only via HTTP(S)
    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always; // Only send cookies over HTTPS
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Set cookie expiration
});

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:Secret"]!);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero // Remove expiration delay
    };
    options.SaveToken = true; // Save the JWT token in the authentication properties
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // For cookie-based token handling
            var accessToken = context.Request.Cookies["access_token"];
            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// Add controllers and other services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Build the application
var app = builder.Build();

// Enable Swagger for testing the API
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS middleware
app.UseCors("AllowSpecificOrigin");

// Enable cookie handling (for storing JWT tokens in cookies)
app.UseCookiePolicy();

// Enable authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map the controllers
app.MapControllers();

app.Run();
