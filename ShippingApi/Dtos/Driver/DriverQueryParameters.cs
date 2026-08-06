using System.ComponentModel.DataAnnotations;

namespace ShippingApi.Dtos.Driver
{
    public class DriverQueryParameters
    {
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 10;

        [StringLength(100)]
        public string? Team { get; set; }

        [StringLength(150)]
        public string? Search { get; set; }
    }
}
