using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{

    public static void DrawLine(LineRenderer lineRenderer, Vector3 start, Vector3 end, float startWidth = 0.02f, float endWidth = 0.02f)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    public static void ClearLines(LineRenderer lineRenderer)
    {
        lineRenderer.positionCount = 0;
    }



}
