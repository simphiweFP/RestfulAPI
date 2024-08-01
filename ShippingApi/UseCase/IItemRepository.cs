using ShippingApi.Models;

namespace ShippingApi.UseCase
{
    public interface IItemRepository
    {
        IEnumerable<Item> GetItems();
        Item GetItemById(int itemId);
        void AddItem(Item item);
        void UpdateItem(Item item);
        void DeleteItem(int itemId);
    }
}
