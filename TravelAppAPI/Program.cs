using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Middleware;
using System.Text;
using TravelAppAPI.Infrastructure;
using TravelAppAPI.Middleware;
using TravelAppAPI.Models;
using TravelAppAPI.Services;
using TravelApp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.Configure<TravelAppDatabaseSettings>(
    builder.Configuration.GetSection("TravelAppDatabase"));
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));
builder.Services.AddTransient<PlaceServices>();
builder.Services.AddTransient<AuthServices>();
builder.Services.AddTransient<WishlistServices>();
builder.Services.AddTransient<BookingServices>();
builder.Services.AddTransient<RatingServices>();
builder.Services.AddTransient<FileServices>();
builder.Services.AddTransient<UserServices>();
builder.Services.AddTransient<DashboardServices>();
builder.Services.AddTransient<CacheServices>();
builder.Services.AddDbContext<TravelAppDbContext>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyCors",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173",
                                              "https://quydt.speak.vn", "https://travel.speak.vn").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                      });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseJWTInHeader();
app.UseCheckJwtToken();
app.UseCors("MyCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
