using Microsoft.EntityFrameworkCore;
using CarParkFinder.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CarParkFinderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CarParkDb")));
builder.Services.AddSingleton(new CsvImporter(builder.Configuration.GetConnectionString("CarParkDb")));
builder.Services.AddHttpClient<CarParkAvailabilityService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Insert data with Csv Importer
//await new CsvImporter(builder.Configuration.GetConnectionString("CarParkDb"))
//    .ImportCarParkDataAsync("D:\\LYC\\CarPark\\ProjectData\\HDBCarparkInformation.csv");


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

