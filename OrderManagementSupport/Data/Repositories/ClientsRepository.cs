using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using OrderManagementSupport.Data.Entities;

namespace OrderManagementSupport.Data.Repositories
{
    public class ClientsRepository : IClientsRepository
    {
        private readonly OrderManagementContext _ctx;
        private readonly ILogger<ClientsRepository> _logger;

        public ClientsRepository(OrderManagementContext ctx, ILogger<ClientsRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }


        public void AddClient(object client)
        {
            _ctx.AddAsync(client);
        }

        public IEnumerable<Client> GetAllClients()
        {
            try
            {
                _logger.LogInformation($"GetAllClients was called with order numbers: {_ctx.Clients.ToList().Count}");
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

        public Client GetClientById(int id)
        {
            try
            {
                _logger.LogInformation($"GetClient was called with order numbers: {_ctx.Clients.ToList().Count}");
                return _ctx.Clients
                    .Where(c => c.Id == id)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get client with id: {ex}");
                return null;
            }
        }

        public void ModifyClient(Client client)
        {
            _ctx.Update(client);
        }

        public Client DeleteClientById(int id)
        {
            try
            {
                var client = _ctx.Clients
                    .Where(c => c.Id == id)
                    .FirstOrDefault();
                _ctx.Remove(client);
                return client;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete client by id: {ex}");
                return null;
            }
        }

        public bool SaveAll()
        {
            return _ctx.SaveChanges() > 0;
        }

    }
}
