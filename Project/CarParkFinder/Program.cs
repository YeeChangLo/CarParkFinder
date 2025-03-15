using Microsoft.EntityFrameworkCore;
using CarParkFinder.Infrastructure.Persistence;
using CarParkFinder.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Database **before** builder.Build()
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CarParkDb"));
    options.EnableSensitiveDataLogging(); // Helps debug conflicting key values
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); // Prevents duplicate tracking
});


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

//Insert data with Csv Importer
//await new CsvImporter(builder.Configuration.GetConnectionString("CarParkDb"))
//    .ImportCarParkDataAsync("D:\\LYC\\Project\\ProjectData\\HDBCarparkInformation.csv");



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

