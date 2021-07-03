using System;
using System.Collections.Generic;
using Catalog.Entities;

namespace Catalog.Repositories
{
    public interface IItemRepository
    {
        Item GetItem(Guid id);
        IEnumerable<Item> GetItems();
        void CreteItem(Item item);
        void UpdateItem(Item item);
        void DeleteItem(Guid id);
    }
}