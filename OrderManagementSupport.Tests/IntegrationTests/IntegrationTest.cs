using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OrderManagementSupport.Data;

namespace OrderManagementSupport.Tests.IntegrationTests
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;
        protected readonly JsonSerializer Serializer;

        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        ReplaceCoreServices<OrderManagementContext>(services, (p, o) =>
                        {
                            o.UseInMemoryDatabase("DB");
                        }, ServiceLifetime.Scoped);

                    });
                });
            TestClient = appFactory.CreateClient();
            Serializer = new JsonSerializer();
        }

        private static void ReplaceCoreServices<TContextImplementation>(IServiceCollection serviceCollection,
            Action<IServiceProvider, DbContextOptionsBuilder> optionsAction,
            ServiceLifetime optionsLifetime) where TContextImplementation : DbContext
        {
            serviceCollection.Add(new ServiceDescriptor(typeof(DbContextOptions<TContextImplementation>),
                (IServiceProvider p) => DbContextOptionsFactory<TContextImplementation>(p, optionsAction), optionsLifetime));
            serviceCollection.Add(new ServiceDescriptor(typeof(DbContextOptions),
                (IServiceProvider p) => p.GetRequiredService<DbContextOptions<TContextImplementation>>(), optionsLifetime));
        }

        private static DbContextOptions<TContext> DbContextOptionsFactory<TContext>(IServiceProvider applicationServiceProvider,
            Action<IServiceProvider, DbContextOptionsBuilder> optionsAction) where TContext : DbContext
        {
            DbContextOptionsBuilder<TContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<TContext>(
                new DbContextOptions<TContext>(new Dictionary<Type, IDbContextOptionsExtension>()));
            dbContextOptionsBuilder.UseApplicationServiceProvider(applicationServiceProvider);
            optionsAction?.Invoke(applicationServiceProvider, dbContextOptionsBuilder);
            return dbContextOptionsBuilder.Options;
        }
    }
}



//protected IntegrationTest()
//{
//    var appFactory = new WebApplicationFactory<Startup>()
//        .WithWebHostBuilder(builder =>
//        {
//            builder.ConfigureTestServices(services =>
//            {
//                var descriptor = services.SingleOrDefault(
//                    d => d.ServiceType ==
//                         typeof(DbContextOptions<OrderManagementContext>));

//                if (descriptor != null)
//                {
//                    services.Remove(descriptor);
//                }

//                var serviceProvider = new ServiceCollection()
//                    .AddEntityFrameworkInMemoryDatabase()
//                    .BuildServiceProvider();

//                services.AddDbContext<OrderManagementContext>(options =>
//                {
//                    options.UseInMemoryDatabase("TestDb");
//                            //options.UseInternalServiceProvider(serviceProvider);
//                        });
//                var sp = services.BuildServiceProvider();

//                using (var scope = sp.CreateScope())
//                {
//                    var scopedServices = scope.ServiceProvider;
//                    var db = scopedServices.GetRequiredService<OrderManagementContext>();
//                    var logger = scopedServices
//                        .GetRequiredService<ILogger<IntegrationTest>>();

//                    // Ensure the database is created.
//                    db.Database.EnsureCreated();

//                    try
//                    {
//                        // Seed the database with test data.
//                        Utilities.InitializeDbForTests(db);
//                    }
//                    catch (Exception ex)
//                    {
//                        logger.LogError(ex, "An error occurred seeding the " +
//                                            "database with test messages. Error: {Message}", ex.Message);
//                    }
//                }

//            });
//        });
//    TestClient = appFactory.CreateClient();
//    Serializer = new JsonSerializer();
//}