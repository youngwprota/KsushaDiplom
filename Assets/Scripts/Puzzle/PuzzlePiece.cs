using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Vector3 startPosition;
    private Canvas canvas;
    private bool placedCorrectly = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        startPosition = rectTransform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (placedCorrectly) return;
        GetComponent<Image>().raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (placedCorrectly) return;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (placedCorrectly) return;

        GameObject target = eventData.pointerCurrentRaycast.gameObject;
        if (target != null && target.CompareTag(gameObject.tag))
        {
            rectTransform.position = target.transform.position;
            placedCorrectly = true;
            PuzzleGameManager.Instance.RegisterCorrectPiece();
        }
        else
        {
            rectTransform.position = startPosition;
            GetComponent<Image>().raycastTarget = true;

        }
    }
}