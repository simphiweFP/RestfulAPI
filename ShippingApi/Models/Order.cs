namespace ShippingApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public IEnumerable<Item> Items { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
