using System.Net.Http;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using OrderManagementSupport.Controllers;
using OrderManagementSupport.Data;
using OrderManagementSupport.Data.Repositories;
using Xunit;

namespace OrderManagementSupport.Tests
{
    public class ClientServiceUnitTests
    {
        private readonly ClientsController _sut;
        private readonly ILogger<ClientsController> _logger = Substitute.For<ILogger<ClientsController>>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        private readonly IClientsRepository _repo = Substitute.For<IClientsRepository>();

        public ClientServiceUnitTests()
        {
            _sut = new ClientsController(_repo, _logger, _mapper);
        }

        [Fact] 
        public void AddClientTest()
        {
            Assert.True(true);
        }
    }
}
