using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
                        services.RemoveAll(typeof(OrderManagementContext));
                        var serviceProvider = new ServiceCollection()
                            .AddEntityFrameworkInMemoryDatabase()
                            .BuildServiceProvider();

                        services.AddDbContext<OrderManagementContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                            options.UseInternalServiceProvider(serviceProvider);
                        });
                        services.BuildServiceProvider();
                    });
                });
            TestClient = appFactory.CreateClient();
            Serializer = new JsonSerializer();
        }
    }
}
