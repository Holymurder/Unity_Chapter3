using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventorySO : ScriptableObject
{
    [SerializeField]
    private List<InventoryItem> _inventoryItems;

    [field: SerializeField]
    public int Size { get; private set; } = 10;

    public event Action<Dictionary<int, InventoryItem>> InventoryChanged;

    public void Initialize()
    {
        _inventoryItems = new List<InventoryItem>();
        for (int i = 0; i < Size; i++)
        {
            _inventoryItems.Add(InventoryItem.GetEmptyItem());
        }
    }

    public void AddItem(ItemSO item, int quantity)
    {
        for (int i = 0; i < _inventoryItems.Count; i++)
        {
            if (_inventoryItems[i].IsEmpty)
            {
                _inventoryItems[i] = new InventoryItem
                {
                    Item = item,
                    Quantity = quantity
                };
                return;
            }
        }
    }

    public Dictionary<int, InventoryItem> GetCurrentInventoryState()
    {
        var inventoryState = new Dictionary<int, InventoryItem>();
        for (int i = 0; i < _inventoryItems.Count; i++)
        {
            if (_inventoryItems[i].IsEmpty)
            {
                continue;
            }
            inventoryState[i] = _inventoryItems[i];
        }
        return inventoryState;
    }

    public InventoryItem GetItemAt(int itemIndex)
    {
        return _inventoryItems[itemIndex];
    }

    public void AddItem(InventoryItem inventoryItem)
    {
        AddItem(inventoryItem.Item, inventoryItem.Quantity);
    }

    public void SwapItems(int firstIndex, int secondIndex)
    {
        InventoryItem tempItem = _inventoryItems[firstIndex];
        _inventoryItems[firstIndex] = _inventoryItems[secondIndex];
        _inventoryItems[secondIndex] = tempItem;
        NotifyInventoryChanged();
    }

    private void NotifyInventoryChanged()
    {
        InventoryChanged?.Invoke(GetCurrentInventoryState());
    }
}

[Serializable]
public struct InventoryItem
{
    public int Quantity;
    public ItemSO Item;

    public bool IsEmpty => Item == null;

    public InventoryItem ChangeQuantity(int newQuantity)
    {
        return new InventoryItem
        {
            Item = this.Item,
            Quantity = newQuantity
        };
    }

    public static InventoryItem GetEmptyItem() => new InventoryItem
    {
        Item = null,
        Quantity = 0
    };
}
