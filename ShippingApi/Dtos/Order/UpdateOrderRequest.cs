using System.ComponentModel.DataAnnotations;

namespace ShippingApi.Dtos.Order
{
    public class UpdateOrderRequest
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<OrderItemRequest> Items { get; set; } = new List<OrderItemRequest>();
    }
}
