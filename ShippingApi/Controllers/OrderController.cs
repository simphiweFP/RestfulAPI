using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingApi.Dtos.Common;
using ShippingApi.Dtos.Mapping;
using ShippingApi.Dtos.Order;
using ShippingApi.Services;

namespace ShippingApi.Controllers
{
    /// <summary>
    /// Manages order records.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>Gets paginated orders with optional filters.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<OrderResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResponse<OrderResponse>>> GetOrders([FromQuery] OrderQueryParameters queryParameters, CancellationToken cancellationToken)
        {
            var pagedOrders = await _orderService.GetOrdersAsync(queryParameters, cancellationToken);

            var response = new PagedResponse<OrderResponse>
            {
                Items = pagedOrders.Items.Select(o => o.ToResponse()).ToList(),
                PageNumber = pagedOrders.PageNumber,
                PageSize = pagedOrders.PageSize,
                TotalCount = pagedOrders.TotalCount,
                TotalPages = pagedOrders.TotalPages
            };

            return Ok(response);
        }

        /// <summary>Gets an order by id.</summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderResponse>> GetOrder(int id, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderByIdAsync(id, cancellationToken);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order.ToResponse());
        }

        /// <summary>Creates a new order.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
        public async Task<ActionResult<OrderResponse>> CreateOrder(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var order = request.ToModel();
            await _orderService.AddOrderAsync(order, cancellationToken);

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order.ToResponse());
        }

        /// <summary>Updates an existing order.</summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrder(int id, UpdateOrderRequest request, CancellationToken cancellationToken)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            var existingOrder = await _orderService.GetOrderByIdAsync(id, cancellationToken);
            if (existingOrder == null)
            {
                return NotFound();
            }

            var order = request.ToModel();
            await _orderService.UpdateOrderAsync(order, cancellationToken);
            return NoContent();
        }

        /// <summary>Deletes an order.</summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(int id, CancellationToken cancellationToken)
        {
            var existingOrder = await _orderService.GetOrderByIdAsync(id, cancellationToken);
            if (existingOrder == null)
            {
                return NotFound();
            }

            await _orderService.DeleteOrderAsync(id, cancellationToken);
            return NoContent();
        }
    }
}

