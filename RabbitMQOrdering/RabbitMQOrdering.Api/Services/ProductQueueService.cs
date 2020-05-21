using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQOrdering.Api.Entities;
using RabbitMQOrdering.Api.Services.IServices;
using RabbitMQOrdering.Api.Shared;

namespace RabbitMQOrdering.Api.Services
{
    public class ProductQueueService : IProductQueueService
    {
        public IServiceProvider _serviceScopefactory;
        public ProductQueueService(IServiceProvider _serviceScopefactory)
        {
            this._serviceScopefactory = _serviceScopefactory;
        }

        public async Task<Result<ProductQueue>> UpdateToDone(ProductQueue productQueue)
        {
            try{ 

                _serviceScopefactory = productQueue._serviceScopefactory;
                
                using (var scope = _serviceScopefactory.CreateScope())
                {
                    var _productOrderService = scope.ServiceProvider.GetService<IProductOrderService>();
                    
            
                    var result = await _productOrderService.GetByOrderAndProductId( productQueue.Id,productQueue.OrderId, productQueue.ProductId);
                    if(result)
                    {
                        var updatedDate = DateTime.Now;
                        result.Value.ProductQueueStatus = ProductOrderStatus.Done;
                        result.Value.UpdatedAt = updatedDate;

                        var updateProductOrder = await _productOrderService.Update(result.Value);

                        if(updateProductOrder.IsSuccess)
                        {
                            
                            return Result.Ok(productQueue);
                        }

                        return Result.Fail<ProductQueue>("Error while updating ProductOrder entity", ResultCode.BadRequest);
                    }

                    return Result.Fail<ProductQueue>("No ProductOrder entity was found with given ID", ResultCode.BadRequest);
                }
            }
            catch(Exception ex)
            {
                return Result.Fail<ProductQueue>(ex.Message, ResultCode.InternalServerError);
            }
        }
        

        public async Task<Result<ProductQueue>> UpdateToPreparing(ProductQueue productQueue)
        {
            _serviceScopefactory = productQueue._serviceScopefactory;
            var result = await this.UpdateToStatus(ProductOrderStatus.Preparing, OrderStatusEnum.Preparing, productQueue);

            return result;
        }


        public async Task<Result<ProductQueue>> UpdateToStatus(ProductOrderStatus productOrderStatus, OrderStatusEnum orderStatus, ProductQueue productQueue)
        {
            try{ 
                
                using (var scope = _serviceScopefactory.CreateScope())
                {
                    var _orderservice = scope.ServiceProvider.GetService<IOrderService>();
                    var _productOrderService = scope.ServiceProvider.GetService<IProductOrderService>();
                    
               
                    var result = await _productOrderService.GetByOrderAndProductId(productQueue.Id, productQueue.OrderId, productQueue.ProductId);
                    if(result)
                    {
                        var updatedDate = DateTime.Now;
                        result.Value.ProductQueueStatus = productOrderStatus;
                        result.Value.UpdatedAt = updatedDate;

                        var updateProductOrder = await _productOrderService.Update(result.Value);

                        if(updateProductOrder.IsSuccess)
                        {
                            var order = await _orderservice.GetById(productQueue.OrderId);
                            if(order && order.Value.OrderStatus != OrderStatusEnum.Preparing)
                            {
                                order.Value.OrderStatus = orderStatus;
                                order.Value.UpdatedAt = updatedDate;

                                var updateOrder = await _orderservice.Update(order.Value);

                                if(updateOrder.IsSuccess)
                                    return Result.Ok(productQueue);

                                return Result.Fail<ProductQueue>("Error while updating order entity", ResultCode.BadRequest);
                            }

                            return Result.Fail<ProductQueue>("No order was found with given ID", ResultCode.BadRequest);
                        }

                        return Result.Fail<ProductQueue>("Error while updating ProductOrder entity", ResultCode.BadRequest);
                    }

                    return Result.Fail<ProductQueue>("No ProductOrder entity was found with given ID", ResultCode.BadRequest);
                }
            }
            catch(Exception ex)
            {
                return Result.Fail<ProductQueue>(ex.Message, ResultCode.InternalServerError);
            }
        }
    }

    public interface IProductQueueService
    {
        Task<Result<ProductQueue>> UpdateToDone(ProductQueue productQueue);
        Task<Result<ProductQueue>> UpdateToPreparing(ProductQueue productQueue);
        Task<Result<ProductQueue>> UpdateToStatus(ProductOrderStatus productOrderStatus, OrderStatusEnum orderStatus, ProductQueue productQueue);
    }
}