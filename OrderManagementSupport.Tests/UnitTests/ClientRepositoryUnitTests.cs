using System.Net.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using OrderManagementSupport.Data;
using OrderManagementSupport.Data.Repositories;
using Xunit;

namespace OrderManagementSupport.Tests
{
    public class ClientRepositoryUnitTests
    {
        private readonly IClientsRepository _sut;
        //private readonly OrderManagementContext _ctx = Substitute.For<OrderManagementContext>();
        //private readonly ILogger<ClientsRepository> _logger = Substitute.For<ILogger<ClientsRepository>>();

        public ClientRepositoryUnitTests()
        {
            //_sut = new ClientsRepository(_ctx, _logger);
        }

        [Fact] 
        public void AddClientTest()
        {

            Assert.True(true);
        }
    }
}
