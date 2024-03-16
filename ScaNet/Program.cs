
using EndPointFinder.Repository.Implementation;
using EndPointFinder.Repository.Interfaces;

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

            builder.Services.AddScoped<IApiFinder, ApiFinder>();
            builder.Services.AddScoped<IEndpointFinder, EndpointFinder>();
            builder.Services.AddScoped<IHelperMethods,  HelperMethods>();
            builder.Services.AddScoped<IMainMethods, MainMethods>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
