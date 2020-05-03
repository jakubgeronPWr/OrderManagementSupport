using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderManagementSupport.Data;
using OrderManagementSupport.Data.Entities;

namespace OrderManagementSupport.Controllers
{
    [Route("api/[Controller]")]
    public class ClientsController: Controller
    {
        private readonly IOrderManagementRepository _repo;
        private readonly ILogger<ClientsController> _logger;
        private readonly IMapper _mapper;

        public ClientsController(IOrderManagementRepository repo, ILogger<ClientsController> logger, IMapper mapper)
        {
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Client>> GetClients()
        {
            try
            {
                return Ok(_repo.GetAllClients());
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get clients: {e}");
                return BadRequest(("Failed to get client"));
            }
        }



    }
}
