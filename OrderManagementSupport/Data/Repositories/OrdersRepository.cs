using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagementSupport.Data.Entities;

namespace OrderManagementSupport.Data.Repositories
{
    public class OrdersRepository: IOrdersRepository
    {
        private readonly OrderManagementContext _ctx;
        private readonly ILogger<OrdersRepository> _logger;


        public OrdersRepository(OrderManagementContext ctx, ILogger<OrdersRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public void AddOrder(object order)
        {
            _ctx.AddAsync(order);
        }

        public IEnumerable<Order> GetAllOrders()
        {
            try
            {
                _logger.LogInformation($"GetAllOrders was called with order numbers: {_ctx.Orders.ToList().Count}");
                return _ctx.Orders
                    .Include(o => o.Client)
                    .OrderBy(o => o.OrderRealizationDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all products: {ex}");
                return Enumerable.Empty<Order>();
            }
        }

        public IEnumerable<Order> GetClientOrders(int clientId)
        {
            try
            {
                _logger.LogInformation($"GetClientOrders was called with order numbers: {_ctx.Orders.ToList().Count}");
                return _ctx.Orders
                    .Include(o => o.Client)
                    .Where(o => o.Client.Id == clientId)
                    .OrderBy(o => o.OrderNumber)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get client products: {ex}");
                return Enumerable.Empty<Order>();
            }
        }

        public IEnumerable<Order> GetOrdersByPagination(int paginationSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetAllOrdersSortedByCreationDate()
        {
            return _ctx.Orders
                .OrderBy(o => o.OrderDate)
                .ToList();
        }

        public Order GetOrderById(int id)
        {
            try
            {
                return _ctx.Orders
                    .Include(o => o.Client)
                    .Where(o => o.Id == id)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get order by id: {ex}");
                return null;
            }
        }

        public void ModifyOrder(Order order)
        {
            _ctx.Update(order);
        }

        public Order DeleteOrderById(int id)
        {
            try
            {
                var order = _ctx.Orders.Where(o => o.Id == id).FirstOrDefault();
                _ctx.Remove(order);
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete order by id: {ex}");
                return null;
            }
        }

        public bool SaveAll()
        {
            return _ctx.SaveChanges() > 0;
        }

    }

}