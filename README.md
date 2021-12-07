# Bussy Event Bus /w RabbitMQ
An ASP.NET 5 event bus implementation with RabbitMQ. Code heavily inspired from:
- [.NET Microservices Sample Reference Application](https://github.com/dotnet-architecture/eShopOnContainers)
- [Wrapt](https://wrapt.dev/)

# How to Make It Work
Just update the `application.json` file to point a valid queue and start the application. Since it doesn't have any dependencies, I ignored the Dockerfile, but feel free to add one.

Also, the implementation assumes that you publish to a topic exchange. For further details, you can check the RabbitMQ [documents](https://www.rabbitmq.com/tutorials/amqp-concepts.html).
