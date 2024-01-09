using Inventory;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DependencyInjection
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private InventoryDragItem inventoryDragItemPrefab;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab(inventoryDragItemPrefab, Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<InventoryController>();
        }
    }
}