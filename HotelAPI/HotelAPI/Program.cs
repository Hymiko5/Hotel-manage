

using Confluent.Kafka;
using Elasticsearch.Net;
using HotelAPI;
using HotelAPI.Extensions;
using HotelAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Nest;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("HotelDB");

builder.Services.AddDbContext<HotelContext>(options => options.UseSqlServer(connectionString));






builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});


var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app, builder.Environment);


app.MapControllers();

app.Run();


