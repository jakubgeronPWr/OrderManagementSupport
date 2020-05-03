using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderManagementSupport.Data;

namespace OrderManagementSupport.Controllers
{
    public class AppController: Controller
    {
        private readonly IOrderManagementRepository _repo;
        private readonly ILogger<AppController> _logger;

        public AppController(IOrderManagementRepository repo, ILogger<AppController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var result = _repo.GetAllOrders();
            return View();
        }

    }
}
