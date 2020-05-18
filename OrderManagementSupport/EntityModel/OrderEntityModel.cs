using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderManagementSupport.EntityModel
{
    public class OrderEntityModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
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
