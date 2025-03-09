using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableNoteButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int noteIndex; // The note index this button represents.

    private Transform originalParent;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition; // Store the original position

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        // Find the parent canvas (required for correct drag behavior).
        canvas = GetComponentInParent<Canvas>();
        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition; // Capture the original position
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Move to the canvas root so it appears on top.
        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move this note button with the mouse.
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        // Always return the button to its original parent & position.
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = originalPosition; // Reset position to where it began.
    }
}


