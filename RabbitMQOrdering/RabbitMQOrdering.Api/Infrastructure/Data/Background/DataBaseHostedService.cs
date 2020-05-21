using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQOrdering.Api.Entities;
using RabbitMQOrdering.Api.Services.IServices;

namespace RabbitMQOrdering.Api.Infrastructure.Data.Background
{
    public class DataBaseHostedService : BackgroundService
    {
        private IServiceProvider _serviceProvider;
        private CancellationTokenSource _stoppingCts =
                                                   new CancellationTokenSource();
        public DataBaseHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (true)
                {
                    await CheckOrderDoneStatus();

                    await Task.Delay(3000);
                }
            }
            catch
            {
                _stoppingCts =  new CancellationTokenSource();

                await ExecuteAsync(_stoppingCts.Token);   
            }
        }

        public async Task CheckOrderDoneStatus()
        {
            if(_serviceProvider != null)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    await UpdateOrderToDoneStatus(scope);
                }
            }
        }


        public async Task UpdateOrderToDoneStatus(IServiceScope scope)
        {
            var _orderservice = scope.ServiceProvider.GetService<IOrderService>();

            var result = await _orderservice.GetWaitingAndPreparingOrders ();

            if(result)
            {
                if(result.Value.AsEnumerable().Any())
                {
                    foreach (var order in result.Value)
                    {
                        var updatedOrder = new Order();
                        var orderDone = order.ProductOrder.Count();

                        var productsList = order.ProductOrder.ToList();

                        if(productsList.AsEnumerable().Any())
                        {
                            foreach (var product in productsList)
                            {
                                if(product.ProductQueueStatus == ProductOrderStatus.Done)
                                {
                                    orderDone --;
                                }
                            }

                            if(orderDone == 0)
                            {
                                updatedOrder = order;
                                updatedOrder.OrderStatus = OrderStatusEnum.Done;
                                await _orderservice.Update(updatedOrder);
                            }
                        }    
                    }
                }
            }     
        }
    }
}