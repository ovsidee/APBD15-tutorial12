using APBD15_tutorial12.DAL;
using APBD15_tutorial12.DAL.Repositories;
using APBD15_tutorial12.Data;
using APBD15_tutorial12.Repositories;
using APBD15_tutorial12.Services;
using Microsoft.EntityFrameworkCore;

namespace APBD15_tutorial12;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        
        //adding controllers
        builder.Services.AddControllers();
    
        //to use DI
        //services
        builder.Services.AddScoped<ITripsService, TripsService>();
        builder.Services.AddScoped<IClientsService, ClientsService>();
        
        //unmit
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        //repositories
        builder.Services.AddScoped<ITripsRepository, TripsRepository>();
        builder.Services.AddScoped<IClientsRepository, ClientsRepository>();
        

        
        //adding dbContext to application
        builder.Services.AddDbContext<TripsDbContext>(opt =>
        {
            var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
            opt.UseSqlServer(connectionString);
        });
        
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}