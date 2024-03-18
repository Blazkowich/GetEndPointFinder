
using EndPointFinder.Data.Context.Settings;
using EndPointFinder.Models.ApiScanerModels;
using EndPointFinder.Models.EndpointScanerModels;
using EndPointFinder.Repository.Helpers.HelperMethodsImplementation;
using EndPointFinder.Repository.Implementation.ApiFinder;
using EndPointFinder.Repository.Implementation.ApiFinderImpl;
using EndPointFinder.Repository.Implementation.EndpointFinderImpl;
using EndPointFinder.Repository.Interfaces.IApiFinderInterface;
using EndPointFinder.Repository.Interfaces.IEndpointFinderInterface;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ScaNet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddRouting();

            builder.Services.Configure<MongoSettings>(
                        builder.Configuration.GetSection("ScanNetDatabaseSettings"));

            builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                var settings = serviceProvider.GetRequiredService<IOptions<MongoSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
            });

            builder.Services.AddSingleton(serviceProvider =>
            {
                var client = serviceProvider.GetRequiredService<IMongoClient>();
                var settings = serviceProvider.GetRequiredService<IOptions<MongoSettings>>().Value;
                return client.GetDatabase(settings.DatabaseName);
            });

            builder.Services.AddScoped<IMongoCollection<ApiScanerRootModels>>(serviceProvider =>
            {
                var database = serviceProvider.GetRequiredService<IMongoDatabase>();
                var mongoSettings = serviceProvider.GetRequiredService<IOptions<MongoSettings>>().Value;
                return database.GetCollection<ApiScanerRootModels>(mongoSettings.ApiCollectionName);
            });

            builder.Services.AddScoped<IMongoCollection<EndpointScanerRootModels>>(serviceProvider =>
            {
                var database = serviceProvider.GetRequiredService<IMongoDatabase>();
                var mongoSettings = serviceProvider.GetRequiredService<IOptions<MongoSettings>>().Value;
                return database.GetCollection<EndpointScanerRootModels>(mongoSettings.EndpointCollectionName);
            });


            builder.Services.AddScoped<IApiFinderGet, ApiFinderGet>();
            builder.Services.AddScoped<IApiFinderPost, ApiFinderPost>();

            builder.Services.AddScoped<IEndpointFinderGet, EndpointFinderGet>();
            builder.Services.AddScoped<IEndpointFinderPost, EndpointFinderPost>();

            builder.Services.AddScoped<IHelperMethods,  HelperMethods>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseRouting();
            app.MapControllers();

            app.Run();
        }
    }
}
