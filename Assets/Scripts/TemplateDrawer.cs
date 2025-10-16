using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TemplateDrawer : MonoBehaviour
{
    [Tooltip("Шаблон буквы в локальных координатах (XY). Настройте в инспекторе.")]
    public Vector3[] templatePoints;

    [Tooltip("LineRenderer, который рисует слабый шаблон (пунктир/полупрозрачный).")]
    public LineRenderer templateLine;

    void Reset()
    {
        templateLine = GetComponent<LineRenderer>();
    }

    void Start()
    {
        DrawTemplate();
    }

    [ContextMenu("Draw Template")]
    public void DrawTemplate()
    {
        if (templateLine == null) return;
        if (templatePoints == null || templatePoints.Length == 0)
        {
            templateLine.positionCount = 0;
            return;
        }

        templateLine.positionCount = templatePoints.Length;
        for (int i = 0; i < templatePoints.Length; i++)
        {
            templateLine.SetPosition(i, templatePoints[i]);
        }
    }

    // Для доступа внешних скриптов
    public Vector3[] GetTemplatePoints()
    {
        return templatePoints;
    }
}
