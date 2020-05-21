using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RabbitMQOrdering.Api.Entities;
using RabbitMQOrdering.Api.Infrastructure.RabbitMQ;
using RabbitMQOrdering.Api.Services.IServices;
using RabbitMQOrdering.Api.Shared.Responses;

namespace RabbitMQOrdering.Api.Controllers
{
    public class OrderController : BaseController<IOrderService, Order>
    {
        private readonly IProductService _productService;
        private readonly IKitchenAreaService _kitchenAreaService;
        private readonly IRabbitMQConsumeHelper _rabbitMQService;
        public OrderController(IOrderService service,
                                IProductService productService,
                                IKitchenAreaService kitchenAreaService,
                                IRabbitMQConsumeHelper rabbitMQservice) : base(service)
        {
            _productService = productService;
            _kitchenAreaService = kitchenAreaService;
            _rabbitMQService = rabbitMQservice;
        }


        [HttpGet]
        public override async Task<IActionResult> GetAll () {
            try {
                var result = await service.GetFullOrdersList ();
                if (result) {
                    return Ok (result.Value);
                }

                return StatusCode (result.StatusCode, ErrorDto.Create (result.StatusCode, result.Error));
            } catch (Exception ex) {

                return StatusCode (500, ErrorDto.Create (500, "Internal server error: "+ex.Message));
            }
        }


        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderDto orderDto)
        {
            try
            {
                var order = service.Create(orderDto);

                var addOrder = await service.Add(order);

                if(!addOrder)
                    return BadRequest();

                var count = 0;
                foreach(var item in orderDto.ProductOrder)
                {
                    var result = await _productService.GetById(item.ProductId);

                    if(result != null && result.Value.Price == item.Price){

                        var kitchenQueue = await _kitchenAreaService.GetById(result.Value.KitchenAreaId);

                        if(!kitchenQueue)
                            return StatusCode (kitchenQueue.StatusCode, ErrorDto.Create (kitchenQueue.StatusCode, kitchenQueue.Error));
                        
                        Console.WriteLine(kitchenQueue.Value.Name);

                        var getOrder = await service.GetOrderByCreatedDate(order.CreatedAt);

                        if(!getOrder)
                            return StatusCode (getOrder.StatusCode, ErrorDto.Create (getOrder.StatusCode, getOrder.Error));

                        var productQueue = ProductQueueDto.CreateProductQueueDto(getOrder.Value, result.Value, kitchenQueue.Value, count);

                        var publisher = _rabbitMQService.PublishToQueueAsync(kitchenQueue.Value, productQueue).ConfigureAwait(true);

                        if(!publisher.GetAwaiter().IsCompleted)
                            return BadRequest();       
                    }
                    count ++;
                }
                return Ok(order);
            }
            catch(Exception ex)
            {
                return StatusCode (500, ErrorDto.Create (500, "Internal server error: "+ex.Message));
            }
        }
    }
}