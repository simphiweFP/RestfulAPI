using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using ShippingApi.Dtos.Address;
using ShippingApi.Dtos.Order;
using Xunit;

namespace ShippingApi.UnitTests.Validation;

public class RequestValidationTests
{
    [Fact]
    public void OrderQueryParameters_WhenMinimumExceedsMaximum_IsInvalid()
    {
        var parameters = new OrderQueryParameters
        {
            MinTotalAmount = 100,
            MaxTotalAmount = 10
        };

        var results = Validate(parameters);

        results.Should().Contain(result => result.ErrorMessage!.Contains("less than or equal"));
    }

    [Fact]
    public void OrderQueryParameters_WhenPageSizeExceedsMaximum_IsInvalid()
    {
        var parameters = new OrderQueryParameters { PageSize = 101 };

        var results = Validate(parameters);

        results.Should().Contain(result => result.MemberNames.Contains(nameof(OrderQueryParameters.PageSize)));
    }

    [Fact]
    public void UpdateAddressRequest_WhenIdIsZero_IsInvalid()
    {
        var request = new UpdateAddressRequest
        {
            Id = 0,
            Street = "Main Street",
            City = "Cape Town",
            ZipCode = "8001"
        };

        var results = Validate(request);

        results.Should().Contain(result => result.MemberNames.Contains(nameof(UpdateAddressRequest.Id)));
    }

    private static IReadOnlyCollection<ValidationResult> Validate(object instance)
    {
        var context = new ValidationContext(instance);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(instance, context, results, validateAllProperties: true);
        return results;
    }
}
