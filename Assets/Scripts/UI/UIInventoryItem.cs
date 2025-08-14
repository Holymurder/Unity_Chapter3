using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
{
    [SerializeField]
    private Image _itemImage;

    [SerializeField]
    private TMP_Text _quantityText;

    [SerializeField]
    private Image _borderImage;

    public event Action<UIInventoryItem> ItemClicked;
    public event Action<UIInventoryItem> ItemDroppedOn;
    public event Action<UIInventoryItem> ItemBeginDrag;
    public event Action<UIInventoryItem> ItemEndDrag;
    public event Action<UIInventoryItem> RightMouseButtonClicked;

    private bool _isEmpty = true;

    private void Awake()
    {
        ResetData();
        Deselect();
    }

    public void ResetData()
    {
        _itemImage.gameObject.SetActive(false);
        _isEmpty = true;
    }

    public void Deselect()
    {
        _borderImage.enabled = false;
    }

    public void SetData(Sprite sprite, int quantity)
    {
        _itemImage.gameObject.SetActive(true);
        _itemImage.sprite = sprite;
        _quantityText.text = quantity.ToString();
        _isEmpty = false;
    }

    public void Select()
    {
        _borderImage.enabled = true;
    }

    public void OnPointerClick(PointerEventData pointerData)
    {
        if (pointerData.button == PointerEventData.InputButton.Right)
        {
            RightMouseButtonClicked?.Invoke(this);
        }
        else
        {
            ItemClicked?.Invoke(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isEmpty)
        {
            return;
        }
        ItemBeginDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ItemEndDrag?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemDroppedOn?.Invoke(this);
    }
}
