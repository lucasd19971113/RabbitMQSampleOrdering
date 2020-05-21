# RabbitMQSampleOrdering Test

Task: "Assume that you have a restaurant with multiple POS (point-of-sale) instances sending orders that should be routed to specific areas of a kitchen.

It must comprise an HTTP Server with an endpoint to receive an Order and place it in a queue representing a destination kitchen area".



## Dependencies

.Net Core SDK - 3.0.102

RabbitMQ 

## Restoring Dependencies

 After downloading the .Net Core SDK and RabbitMQ Broker locally, go to the root folder of each main project (./RabbitMQOrdering.Api and ./RabbitMQOrdering.Queue.Api) and execute the following commands to restore each project dependencies:



```bash
dotnet restore
```


## Start Projects

Before running the RabbitMQOrdering.Api and RabbitMQOrdering.Queue.Api projects, make sure your RabbitMQ service is already started.

Also, as the RabbitMQOrdering.Api is the only application which can access the In Memory Database, every time the RabbitMQOrdering.Api restarts for any reason, you should clean all the Queues in the RabbitMQ broker.

To start the RabbitMQOrdering.Queue.Api and RabbitMQOrdering.Api projects, open each one of them in separate instances of your preferred IDE and execute the following commands in the terminal, in the root directory:

```bash
dotnet watch run
```
Or:
```bash
dotnet run
```

Or you can just select the "Run" option of your IDE. 

After compilation, go to your browser and navigate to informed network address inside the terminal.


## Testing Projects

1. Start RabbitMQ broker and both applications (make sure both applications are configured with your RabbitMQ settings in the appsettings.json file);
2. Use Postman, for example, to make API calls to the server;
3. As the RabbitMQOrdering.Queue.Api starts, you should see 9 Queues in the RabbitMQ management port, 8 of representing each Kitchen Area and status (waiting and preparing), and one Queue of Commands, which is responsible to send commands to RabbitMQOrdering.Api execute in the Database;
4. Place Order using POST method in the '/api/order/placeorder' endpoint (RabbitMQOrdering.Api);
5. Check if the queue was filled;
6. Process your order products from Waiting to Preparing Status using /api/queue/Process+{kitchenArea.Name}+Waiting (RabbitMQOrdering.Queue.Api);
7. Process your order products from Preparing to Done Status using /api/queue/Process+{kitchenArea.Name}+Preparing (RabbitMQOrdering.Queue.Api);
8. You can always check all updates by calling '/api/order/getall' (RabbitMQOrdering.Api);
9. Check Queue items by calling, for example, 'api/queue/ShowQueueItem/Side Dish/waiting' GET method (RabbitMQOrdering.QueueApi);
10. When all order products are in 'Done Status', the Order automatically goes to 'Done';

## Order Json Example


```json
{
	"productOrder" : [
		{"productId":1, "price":10.00},
		{"productId":1, "price":10.00},
		{"productId":1, "price":10.00},
		{"productId":1, "price":10.00},
		{"productId":2, "price":13.50},
		{"productId":3, "price":8.00},
		{"productId":4, "price":3.50}
	],
	"pointOfSaleId": 1
}
```

## Products

```json
{
	"products" : [
		{"id":1, "name":"French Fries", "price":10.00, "kitchenAreaId":3},
		{"id":2, "name":"Cheese burger", "price":13.50, "kitchenAreaId":1},
		{"id":3, "name":"Large Coke", "price":8.00, "kitchenAreaId":2},
		{"id":4, "name":"Ice cream", "price":3.50, "kitchenAreaId":4}
	]
}
```


## Points Of Sale

Id = 1, Cashier

Id = 2, Self-Service Totem

## Kitchen Areas

Id = 1, Meal

Id = 2, Beverage

Id = 3, Side Dish

Id = 4, Desert

## Order Status Enum

Waiting = 1,
Preparing = 2,
Done = 3,
Delivered = 4,
Cancelled = 5

## Order Product Status Enum

Waiting = 1,
Preparing = 2,
Done = 3


