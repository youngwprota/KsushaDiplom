using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LineRenderer))]
public class DrawingController : MonoBehaviour
{
    public LineRenderer strokeLine; // LineRenderer для линии игрока
    public float minPointDistance = 0.02f; // минимальное расстояние между точками записи
    public Camera drawingCamera; // камера, если NULL возьмём Camera.main

    private List<Vector3> points = new List<Vector3>();
    private bool isDrawing = false;
    public TracingEvaluator tracingEvaluator;
    void Reset()
    {
        strokeLine = GetComponent<LineRenderer>();
    }

    void Start()
    {
        if (drawingCamera == null) drawingCamera = Camera.main;
        ClearStroke();
    }

    void Update()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        #if UNITY_ANDROID || UNITY_IOS
        HandleTouch();
        #else
        HandleMouse();
        #endif
    }

    void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BeginStroke(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && isDrawing)
        {
            ContinueStroke(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0) && isDrawing)
        {
            EndStroke();
        }
    }

    void HandleTouch()
    {
        if (Input.touchCount == 0) return;
        Touch t = Input.GetTouch(0);
        if (t.phase == TouchPhase.Began) BeginStroke(t.position);
        else if ((t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary) && isDrawing) ContinueStroke(t.position);
        else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) EndStroke();
    }

    void BeginStroke(Vector2 screenPos)
    {
        isDrawing = true;
        points.Clear();
        ContinueStroke(screenPos);
    }

    
    void ContinueStroke(Vector2 screenPos)
    {
        Vector3 worldPos = ScreenToWorld(screenPos);
        if (points.Count == 0 || Vector3.Distance(points[points.Count - 1], worldPos) >= minPointDistance)
        {
            points.Add(worldPos);
            UpdateLineRenderer();
        }
    }

    void EndStroke()
    {
        isDrawing = false;
        if (tracingEvaluator != null)
            tracingEvaluator.EvaluateStroke(points);
    }

    Vector3 ScreenToWorld(Vector2 screenPos)
    {
        float zDistance = Mathf.Abs(drawingCamera.transform.position.z - 0f); // Z=0 — плоскость буквы
        Vector3 sp = new Vector3(screenPos.x, screenPos.y, zDistance);
        return drawingCamera.ScreenToWorldPoint(sp);
    }


    void UpdateLineRenderer()
    {
        strokeLine.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
            strokeLine.SetPosition(i, points[i]);
    }

    public void ClearStroke()
    {
        points.Clear();
        strokeLine.positionCount = 0;
    }

    public List<Vector3> GetStrokePoints()
    {
        return new List<Vector3>(points);
    }
}
