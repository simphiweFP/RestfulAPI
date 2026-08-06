using System.ComponentModel.DataAnnotations;

namespace ShippingApi.Dtos.Order
{
    public class OrderQueryParameters : IValidatableObject
    {
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 10;

        [Range(1, int.MaxValue)]
        public int? UserId { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal? MinTotalAmount { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal? MaxTotalAmount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MinTotalAmount.HasValue && MaxTotalAmount.HasValue && MinTotalAmount > MaxTotalAmount)
            {
                yield return new ValidationResult(
                    "MinTotalAmount must be less than or equal to MaxTotalAmount.",
                    new[] { nameof(MinTotalAmount), nameof(MaxTotalAmount) });
            }
        }
    }
}