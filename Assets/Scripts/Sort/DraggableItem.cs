using Sort;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private Vector3 startPosition;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        startPosition = rectTransform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        GameObject dropTarget = eventData.pointerCurrentRaycast.gameObject;
        if (dropTarget != null && dropTarget.CompareTag(gameObject.tag))
        {
            rectTransform.position = dropTarget.transform.position;
            SortGameManager.Instance.RegisterCorrectItem();
            Destroy(this); 
        }
        else
        {
            rectTransform.position = startPosition; // возвращаем на место
        }
    }
}