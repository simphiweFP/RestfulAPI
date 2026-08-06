namespace ShippingApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ICollection<Item> Items { get; set; } = new List<Item>();
        public decimal TotalAmount { get; set; }
    }
}
