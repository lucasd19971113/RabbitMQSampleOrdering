using System;

namespace RabbitMQOrdering.Queue.Api.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt{ get; set; } =  DateTime.Now;
    }
}