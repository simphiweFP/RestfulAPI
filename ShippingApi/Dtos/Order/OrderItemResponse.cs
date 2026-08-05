namespace ShippingApi.Dtos.Order
{
    public class OrderItemResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
