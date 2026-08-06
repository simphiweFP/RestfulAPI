using System.ComponentModel.DataAnnotations;

namespace ShippingApi.Dtos.Order
{
    public class CreateOrderRequest
    {
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<OrderItemRequest> Items { get; set; } = new List<OrderItemRequest>();
    }
}
