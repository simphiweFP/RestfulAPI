using System.ComponentModel.DataAnnotations;

namespace ShippingApi.Dtos.Address
{
    public class CreateAddressRequest
    {
        [Required]
        [StringLength(200)]
        public string Street { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string ZipCode { get; set; } = string.Empty;
    }
}
