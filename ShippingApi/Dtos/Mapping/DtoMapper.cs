using AddressModel = ShippingApi.Models.Address;
using DriverModel = ShippingApi.Models.Driver;
using OrderModel = ShippingApi.Models.Order;
using ItemModel = ShippingApi.Models.Item;
using AddressDto = ShippingApi.Dtos.Address;
using DriverDto = ShippingApi.Dtos.Driver;
using OrderDto = ShippingApi.Dtos.Order;

namespace ShippingApi.Dtos.Mapping
{
    public static class DtoMapper
    {
        public static AddressDto.AddressResponse ToResponse(this AddressModel address)
        {
            return new AddressDto.AddressResponse
            {
                Id = address.Id,
                Street = address.Street,
                City = address.City,
                ZipCode = address.ZipCode
            };
        }

        public static AddressModel ToModel(this AddressDto.CreateAddressRequest request)
        {
            return new AddressModel
            {
                Street = request.Street,
                City = request.City,
                ZipCode = request.ZipCode
            };
        }

        public static AddressModel ToModel(this AddressDto.UpdateAddressRequest request)
        {
            return new AddressModel
            {
                Id = request.Id,
                Street = request.Street,
                City = request.City,
                ZipCode = request.ZipCode
            };
        }

        public static DriverDto.DriverResponse ToResponse(this DriverModel driver)
        {
            return new DriverDto.DriverResponse
            {
                Id = driver.Id,
                Name = driver.Name,
                Email = driver.Email,
                DriverNumber = driver.DriverNumber,
                Team = driver.Team,
                Address = driver.Address?.ToResponse()
            };
        }

        public static DriverModel ToModel(this DriverDto.CreateDriverRequest request)
        {
            return new DriverModel
            {
                Name = request.Name,
                Email = request.Email,
                DriverNumber = request.DriverNumber,
                Team = request.Team,
                Address = request.Address?.ToModel()
            };
        }

        public static DriverModel ToModel(this DriverDto.UpdateDriverRequest request)
        {
            return new DriverModel
            {
                Id = request.Id,
                Name = request.Name,
                Email = request.Email,
                DriverNumber = request.DriverNumber,
                Team = request.Team,
                Address = request.Address?.ToModel()
            };
        }

        public static OrderDto.OrderItemResponse ToResponse(this ItemModel item)
        {
            return new OrderDto.OrderItemResponse
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price
            };
        }

        public static ItemModel ToModel(this OrderDto.OrderItemRequest request)
        {
            return new ItemModel
            {
                Name = request.Name,
                Price = request.Price
            };
        }

        public static OrderDto.OrderResponse ToResponse(this OrderModel order)
        {
            return new OrderDto.OrderResponse
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalAmount = order.TotalAmount,
                Items = order.Items?.Select(ToResponse).ToList() ?? new List<OrderDto.OrderItemResponse>()
            };
        }

        public static OrderModel ToModel(this OrderDto.CreateOrderRequest request)
        {
            return new OrderModel
            {
                UserId = request.UserId,
                Items = request.Items.Select(ToModel).ToList(),
                TotalAmount = request.Items.Sum(i => i.Price)
            };
        }

        public static OrderModel ToModel(this OrderDto.UpdateOrderRequest request)
        {
            return new OrderModel
            {
                Id = request.Id,
                UserId = request.UserId,
                Items = request.Items.Select(ToModel).ToList(),
                TotalAmount = request.Items.Sum(i => i.Price)
            };
        }
    }
}
