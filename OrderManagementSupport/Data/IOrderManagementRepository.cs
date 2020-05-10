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
        void AddOrder(object order);
        IEnumerable<Order> GetAllOrders();
        Order GetOrderById(int id);
        void ModifyOrder(Order order);
        Order DeleteOrderById(int id);
        IEnumerable<Order> GetAllOrdersSortedByCreationDate();

        void AddClient(object client);
        IEnumerable<Client> GetAllClients();
        void ModifyClient(Client client);
        Client DeleteClientById(int id);

        bool SaveAll();

        
    }
}
