using System.Collections;
using System.Collections.Generic;
using Inventory;
using UnityEngine;
using VContainer;

public class TestController : MonoBehaviour
{
    [Inject]
    private InventoryDragItem _test;

    [Inject]
    private InventoryController _inventoryController;
    
    void Start()
    {
        Debug.Log($"Injected for test: {_test}, {_inventoryController}");
    }
}
