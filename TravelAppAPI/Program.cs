using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Middleware;
using System.Text;
using TravelAppAPI.Infrastructure;
using TravelAppAPI.Middleware;
using TravelAppAPI.Models;
using TravelAppAPI.Sevices;
using TravelApp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TravelAppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.Configure<TravelAppDatabaseSettings>(
    builder.Configuration.GetSection("TravelAppDatabase"));
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));
builder.Services.AddSingleton<PlaceServices>();
builder.Services.AddSingleton<AuthServices>();
builder.Services.AddSingleton<WishlistServices>();
builder.Services.AddSingleton<BookingServices>();
builder.Services.AddSingleton<UserServices>();
builder.Services.AddSingleton<FileServices>();
builder.Services.AddSingleton<RatingServices>();
builder.Services.AddSingleton<DashboardServices>();
builder.Services.AddSingleton<CacheServices>();
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
