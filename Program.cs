using Backend.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using DotNetEnv;  // Add this to load environment variables from .env file

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from the .env file
Env.Load();  // This loads the environment variables from the .env file

// Fetch the connection string from environment variables
var connectionString = Environment.GetEnvironmentVariable("Connect_String");

// Add services to the container
builder.Services.AddDbContext<CarParkingContext>(options =>
{
    options.UseSqlServer(connectionString);  // Use the connection string loaded from the .env file
});

// Register services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtServices>();

// Add controllers
builder.Services.AddControllers();

// Configure Authentication and JWT Bearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,  // Ensure token expiration is validated
        ClockSkew = TimeSpan.Zero  // Optional: Reduce clock skew tolerance (e.g., 5 minutes)
    };
})
.AddCookie(options =>
{
    options.LoginPath = "/api/auth/login";  // Redirect to this path for login
    options.LogoutPath = "/api/auth/logout"; // Optional, for logout path
});

// Add CORS policy for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", builder =>
    {
        builder.WithOrigins("http://localhost:4200") // Replace with your Angular app URL
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials(); // Optional: If you're using credentials
    });
});

// Add Swagger services for API documentation and JWT support in Swagger UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Car Parking System API", Version = "v1" });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Error handling middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware configuration for Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car Parking System API v1");
    c.RoutePrefix = "swagger";  // Set the Swagger UI to "/swagger/index.html"
});

// Apply CORS policy before authentication and authorization
app.UseCors("AllowAngularApp");

// Add HTTPS redirection (optional, based on your configuration)
app.UseHttpsRedirection();

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();
