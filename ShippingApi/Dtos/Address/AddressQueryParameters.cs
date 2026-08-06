using System.ComponentModel.DataAnnotations;

namespace ShippingApi.Dtos.Address
{
    public class AddressQueryParameters
    {
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 10;

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(200)]
        public string? Search { get; set; }
    }
}
