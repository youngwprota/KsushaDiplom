using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TracingEvaluator : MonoBehaviour
{
    [Tooltip("Ссылка на TemplateDrawer (эталон)")]
    public TemplateDrawer templateDrawer;

    [Tooltip("Ссылка на DrawingController")]
    public DrawingController drawingController;

    [Tooltip("Порог (в юнитах) — расстояние от шаблонной точки до ближайшей точки игрока, чтобы считать её покрытой.")]
    public float coverThreshold = 0.25f;

    [Tooltip("Процент покрытия шаблона, от которого считаем успех (0..1).")]
    [Range(0f, 1f)]
    public float requiredCoverage = 0.85f;

    [Tooltip("UI текст для вывода результата (необязательно).")]
    public TMP_Text resultText;

    void Start()
    {
        if (templateDrawer == null) templateDrawer = GetComponent<TemplateDrawer>();
        if (drawingController == null) drawingController = GetComponent<DrawingController>();
    }

    void OnStrokeFinished(object strokeObj)
    {
        List<Vector3> stroke = strokeObj as List<Vector3>;
        EvaluateStroke(stroke);
    }

    public void EvaluateStroke(List<Vector3> stroke)
    {
        Vector3[] template = templateDrawer.GetTemplatePoints();
        if (template == null || template.Length == 0)
        {
            ShowResult(false);
            return;
        }

        if (stroke == null || stroke.Count == 0)
        {
            ShowResult(false);
            return;
        }

        int covered = 0;
        for (int i = 0; i < template.Length; i++)
        {
            Vector3 t = template[i];
            bool isCovered = IsPointCovered(t, stroke, coverThreshold);
            if (isCovered) covered++;
        }

        float coverage = (float)covered / (float)template.Length;
        Debug.Log($"Coverage: {coverage:F2} ({covered}/{template.Length})");

        bool success = coverage >= requiredCoverage;
        ShowResult(success);
    }

    bool IsPointCovered(Vector3 templatePoint, List<Vector3> stroke, float threshold)
    {
        float sqrThresh = threshold * threshold;
        for (int i = 0; i < stroke.Count; i++)
        {
            if ((stroke[i] - templatePoint).sqrMagnitude <= sqrThresh) return true;
        }
        return false;
    }

    void ShowResult(bool success)
    {
        if (resultText != null)
        {
            resultText.text = success ? "Правильно! 🎉" : "Попробуй ещё ☺️";
        }

        // Можно добавить анимацию, звук и т.д.
    }

    // Вызов для ручной проверки (кнопка Check)
    public void CheckCurrentStroke()
    {
        EvaluateStroke(drawingController.GetStrokePoints());
    }
}
