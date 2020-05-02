using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSupport.Data;

namespace OrderManagementSupport.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderManagementRepository _repository;
        public OrdersController(IOrderManagementRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var result = _repository.GetAllOrders();
            return View();
        }

        [HttpGet]
        public IActionResult AllOrders()
        {
            var result = _repository.GetAllOrders();
            return View(result.ToList());
        }

    }
}
