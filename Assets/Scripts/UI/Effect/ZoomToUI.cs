using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomToUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private float zoomFactor = 1.5f;  // ¡‹¿Œ πË¿≤
    [SerializeField] private float duration = 0.5f;    // ¡‹¿Œ Ω√∞£
    [SerializeField] private RectTransform canvasTransform;  // Canvas¿« RectTransform
    [SerializeField] private Canvas canvas;  // Canvas ƒƒ∆˜≥Õ∆Æ
    [SerializeField] private UI_Map map;

    private Vector3 originalScale;
    private Vector3 originalPosition;
    private bool isZoomedIn = false;
    private RectTransform targetElement;

    void Start()
    {
        canvasTransform = transform.parent.parent.GetComponent<RectTransform>();
        canvas = map.canvas;

        if (canvasTransform != null)
        {
            originalScale = canvasTransform.parent.localScale;
            originalPosition = canvasTransform.parent.localPosition;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        RectTransform clickedElement = eventData.pointerPress.GetComponent<RectTransform>();
        if (clickedElement != null)
        {
            targetElement = clickedElement;
            if (isZoomedIn)
            {
                StartCoroutine(ZoomOut());
            }
            else
            {
                StartCoroutine(ZoomIn(targetElement));
            }
            isZoomedIn = !isZoomedIn;
        }
    }

    private IEnumerator ZoomIn(RectTransform target)
    {
        Vector3 targetScale = originalScale * zoomFactor;
        Vector3 targetPosition = GetCanvasPosition(target) * zoomFactor;

        yield return ScaleAndMoveOverTime(targetScale, targetPosition);
    }

    private IEnumerator ZoomOut()
    {
        yield return ScaleAndMoveOverTime(originalScale, originalPosition);
    }

    private IEnumerator ScaleAndMoveOverTime(Vector3 targetScale, Vector3 targetPosition)
    {
        Vector3 initialScale = canvasTransform.parent.localScale;
        Vector3 initialPosition = canvasTransform.parent.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            canvasTransform.parent.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            canvasTransform.parent.localPosition = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasTransform.parent.localScale = targetScale;
        canvasTransform.parent.localPosition = targetPosition;
        Debug.Log("targetScale" + targetScale);
        Debug.Log("targetPosition" + targetPosition);
    }

    private Vector3 GetCanvasPosition(RectTransform target)
    {
        if (canvas.worldCamera == null) 
            canvas.worldCamera = Camera.main;

        Vector2 viewportPoint = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, target.position);
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, viewportPoint, canvas.worldCamera, out localPoint);
        return localPoint;
    }
}
