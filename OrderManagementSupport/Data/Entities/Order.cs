using System;

namespace OrderManagementSupport.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime OrderRealizationDate { get; set; }
        public string Service { get; set; }
        public double Price { get; set; }
        public bool IsPayed { get; set; }
        public Client Client { get; set; }
    }
}
