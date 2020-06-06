using Microsoft.EntityFrameworkCore;
using OrderManagementSupport.Data.Entities;

namespace OrderManagementSupport.Data
{
    public class OrderManagementContext : DbContext
    {
        public OrderManagementContext(DbContextOptions<OrderManagementContext> options): base(options)
        {
            
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders{ get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //}
    }
}
