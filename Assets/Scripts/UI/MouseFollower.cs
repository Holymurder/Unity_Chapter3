using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;

    [SerializeField]
    private Camera _mainCamera;

    [SerializeField]
    private UIInventoryItem _item;

    private void Awake()
    {
        _canvas = transform.root.GetComponent<Canvas>();
        _mainCamera = Camera.main;
        _item = GetComponentInChildren<UIInventoryItem>();
    }

    public void SetData(Sprite sprite, int quantity)
    {
        _item.SetData(sprite, quantity);
    }

    private void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)_canvas.transform,
            Input.mousePosition,
            _canvas.worldCamera,
            out position
        );

        transform.position = _canvas.transform.TransformPoint(position);
    }

    public void Toggle(bool isVisible)
    {
        Debug.Log($"Item toggled {isVisible}");
        gameObject.SetActive(isVisible);
    }
}
