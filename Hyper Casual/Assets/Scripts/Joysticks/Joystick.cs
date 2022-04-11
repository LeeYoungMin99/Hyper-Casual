using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform _background = null;
    [SerializeField] private RectTransform _handle = null;

    [SerializeField] private bool _snapX = false;
    [SerializeField] private bool _snapY = false;

    private enum EAxisOptions { Both, Horizontal, Vertical }

    private Canvas _canvas;
    private RectTransform _canvasRectTransform;
    private RectTransform _rectTransform;
    private Vector2 input;

    public float Horizontal { get { return (_snapX) ? SnapFloat(input.x, EAxisOptions.Horizontal) : input.x; } }
    public float Vertical { get { return (_snapY) ? SnapFloat(input.y, EAxisOptions.Vertical) : input.y; } }

    private const float HANDLE_RANGE = 1f;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _canvasRectTransform = _canvas.GetComponent<RectTransform>();
        input = Utils.ZERO_VECTOR2;

        Vector2 center = new Vector2(0.5f, 0.5f);
        _background.pivot = center;
        _handle.anchorMin = center;
        _handle.anchorMax = center;
        _handle.pivot = center;
        _handle.anchoredPosition = Utils.ZERO_VECTOR2;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        float x = _canvasRectTransform.rect.width / Screen.width;
        float y = _canvasRectTransform.rect.height / Screen.height;

        Vector2 newPosition = new Vector2(eventData.position.x * x, eventData.position.y * y);

        _background.anchoredPosition = ScreenPointToAnchoredPosition(newPosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position = RectTransformUtility.WorldToScreenPoint(null, _background.position);
        Vector2 radius = _background.sizeDelta / 2;
        input = (eventData.position - position) / (radius * _canvas.scaleFactor);

        if (input.magnitude > 1)
        {
            input = input.normalized;
        }

        _handle.anchoredPosition = input * radius * HANDLE_RANGE;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        input = Utils.ZERO_VECTOR2;
        _handle.anchoredPosition = Utils.ZERO_VECTOR2;
        _background.anchoredPosition = Utils.ZERO_VECTOR2;
    }

    private float SnapFloat(float value, EAxisOptions snapAxis)
    {
        if (value == 0) return value;

        float angle = Vector2.Angle(input, Vector2.up);
        if (snapAxis == EAxisOptions.Horizontal)
        {
            if (angle < 22.5f || angle > 157.5f) return 0;
            else return (value > 0) ? 1 : -1;
        }
        else if (snapAxis == EAxisOptions.Vertical)
        {
            if (angle > 67.5f && angle < 112.5f) return 0;
            else return (value > 0) ? 1 : -1;
        }

        return value;
    }

    private Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        float width = _rectTransform.rect.width * 0.5f;
        float height = _rectTransform.rect.height * 0.15f;

        return screenPosition - new Vector2(width, height);
    }
}

