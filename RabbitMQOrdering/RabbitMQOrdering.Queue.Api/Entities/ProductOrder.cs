using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RabbitMQOrdering.Queue.Api.Entities
{
    public class ProductOrder : BaseEntity
    {
        public int OrderId { get; set; }

        [JsonIgnore]
        public Order Order { get; set; }

        public int ProductId { get; set; }

        public ProductOrderStatus ProductQueueStatus { get; set; }

        [JsonIgnore]
        public Product Product { get; set; }

    }

    public class ProductOrderDto
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
    }

    public enum ProductOrderStatus
    {
        Waiting = 1,
        Preparing = 2,
        Done = 3
    }
}