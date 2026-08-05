namespace ShippingApi.Dtos.Order
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ICollection<OrderItemResponse> Items { get; set; } = new List<OrderItemResponse>();
        public decimal TotalAmount { get; set; }
    }
}
