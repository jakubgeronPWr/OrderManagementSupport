using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderManagementSupport.Data;
using OrderManagementSupport.Data.Entities;
using OrderManagementSupport.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderManagementSupport.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrdersController : Controller
    {
        private readonly int MAXIMUM_REALIZATION_TIME_DAYS = 90;
        private readonly int MAXIMUM_ORDER_BUFFER_TIME_DAYS = 7;

        private readonly IOrdersRepository _repo;
        private readonly IClientsRepository _clientsRepo;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;

        public OrdersController(IOrdersRepository repo, IClientsRepository clientRepo, ILogger<OrdersController> logger, IMapper mapper)
        {
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
            _clientsRepo = clientRepo;
        }


        [HttpPost]
        public IActionResult PostOrder([FromBody]OrderEntityModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = _mapper.Map<OrderEntityModel, Order>(model);
                    CheckDataValidation(newOrder);
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    var client = _clientsRepo
                        .GetClientById(model.ClientId);

                    string firsName = client.FirstName.ToUpper();
                    string lastName = client.LastName.ToUpper();
                    
                    newOrder.OrderNumber =
                        $"{firsName[0]}{lastName.Substring(0, 3)}-{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}";

                    newOrder.Client = _clientsRepo
                        .GetAllClients()
                        .FirstOrDefault(c => c.Id == model.ClientId);
                    _repo.AddOrder(newOrder);
                    if (_repo.SaveAll())
                    {
                        return Created($"/api/orders/{newOrder.Id}", _mapper.Map<Order, OrderEntityModel>(newOrder));
                    }
                }
                else
                {
                    _logger.LogDebug($"bad request from post order made by : {ModelState} ");
                    return BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to post order: {e}");
            }
            return BadRequest(("Failed to save new order"));
        }

        private void CheckDataValidation(Order order)
        {
            if (order.OrderDate < DateTime.Now.AddDays(-MAXIMUM_ORDER_BUFFER_TIME_DAYS) || order.OrderDate > DateTime.Now.AddDays(MAXIMUM_ORDER_BUFFER_TIME_DAYS))
            {
                ModelState.AddModelError("Errors", "Bad Order Date");
            }

            if (order.OrderRealizationDate < order.OrderDate || order.OrderRealizationDate > order.OrderDate.AddDays(MAXIMUM_REALIZATION_TIME_DAYS))
            {
                ModelState.AddModelError("Errors", "Bad Order Realization Date");
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<Order>> GetOrders()
        {
            try
            {
                var orders = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderEntityModel>>(_repo.GetAllOrders());
                return Ok(orders);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get orders: {e}");
                return BadRequest(("Failed to get orders"));
            }

        }

        [HttpGet("{id:int}")]
        public ActionResult<Order> GetOrder(int id)
        {
            try
            {
                var order = _repo.GetOrderById(id);

                if (order != null)
                {
                    return Ok(_mapper.Map<Order, OrderEntityModel>(order));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get order with id {id}: {e}");
                return BadRequest(($"Failed to get order with id {id}"));
            }

        }

        [HttpPut("{id:int}")]
        public IActionResult PutOrder(int id, [FromBody]OrderEntityModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var order = _repo
                        .GetAllOrders()
                        .FirstOrDefault(o => o.Id == id);
                    if (order != null)
                    {
                        var newOrder = _mapper.Map<OrderEntityModel, Order>(model);
                        newOrder.Id = id;
                        _repo.ModifyOrder(newOrder);
                        if (_repo.SaveAll())
                        {
                            return Accepted($"/api/clients/{newOrder.Id}", _mapper.Map<Order, OrderEntityModel>(newOrder));
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
                _logger.LogError($"Failed to put order with id {id}: {e}");
            }
            return BadRequest(($"Failed to edit order with id {id}"));
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Order> DeleteOrder(int id)
        {
            try
            {
                var order = _repo.DeleteOrderById(id);

                if (order != null && _repo.SaveAll())
                {
                    return Accepted(_mapper.Map<Order, OrderEntityModel>(order));
                }
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to delete order with id {id}: {e}");
                return BadRequest(($"Failed to delete order with id {id}"));
            }

        }

    }
}
