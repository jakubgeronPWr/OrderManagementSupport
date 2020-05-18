using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using OrderManagementSupport.Data;
using OrderManagementSupport.Data.Entities;
using OrderManagementSupport.EntityModel;

namespace OrderManagementSupport.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrdersController : Controller
    {
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
                    checkDataValidation(newOrder);
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);

                    var orderNumber = _repo
                        .GetClientOrders(model.ClientId)
                        .LastOrDefault()
                        ?.OrderNumber;
                    newOrder.OrderNumber = (orderNumber != null) ? (int.Parse(orderNumber) + 1).ToString() : "1";

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
                    return BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to post order: {e}");
            }
            return BadRequest(("Failed to save new order"));
        }

        private void checkDataValidation(Order order)
        {
            if (order.OrderDate < DateTime.Now.AddDays(-7) || order.OrderDate > DateTime.Now.AddDays(7))
            {
                ModelState.AddModelError("Errors", "Bad Order Date");
            }

            if (order.OrderRealizationDate < order.OrderDate || order.OrderRealizationDate > order.OrderDate.AddDays(90))
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

                if (order != null) return Ok(_mapper.Map<Order, OrderEntityModel>(order));
                else return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get orders: {e}");
                return BadRequest(("Failed to get orders"));
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
                _logger.LogError($"Failed to put order: {e}");
            }
            return BadRequest(("Failed to edit order"));
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Order> DeleteOrder(int id)
        {
            try
            {
                var order = _repo.DeleteOrderById(id);

                if (order != null && _repo.SaveAll()) return Accepted(_mapper.Map<Order, OrderEntityModel>(order));
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
