using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagementSupport.Data.Entities;

namespace OrderManagementSupport.Data
{
    public interface IOrderManagementRepository
    {
        IEnumerable<Client> GetAllClients();
        IEnumerable<Order> GetAllOrders();
        Order GetOrderById(int id);
        IEnumerable<Order> GetAllOrdersSortedByCreationDate();
        bool SaveAll();

        void AddOrder(object order);
    }
}
