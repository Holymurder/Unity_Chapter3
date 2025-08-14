using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private UIInventoryPage _inventoryUI;

    [SerializeField]
    private InventorySO _inventoryData;

    public int InventorySize = 10;

    public List<InventoryItem> InitialItems = new List<InventoryItem>();

    private void Start()
    {
        InitializeUI();
        InitializeInventoryData();
    }

    private void InitializeInventoryData()
    {
        _inventoryData.Initialize();
        _inventoryData.InventoryChanged += UpdateInventoryUI;

        foreach (InventoryItem item in InitialItems)
        {
            if (item.IsEmpty)
            {
                continue;
            }
            _inventoryData.AddItem(item);
        }
    }

    private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
    {
        _inventoryUI.ResetAllItems();
        foreach (var item in inventoryState)
        {
            _inventoryUI.UpdateData(item.Key, item.Value.Item.ItemImage, item.Value.Quantity);
        }
    }

    private void InitializeUI()
    {
        _inventoryUI.InitializeInventoryUI(_inventoryData.Size);
        _inventoryUI.DescriptionRequested += HandleDescriptionRequest;
        _inventoryUI.ItemsSwapped += HandleSwapItems;
        _inventoryUI.StartDragging += HandleDragging;
    }

    private void HandleDragging(int itemIndex)
    {
        InventoryItem inventoryItem = _inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
        {
            return;
        }
        _inventoryUI.CreateDraggedItem(inventoryItem.Item.ItemImage, inventoryItem.Quantity);
    }

    private void HandleSwapItems(int firstIndex, int secondIndex)
    {
        _inventoryData.SwapItems(firstIndex, secondIndex);
    }

    private void HandleDescriptionRequest(int itemIndex)
    {
        InventoryItem inventoryItem = _inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
        {
            _inventoryUI.ResetSelection();
            return;
        }

        ItemSO item = inventoryItem.Item;
        _inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.name, item.Description);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!_inventoryUI.isActiveAndEnabled)
            {
                _inventoryUI.Show();
                foreach (var item in _inventoryData.GetCurrentInventoryState())
                {
                    _inventoryUI.UpdateData(item.Key, item.Value.Item.ItemImage, item.Value.Quantity);
                }
            }
            else
            {
                _inventoryUI.Hide();
            }
        }
    }
}
