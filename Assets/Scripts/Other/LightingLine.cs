using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class LightingLine : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    public int pointCount;     //偏移的点的数量

    private LineRenderer lineRenderer;

    private List<Vector3> points = new List<Vector3>();
    public List<float> pointX = new List<float>();

    //y = k * x + b
    //b = y - k * x
    float k = 0;
    float b = 0;

    public float range = 0.6f;

    public float f = 0.1f;    //波动的频率
    float timer;

    private void Start()
    {
        lineRenderer = transform.GetComponent<LineRenderer>();
        //GetPoints();
        k = (endPoint.position.y - startPoint.position.y) / (endPoint.position.x - startPoint.position.x);
        b = startPoint.position.y - k * startPoint.position.x;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > f)
        {
            lineRenderer.positionCount = pointCount + 2;
            lineRenderer.SetPositions(GetPoints());
            timer = 0;
        }
        
    }

    public Vector3[] GetPoints()
    {
        points.Clear();
        pointX.Clear();

        points.Add(startPoint.position);
        points.Add(endPoint.position);

        pointX.Add(startPoint.position.x);
        pointX.Add(endPoint.position.x);

        for (int i = 0; i < pointCount; i++)
        {
            pointX.Add(Random.Range(startPoint.position.x, endPoint.position.x));
        }

        pointX.Sort();

        //根据随机的点 来求y
        for (int i = 0; i < pointX.Count; i++)
        {
            float y = k * pointX[i] + b;
            if (i == 0 || i == pointX.Count - 1)
            {
                points.Add(new Vector3(pointX[i], y , 0));
            }
            else
            {
                points.Add(new Vector3(pointX[i], y + Random.Range(-range, range), 0));
            }

        }

        return points.ToArray();
    }
}
