using System.Collections.Generic;

namespace RabbitMQOrdering.Queue.Api.Entities
{
    public class PointOfSale
    {
        public int Id { get; set; }
        public PointOfSaleType PointOfSaleType { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

    public enum PointOfSaleType
    {
        Cashier = 1,
        SelfServiceTotem = 2
    }
}