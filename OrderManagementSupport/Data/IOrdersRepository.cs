using System.Collections.Generic;
using OrderManagementSupport.Data.Entities;

namespace OrderManagementSupport.Data
{
    public interface IOrdersRepository
    {
        void AddOrder(object order);
        IEnumerable<Order> GetAllOrders();
        IEnumerable<Order> GetClientOrders(int clientId);
        IEnumerable<Order> GetOrdersByPagination(int paginationSize, int pageNumber);
        Order GetOrderById(int id);
        Order ModifyOrder(Order order);
        Order DeleteOrderById(int id);
        IEnumerable<Order> GetAllOrdersSortedByCreationDate();
        bool SaveAll();
    }
}
