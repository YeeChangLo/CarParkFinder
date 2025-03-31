using Microsoft.EntityFrameworkCore;
using CarParkFinder.Infrastructure.Persistence;
using CarParkFinder.BackgroundServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Load JWT settings
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(CarParkMappingProfile));

// Configure Database **before** builder.Build()
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CarParkDb"));
    options.EnableSensitiveDataLogging(); // Helps debug conflicting key values
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); // Prevents duplicate tracking
});


// Get the correct project directory example...(D:\LYC\Project)
var baseDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
var filePath = Path.Combine(baseDirectory, "ProjectData", "HDBCarparkInformation.csv");
//Insert data with Csv Importer...Change the pathe here
await new CsvImporter(builder.Configuration.GetConnectionString("CarParkDb"))
    .ImportCarParkDataAsync(filePath);


// Register HttpClient and Services
builder.Services.AddHttpClient<CarParkAvailabilityService>();
builder.Services.AddScoped<CarParkAvailabilityService>();
builder.Services.AddHostedService<CarParkBackgroundService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

