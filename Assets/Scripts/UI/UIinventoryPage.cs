using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryPage : MonoBehaviour
{
    [SerializeField]
    private UIInventoryItem _itemPrefab;

    [SerializeField]
    private RectTransform _contentPanel;

    [SerializeField]
    private UIInventoryDescription _itemDescription;

    [SerializeField]
    private MouseFollower _mouseFollower;

    private List<UIInventoryItem> _uiItems = new List<UIInventoryItem>();

    private int _currentlyDraggedItemIndex = -1;

    public event Action<int> DescriptionRequested;
    public event Action<int> StartDragging;
    public event Action<int, int> ItemsSwapped;

    private void Awake()
    {
        Hide();
        _mouseFollower.Toggle(false);
        _itemDescription.ResetDescription();
    }

    public void InitializeInventoryUI(int inventorySize) 
    {
        for (int i = 0; i < inventorySize; i++)
        {
            UIInventoryItem uiItem = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(_contentPanel);
            _uiItems.Add(uiItem);

            uiItem.ItemClicked += HandleItemSelection;
            uiItem.ItemBeginDrag += HandleBeginDrag;
            uiItem.ItemDroppedOn += HandleSwap;
            uiItem.ItemEndDrag += HandleEndDrag;
        }
    }

    public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity) 
    {
        if (_uiItems.Count > itemIndex)
        {
            _uiItems[itemIndex].SetData(itemImage, itemQuantity);
        }
    }

    private void HandleEndDrag(UIInventoryItem inventoryItemUI)
    {
        ResetDraggedItem();
    }

    private void HandleSwap(UIInventoryItem inventoryItemUI)
    {
        int index = _uiItems.IndexOf(inventoryItemUI);
        if (index == -1)
        {
            return;
        }
        ItemsSwapped?.Invoke(_currentlyDraggedItemIndex, index);
        HandleItemSelection(inventoryItemUI);
    }

    private void ResetDraggedItem()
    {
        _mouseFollower.Toggle(false);
        _currentlyDraggedItemIndex = -1;
    }

    private void HandleBeginDrag(UIInventoryItem inventoryItemUI)
    {
        int index = _uiItems.IndexOf(inventoryItemUI);
        if (index == -1) 
        {
            return;
        }
        _currentlyDraggedItemIndex = index;
        HandleItemSelection(inventoryItemUI);
        StartDragging?.Invoke(index);
    }

    public void CreateDraggedItem(Sprite sprite, int quantity) 
    {
        _mouseFollower.Toggle(true);
        _mouseFollower.SetData(sprite, quantity);
    }

    private void HandleItemSelection(UIInventoryItem inventoryItemUI)
    {
        int index = _uiItems.IndexOf(inventoryItemUI);
        if (index == -1) 
        {
            return;
        }
        DescriptionRequested?.Invoke(index);
    }

    public void Show() 
    {
        gameObject.SetActive(true);
        ResetSelection();
    }

    public void ResetSelection()
    {
        _itemDescription.ResetDescription();
        DeselectAllItems();
    }

    private void DeselectAllItems()
    {
        foreach (UIInventoryItem item in _uiItems)
        {
            item.Deselect();
        }
    }

    public void Hide() 
    {
        gameObject.SetActive(false);
        ResetDraggedItem();
    }

    internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
    {
        _itemDescription.SetDescription(itemImage, name, description);
        DeselectAllItems();
        _uiItems[itemIndex].Select();
    }

    internal void ResetAllItems()
    {
        foreach (var item in _uiItems)
        {
            item.ResetData();
            item.Deselect();
        }
    }
}
