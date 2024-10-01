using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector2 lastMousePosition;
    [SerializeField] private Canvas canvas;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
        lastMousePosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentMousePosition = eventData.position;
        Vector2 diff = currentMousePosition - lastMousePosition;
        RectTransform rect = GetComponent<RectTransform>();

        Vector3 newPosition = rect.position + new Vector3(diff.x, diff.y, transform.position.z);
        Vector3 oldPos = rect.position;
        rect.position = newPosition;
        if (!IsRectTransformInsideSreen(rect))
        {
            rect.position = oldPos;
        }
        lastMousePosition = currentMousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");

    }

    private bool IsRectTransformInsideSreen(RectTransform rectTransform)
    {
        bool isInside = false;
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        int visibleCorners = 0;
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        foreach (Vector3 corner in corners)
        {
            if (rect.Contains(corner))
            {
                visibleCorners++;
            }
        }
        if (visibleCorners == 4)
        {
            isInside = true;
        }
        return isInside;
    }

    private bool isRectInsideParent(RectTransform transform1, RectTransform trasnform2)
    {
        var r = GetWorldRect(transform1, new Vector2(canvas.scaleFactor, canvas.scaleFactor));
        var a = GetWorldRect(trasnform2, new Vector2(canvas.scaleFactor, canvas.scaleFactor));
        return r.xMin <= a.xMin && r.yMin <= a.yMin && r.xMax >= a.xMax && r.yMax >= a.yMax;
    }

    private Rect GetWorldRect(RectTransform rect, Vector2 scale)
    {
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);
        Vector3 topLeft = corners[0];

        Vector2 scaledSize = new Vector2(scale.x * rect.rect.size.x, scale.y * rect.rect.size.y);

        return new Rect(topLeft, scaledSize);
    }
}
