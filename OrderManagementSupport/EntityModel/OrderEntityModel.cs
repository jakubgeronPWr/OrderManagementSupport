using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagementSupport.Data.Entities;

namespace OrderManagementSupport.EntityModel
{
    public class OrderEntityModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime OrderRealizationDate { get; set; }
        [Required]
        [MinLength(6)]
        public string Service { get; set; }
        public double Price { get; set; }
        [DefaultValue(false)]
        public bool IsPayed { get; set; }
        public int ClientId { get; set; }
    }
}
