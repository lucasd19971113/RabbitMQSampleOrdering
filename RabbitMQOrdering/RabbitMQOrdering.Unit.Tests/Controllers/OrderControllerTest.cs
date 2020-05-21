using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RabbitMQOrdering.Api.Controllers;
using RabbitMQOrdering.Api.Entities;
using RabbitMQOrdering.Api.Infrastructure.RabbitMQ;
using RabbitMQOrdering.Api.Services.IServices;
using RabbitMQOrdering.Api.Shared;

namespace RabbitMQOrdering.Unit.Tests.Controllers
{
    [TestFixture]
    public class OrderControllerTest
    {
        private Mock<IOrderService> _orderService;
        private Mock<IProductService> _productService;
        private Mock<IKitchenAreaService> _kitchenAreaService;
        private Mock<IRabbitMQConsumeHelper> _rabbitMQService;

        private OrderController orderController;
    

        public OrderControllerTest () {
            _orderService = new Mock<IOrderService>();

            _productService = new Mock<IProductService> ();

            _kitchenAreaService = new Mock<IKitchenAreaService> ();

            _rabbitMQService = new Mock<IRabbitMQConsumeHelper> ();

            // Options.Create (Configuration.GetSection ("").Get<Config> ());;


            orderController = new OrderController (_orderService.Object, 
                                                        _productService.Object, 
                                                        _kitchenAreaService.Object, 
                                                        _rabbitMQService.Object);

        }

        [Test]
        public async Task ShouldReturnOkIfGetAllDoesNotFail()
        {
            var orderList = CreateOrderList();
            _orderService.Setup(x => x.GetFullOrdersList()).ReturnsAsync(Result.Ok(orderList));

            var result = await orderController.GetAll() as ObjectResult;

            if (result != null) {
                Assert.AreEqual (200, result.StatusCode);

                Assert.AreEqual((typeof(OkObjectResult)), result.GetType());
            }
            
        }

        [Test]
        public async Task ShouldFailWhenThereIsNoOrderList()
        {
            _orderService.Setup(x => x.GetFullOrdersList()).ReturnsAsync(Result.Fail<List<Order>>("No register was found", ResultCode.NoContent));

            var result = await orderController.GetAll() as ObjectResult;

            if (result != null) {
                Assert.AreEqual (204, result.StatusCode);
            }

        }

        [Test]
        public async Task ShouldThrowExceptionOrderServiceAndReturnInternalServerError()
        {
            _orderService.Setup(x => x.GetFullOrdersList()).Throws(new Exception());

            var result = await orderController.GetAll() as ObjectResult;

            if (result != null) {
                Assert.AreEqual (500, result.StatusCode);
            }
        }
        
        public List<Order> CreateOrderList() =>
        new List<Order>{
            new Order{
                Id = 1,
                PointOfSaleId = 1,
                Total = 50m,
                OrderStatus = OrderStatusEnum.Waiting,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ProductOrder = new List<ProductOrder>{
                    new ProductOrder{
                        Id = 1,
                        ProductId = 1,
                        OrderId = 1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new ProductOrder{
                        Id = 2,
                        ProductId = 2,
                        OrderId = 1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    }
                }
            }
        };
    }
}