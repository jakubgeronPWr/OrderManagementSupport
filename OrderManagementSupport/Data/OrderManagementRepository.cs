using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<Client> GetAllClients()
        {
            try
            {
                _logger.LogInformation($"GetAllOrders was called with order numbers: {_ctx.Clients.ToList().Count}");
                return _ctx.Clients
                    .OrderBy(c => c.LastName)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all products: {ex}");
                return Enumerable.Empty<Client>();
            }
        }

        public Client DeleteClientById(int id)
        {
            try
            {
                var client = _ctx.Clients.Where(c => c.Id == id).FirstOrDefault();
                _ctx.Remove(client);
                return client;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete client by id: {ex}");
                return null;
            }
        }

        public IEnumerable<Order> GetAllOrders()
        {
            try
            {
                _logger.LogInformation($"GetAllOrders was called with order numbers: {_ctx.Orders.ToList().Count}");
                return _ctx.Orders
                    .Include( o => o.Client)
                    .OrderBy(o => o.OrderRealizationDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all products: {ex}" );
                return Enumerable.Empty<Order>();
            }
        }

        public Order GetOrderById(int id)
        {
            try
            {
               return  _ctx.Orders
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

        public void AddOrder(object order)
        {
            _ctx.AddAsync(order);
        }

        public void AddClient(object client)
        {
            _ctx.AddAsync(client);
        }

        public void ModifyClient(Client client)
        {
            _ctx.Update(client);
        }
    }
}
