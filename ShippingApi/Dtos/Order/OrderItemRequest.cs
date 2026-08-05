using System.ComponentModel.DataAnnotations;

namespace ShippingApi.Dtos.Order
{
    public class OrderItemRequest
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
