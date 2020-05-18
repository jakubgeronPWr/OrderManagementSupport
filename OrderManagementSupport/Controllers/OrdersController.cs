﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IOrderManagementRepository _repository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;

        public OrdersController(IOrderManagementRepository repository, ILogger<OrdersController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult PostOrder([FromBody]OrderEntityModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = _mapper.Map<OrderEntityModel, Order>(model);

                    if (newOrder.OrderDate == DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }
                    newOrder.Client = _repository
                        .GetAllClients()
                        .FirstOrDefault(c => c.Id == model.ClientId);
                    _repository.AddOrder(newOrder);
                    if (_repository.SaveAll())
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

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<Order>> GetOrders()
        {
            try
            {
                var orders = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderEntityModel>>(_repository.GetAllOrders());
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
                var order = _repository.GetOrderById(id);

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
                    var order = _repository
                        .GetAllOrders()
                        .FirstOrDefault(o => o.Id == id);
                    if (order != null)
                    {
                        var newOrder = _mapper.Map<OrderEntityModel, Order>(model);
                        newOrder.Id = id;
                        _repository.ModifyOrder(newOrder);
                        if (_repository.SaveAll())
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
                var order = _repository.DeleteOrderById(id);

                if (order != null && _repository.SaveAll()) return Accepted(_mapper.Map<Order, OrderEntityModel>(order));
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
