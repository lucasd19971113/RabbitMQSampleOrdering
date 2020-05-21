using System.Collections.Generic;

namespace RabbitMQOrdering.Queue.Api.Entities
{
    public class KitchenArea : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}