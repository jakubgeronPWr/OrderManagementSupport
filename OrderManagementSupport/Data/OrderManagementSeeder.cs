using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using OrderManagementSupport.Data.Entities;

namespace OrderManagementSupport.Data
{
    public class OrderManagementSeeder
    {
        private readonly OrderManagementContext _ctx;
        private readonly IWebHostEnvironment _hosting;

        public OrderManagementSeeder(OrderManagementContext ctx, IWebHostEnvironment hosting)
        {
            _ctx = ctx;
            _hosting = hosting;
        }

        public void Seed()
        {
            _ctx.Database.EnsureCreated();

            if (!_ctx.Clients.Any())
            {
                var filePath = Path.Combine(_hosting.ContentRootPath, "Data/clients.json");
                var clientJson = File.ReadAllText(filePath);
                var clients = JsonConvert.DeserializeObject<IEnumerable<Client>>(clientJson);
                _ctx.Clients.AddRange(clients);
                _ctx.SaveChanges();
            }

            if (!_ctx.Orders.Any())
            {
                var filePath = Path.Combine(_hosting.ContentRootPath, "Data/orders.json");
                var ordersJson = File.ReadAllText(filePath);
                var orders = JsonConvert.DeserializeObject<IEnumerable<Order>>(ordersJson);
                orders.ToList()[0].Client = _ctx.Clients.ToList()[0];
                orders.ToList()[1].Client = _ctx.Clients.ToList()[0];
                orders.ToList()[2].Client = _ctx.Clients.ToList()[1];
                _ctx.Orders.AddRange(orders);
                _ctx.SaveChanges();
            }

        }
    }
}
