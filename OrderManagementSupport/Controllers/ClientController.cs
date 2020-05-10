using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagementSupport.Data;
using OrderManagementSupport.Data.Entities;
using OrderManagementSupport.EntityModel;

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
                var clients = _mapper.Map<IEnumerable<Client>, IEnumerable<ClientEntityModel>>(_repo.GetAllClients());
                return Ok(clients);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get clients: {e}");
                return BadRequest(("Failed to get client"));
            }
        }

        [HttpPost]
        public IActionResult PostClient([FromBody]ClientEntityModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newClient = _mapper.Map<ClientEntityModel, Client>(model);
                    _repo.AddClient(newClient);
                    if (_repo.SaveAll())
                    {

                        return Created($"/api/orders/{newClient.Id}", _mapper.Map<Client, ClientEntityModel>(newClient));
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to post order: {e}");
            }
            return BadRequest(("Failed to save new order"));
        }

        [HttpPut("{id:int}")]
        public IActionResult PutClient(int id, [FromBody]ClientEntityModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var client = _repo
                        .GetAllClients()
                        .FirstOrDefault(c => c.Id == id);
                    if (client != null)
                    {
                        var newClient = _mapper.Map<ClientEntityModel, Client>(model);
                        newClient.Id = id;
                        _repo.ModifyClient(newClient);
                        if (_repo.SaveAll())
                        {
                            return Accepted($"/api/clients/{newClient.Id}", _mapper.Map<Client, ClientEntityModel>(newClient));
                        }
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to put order: {e}");
            }
            return BadRequest(("Failed to edit order"));
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Client> DeleteClient(int id)
        {
            try
            {
                var client = _repo.DeleteClientById(id);

                if (client != null && _repo.SaveAll()) return Accepted(_mapper.Map<Client, ClientEntityModel>(client));
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get orders: {e}");
                return BadRequest(("Failed to get orders"));
            }

        }



    }
}
