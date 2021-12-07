using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Bussy.Server.Domain.Orders.Features;
using Bussy.Server.IntegrationEvents.Events;
using Bussy.Server.Messaging.EventBus;
using Bussy.Server.Models.Order;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bussy.Server.Controllers.v1
{
    [ApiController]
    [Route("api/orders")]
    [ApiVersion("1.0")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IEventBus _eventBus;

        public OrdersController(IMediator mediator, IEventBus eventBus)
        {
            _mediator = mediator;
            _eventBus = eventBus;
        }
        
        /// <summary>
        /// Creates a new Order record.
        /// </summary>
        /// <response code="201">Order created.</response>
        /// <response code="400">Order has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the Order.</response>
        [ProducesResponseType(typeof(OrderModel), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost(Name = "AddOrder")]
        public async Task<ActionResult<OrderModel>> AddOrder([FromBody]OrderModelForCreation orderForCreation)
        {
            var command = new AddOrder.AddOrderCommand(orderForCreation);
            var commandResponse = await _mediator.Send(command);

            _eventBus.Publish(new OrderCreatedIntegrationEvent(
                commandResponse.Id,
                commandResponse.Account,
                commandResponse.Symbol,
                commandResponse.Price,
                commandResponse.Size
            ));

            return CreatedAtRoute("GetOrder",
                new { commandResponse.Id },
                commandResponse);
        }
        
        /// <summary>
        /// Gets a single Order by ID.
        /// </summary>
        /// <response code="200">Order record returned successfully.</response>
        /// <response code="400">Order has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the Order.</response>
        [ProducesResponseType(typeof(OrderModel), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpGet("{id:guid}", Name = "GetOrder")]
        public async Task<ActionResult<OrderModel>> GetOrder(Guid id)
        {
            var query = new GetOrder.OrderQuery(id);
            var queryResponse = await _mediator.Send(query);

            return Ok(queryResponse);
        }

        /// <summary>
        /// Gets a list of all Orders.
        /// </summary>
        /// <response code="200">Order list returned successfully.</response>
        /// <response code="400">Order has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the Order.</response>
        /// <remarks>
        /// Requests can be narrowed down with a variety of query string values:
        /// ## Query String Parameters
        /// - **PageNumber**: An integer value that designates the page of records that should be returned.
        /// - **PageSize**: An integer value that designates the number of records returned on the given page that you would like to return. This value is capped by the internal MaxPageSize.
        /// - **SortOrder**: A comma delimited ordered list of property names to sort by. Adding a `-` before the name switches to sorting descendingly.
        /// - **Filters**: A comma delimited list of fields to filter by formatted as `{Name}{Operator}{Value}` where
        ///     - {Name} is the name of a filterable property. You can also have multiple names (for OR logic) by enclosing them in brackets and using a pipe delimiter, eg. `(LikeCount|CommentCount)>10` asks if LikeCount or CommentCount is >10
        ///     - {Operator} is one of the Operators below
        ///     - {Value} is the value to use for filtering. You can also have multiple values (for OR logic) by using a pipe delimiter, eg.`Title@= new|hot` will return posts with titles that contain the text "new" or "hot"
        ///
        ///    | Operator | Meaning                       | Operator  | Meaning                                      |
        ///    | -------- | ----------------------------- | --------- | -------------------------------------------- |
        ///    | `==`     | Equals                        |  `!@=`    | Does not Contains                            |
        ///    | `!=`     | Not equals                    |  `!_=`    | Does not Starts with                         |
        ///    | `>`      | Greater than                  |  `@=*`    | Case-insensitive string Contains             |
        ///    | `&lt;`   | Less than                     |  `_=*`    | Case-insensitive string Starts with          |
        ///    | `>=`     | Greater than or equal to      |  `==*`    | Case-insensitive string Equals               |
        ///    | `&lt;=`  | Less than or equal to         |  `!=*`    | Case-insensitive string Not equals           |
        ///    | `@=`     | Contains                      |  `!@=*`   | Case-insensitive string does not Contains    |
        ///    | `_=`     | Starts with                   |  `!_=*`   | Case-insensitive string does not Starts with |
        /// </remarks>
        [ProducesResponseType(typeof(IEnumerable<OrderModel>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpGet(Name = "GetOrders")]
        public async Task<IActionResult> GetOrders([FromQuery] OrderParametersModel orderParametersModel)
        {
            var query = new GetOrderList.OrderListQuery(orderParametersModel);
            var queryResponse = await _mediator.Send(query);

            var paginationMetadata = new
            {
                totalCount = queryResponse.TotalCount,
                pageSize = queryResponse.PageSize,
                currentPageSize = queryResponse.CurrentPageSize,
                currentStartIndex = queryResponse.CurrentStartIndex,
                currentEndIndex = queryResponse.CurrentEndIndex,
                pageNumber = queryResponse.PageNumber,
                totalPages = queryResponse.TotalPages,
                hasPrevious = queryResponse.HasPrevious,
                hasNext = queryResponse.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            return Ok(queryResponse);
        }
        
        /// <summary>
        /// Updates an entire existing Order.
        /// </summary>
        /// <response code="204">Order updated.</response>
        /// <response code="400">Order has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the Order.</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpPut("{id:guid}", Name = "UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(Guid id, OrderModelForUpdate orderModelForUpdate)
        {
            var query = new GetOrder.OrderQuery(id);
            var oldOrderToBeUpdated = await _mediator.Send(query);
            
            var command = new UpdateOrder.UpdateOrderCommand(id, orderModelForUpdate);
            await _mediator.Send(command);
            
            _eventBus.Publish(new OrderModifiedIntegrationEvent(
                id,
                oldOrderToBeUpdated.Account,
                oldOrderToBeUpdated.Symbol,
                oldOrderToBeUpdated.Price,
                oldOrderToBeUpdated.Size,
                orderModelForUpdate.Account,
                orderModelForUpdate.Symbol,
                orderModelForUpdate.Price,
                orderModelForUpdate.Size,
                false));

            return NoContent();
        }


        /// <summary>
        /// Deletes an existing Order record.
        /// </summary>
        /// <response code="204">Order deleted.</response>
        /// <response code="400">Order has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the Order.</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpDelete("{id:guid}", Name = "DeleteOrder")]
        public async Task<ActionResult> DeleteOrder(Guid id)
        {
            var command = new DeleteOrder.DeleteOrderCommand(id);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}