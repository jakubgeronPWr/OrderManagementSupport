﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
                return null;
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
            _ctx.Add(order);
        }

    }
}
