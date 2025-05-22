using UnityEngine;
using UnityEngine.EventSystems;

public class CursorHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D hoverCursor;

    public Texture2D defaultCursor;
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverCursor != null)
            Cursor.SetCursor(hoverCursor, hotspot, cursorMode);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, cursorMode);
    }
}
