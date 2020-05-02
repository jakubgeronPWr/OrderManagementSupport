using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OrderManagementSupport.Data.Entities;

namespace OrderManagementSupport.Data
{
    public class OrderManagementRepository: IOrderManagementRepository
    {
        private readonly OrderManagementContext _ctx;
        private readonly ILogger<OrderManagementRepository> _logger;

        public OrderManagementRepository(OrderManagementContext ctx, ILogger<OrderManagementRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            try
            {
                _logger.LogInformation($"GetAllOrders was called with order numbers: {_ctx.Orders.ToList().Count}");
                return _ctx.Orders
                    .OrderBy(o => o.OrderRealizationDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all products: {ex}" );
                return null;
            }
            
        }

        public IEnumerable<Order> GetAllOrdersSortedByCreationDate()
        {
            return _ctx.Orders
                .OrderBy(o => o.OrderDate)
                .ToList();
        }

        public bool SaveAll()
        {
            return _ctx.SaveChanges() > 0;
        }

    }
}
