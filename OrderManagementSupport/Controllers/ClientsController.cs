using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderManagementSupport.Data;
using OrderManagementSupport.Data.Entities;
using OrderManagementSupport.EntityModel;

namespace OrderManagementSupport.Controllers
{
    [Route("api/[Controller]")]
    public class ClientsController: Controller
    {
        private readonly IClientsRepository _repo;
        private readonly ILogger<ClientsController> _logger;
        private readonly IMapper _mapper;

        public ClientsController(IClientsRepository repo, ILogger<ClientsController> logger, IMapper mapper)
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
                return BadRequest(("Failed to get clients"));
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<IEnumerable<Client>> GetClientsById(int id)
        {
            try
            {
                var client = _mapper.Map<Client, ClientEntityModel>(_repo.GetClientById(id));
                if (client != null)
                {
                    return Ok(client);
                }
                else
                {
                    return NotFound();
                }
                
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get client by id {id}: {e}");
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
                    _logger.LogError($"Failed to post client with wrong model: {ModelState}");
                    return BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to post client: {e}");
            }
            return BadRequest(("Failed to save new client"));
        }

        [HttpPut("{id:int}")]
        public IActionResult PutClient(int id, [FromBody]ClientEntityModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Id = id;
                    var newClient = _mapper.Map<ClientEntityModel, Client>(model);
                    _repo.ModifyClient(newClient);
                    if (_repo.SaveAll())
                    {
                        return Accepted($"/api/clients/{newClient.Id}", _mapper.Map<Client, ClientEntityModel>(newClient));
                    }
                }
                else
                {
                    _logger.LogError($"Failed to put client with id {id} by model validation: {ModelState}");
                    return BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to put client with id {id}: {e}");
            }
            return BadRequest("Failed to edit client");
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Client> DeleteClient(int id)
        {
            try
            {
                var client = _repo.DeleteClientById(id);
                if (client != null && _repo.SaveAll())
                {
                    return Accepted(_mapper.Map<Client, ClientEntityModel>(client));
                }
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to delete client with id {id} : {e}");
                return BadRequest(($"Failed to delete client with id {id}"));
            }

        }
    }
}
