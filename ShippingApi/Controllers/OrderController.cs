using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingApi.Dtos.Mapping;
using ShippingApi.Dtos.Order;
using ShippingApi.Services;

namespace ShippingApi.Controllers
{
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrders(CancellationToken cancellationToken)
        {
            var orders = await _orderService.GetOrdersAsync(cancellationToken);
            return Ok(orders.Select(o => o.ToResponse()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponse>> GetOrder(int id, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderByIdAsync(id, cancellationToken);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order.ToResponse());
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponse>> CreateOrder(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var order = request.ToModel();
            await _orderService.AddOrderAsync(order, cancellationToken);

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order.ToResponse());
        }

        [HttpPut("{id}")]
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

        [HttpDelete("{id}")]
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

