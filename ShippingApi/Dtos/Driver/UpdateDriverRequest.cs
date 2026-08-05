using System.ComponentModel.DataAnnotations;
using ShippingApi.Dtos.Address;

namespace ShippingApi.Dtos.Driver
{
    public class UpdateDriverRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int DriverNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string Team { get; set; } = string.Empty;

        public UpdateAddressRequest? Address { get; set; }
    }
}
