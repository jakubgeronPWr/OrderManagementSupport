using System.Collections.Generic;
using OrderManagementSupport.Data.Entities;

namespace OrderManagementSupport.Data
{
    public interface IClientsRepository
    {
        void AddClient(object client);
        IEnumerable<Client> GetAllClients();
        Client GetClientById(int id);
        Client ModifyClient(Client client);
        Client DeleteClientById(int id);

        bool SaveAll();
    }
}
