using BubbleSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Line : MonoBehaviour {

    private LineRenderer lineRenderer;
    private IEnumerator drawCoroutine;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawLine(BubbleSystem.Situation situation)
    {
        if (BubbleSystemUtility.CheckCoroutine(ref drawCoroutine))
            StopCoroutine(drawCoroutine);

        drawCoroutine = Draw(DefaultData.Instance.GetSituationGraph(situation));
        StartCoroutine(drawCoroutine);
    }

    IEnumerator Draw(List<Vector3> positions)
    {
        lineRenderer.positionCount = positions.Count;
        float duration = DefaultData.Instance.GetBackgroundDuration();
        float initialTime;
        Vector3 intermediatePoint;
        float time;

        for (int i = 0; i < positions.Count; i++)
            lineRenderer.SetPosition(i, positions[0]);

        for (int i = 1; i < positions.Count; i++)
        {
            initialTime = Time.time;
            lineRenderer.SetPosition(i, positions[i - 1]);

            while ((time = ((Time.time - initialTime) * (positions.Count - 1)) / duration) < 1)
            {
                intermediatePoint = Vector3.Lerp(positions[i - 1], positions[i], time);
                lineRenderer.SetPosition(i, intermediatePoint);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            lineRenderer.SetPosition(i, positions[i]);
        }
    }    
}
