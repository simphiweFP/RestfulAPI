using ShippingApi.Models;

namespace ShippingApi.UseCase
{
    public interface IUserRepository
    {
        User GetUserById(int userId);
        void AddUser(User user);
        void UpdateUser(User user);
    }
}
