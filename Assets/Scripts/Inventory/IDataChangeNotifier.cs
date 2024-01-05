using System;

namespace Inventory
{
    public interface IDataChangeNotifier
    {
        event Action<ItemData> ItemDataChanged;
    }
}