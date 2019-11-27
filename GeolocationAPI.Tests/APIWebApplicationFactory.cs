using System;
using GeolocationAPI.Persistence;
using GeolocationAPI.Persistence.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GeolocationAPI.Tests
{
    public class APIWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Add a database context (ApplicationDbContext) using an in-memory 
                // database for testing.
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<WebApplicationFactory<Startup>>>();

                    // Ensure the database is created.
                    db.Database.EnsureCreated();

                    try
                    {
                        // Seed the database with test data.
                        InitDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the database. Error: {Message}", ex.Message);
                    }
                }
            });
        }

        private void InitDbForTests(ApplicationDbContext dbContext)
        {
            dbContext.GeolocationData.Add(new GeolocationData{IpAddress = "80.80.80.80", City = "Warszawa", ContinentCode = "EU", CountryCode = "PL", Latitude = 52.234, Longitude= 21.12, CountryName = "Poland", IpAddressType = "ipv4"});
            dbContext.GeolocationData.Add(new GeolocationData { IpAddress = "10.134.50.2", City = "Los Angeles", ContinentCode = "NA", CountryCode = "PL", Latitude = 34.0655, Longitude = -118.240, CountryName = "Poland", IpAddressType = "ipv4" });
            dbContext.SaveChanges();
        }
    }
}