using ShippingApi.Dtos.Address;

namespace ShippingApi.Dtos.Driver
{
    public class DriverResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int DriverNumber { get; set; }
        public string Team { get; set; } = string.Empty;
        public AddressResponse? Address { get; set; }
    }
}
