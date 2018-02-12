using UnityEngine;
//using UnityEditor;
using System;
using System.Collections.Generic;

public class BezierSpline : MonoBehaviour
{

    [SerializeField]
    private Vector3[] points;

    [SerializeField]
    public bool createUniformSpline = false;

    [SerializeField]
    public float uniformSplineSampleDistance = 0.1f;

    [SerializeField]
    public float numMetersPerUniformPoint = 1f;

    private Vector3[] uniformPoints;

    [SerializeField]
    private BezierControlPointMode[] modes;

    [SerializeField]
    private bool loop;

    [SerializeField]
    public bool displayWhenNotSelected;

    //[Header("Debug")]
    [SerializeField]
    public float splineSmoothness = 0.96f;
    [SerializeField]
    public Color splineColor = Color.yellow;
    [SerializeField]
    public Color pointSphereColor = Color.white;
    [SerializeField]
    public float pointSphereRadius = 0.3f;


    public bool Loop
    {
        get
        {
            return loop;
        }
        set
        {
            loop = value;
            if (value == true)
            {
                modes[modes.Length - 1] = modes[0];
                SetControlPoint(0, points[0]);
            }
        }
    }

    public int ControlPointCount
    {
        get
        {
            return points.Length;
        }
    }

    public Vector3 GetControlPoint(int index)
    {
        return points[index];
    }

    public float GetLength()
    {

        // First calculate the length between the control points to find out resolution of the spline
        float controlPointDistanceSum = 0;
        for (int i = 1; i < points.Length; i++)
        {
            float distance = Vector3.Magnitude(points[i - 1] - points[i]);
            controlPointDistanceSum += distance;
        }

        float splineLength = 0;
        int numPoints = Mathf.FloorToInt(controlPointDistanceSum / uniformSplineSampleDistance);
        Vector3 previousPoint = GetPoint(0.0f);
        for (int i = 1; i < numPoints; i++)
        {
            float t = i / (float)numPoints;
            Vector3 point = GetPoint(t);

            float distance = Vector3.Magnitude(point - previousPoint);

            splineLength += distance;

            previousPoint = point;
        }

        return splineLength;
    }

    public void CreateUniformSpline()
    {
        //Debug.Log("CreateUniformSpline()");
        float splineLength = GetLength();

        float numUniformPointsFloat = splineLength / numMetersPerUniformPoint;
        float numUniformPointsFloatFloored = Mathf.Floor(numUniformPointsFloat);

        numMetersPerUniformPoint = splineLength / numUniformPointsFloatFloored;

        //Debug.Log("splineLength: " + splineLength);
        //Debug.Log("numUniformPointsFloat: " + numUniformPointsFloat);
        //Debug.Log("numUniformPointsFloatFloored: " + numUniformPointsFloatFloored);
        //Debug.Log("numMetersPerUniformPoint: " + numMetersPerUniformPoint);


        List<Vector3> uniformPointsList = new List<Vector3>();

        float t = 0f;
        float tStep = 1.0f / (splineLength / uniformSplineSampleDistance);
        float distanceSum = 0;
        float nextDistance = distanceSum + numMetersPerUniformPoint;

        Vector3 firstSamplePoint = GetPoint(t, false);
        //Debug.Log("firstSamplePoint: " + firstSamplePoint);
        uniformPointsList.Add(firstSamplePoint);
        Vector3 previousSamplePoint = firstSamplePoint;
        t += tStep;

        //int numSteps = 10;
        //for (int i = 0; i < numSteps; i++)
        //{
        //    float tt = i / (float)numSteps;
        //    Vector3 p = GetPoint(tt, false);
        //    Debug.Log("tt: " + tt + ", p: " + p);
        //}

        while (t < 1.0f)
        {
            Vector3 samplePoint = GetPoint(t, false);
            //Debug.Log("samplePoint: " + samplePoint);
            float distance = Vector3.Magnitude(samplePoint - previousSamplePoint);
            distanceSum += distance;
            //Debug.Log("t: " + t + ", distanceSum: " + distanceSum);

            if (distanceSum > nextDistance)
            {
                //Debug.Log("ADD POINT: " + samplePoint);

                uniformPointsList.Add(samplePoint);

                //nextDistance += numMetersPerUniformPoint;
                nextDistance = distanceSum + numMetersPerUniformPoint;
            }

            previousSamplePoint = samplePoint;
            t += tStep;
        }

        uniformPoints = new Vector3[uniformPointsList.Count];
        for (int i = 0; i < uniformPointsList.Count; i++)
        {

            uniformPoints[i] = uniformPointsList[i];
        }


        //uniformPoints = new Vector3[numUniformPoints];

        //float t = 0f;
        //float tStep = 1.0f / (splineLength / uniformSplineSampleDistance);
        //float nextDistance = 0f;
        //float distanceSum = 0f;
        //Vector3 curvePointFirst = GetPoint(0f, false);
        //Debug.Log("curvePointFirst: " + curvePointFirst);
        //Debug.Log("t: " + t);
        //Debug.Log("tStep: " + tStep);
        //uniformPoints[0] = curvePointFirst;
        //Vector3 curvePointPrevious = curvePointFirst;
        //distanceSum += Vector3.Magnitude(curvePointFirst - curvePointPrevious);
        //float realDistanceSum = distanceSum;
        //Debug.Log("distanceSum: " + distanceSum);
        //t += tStep;
        //nextDistance += numMetersPerUniformPoint;

        //Debug.Log("nextDistance: " + nextDistance);
        
        ////Debug.Log("2");


        ////Debug.Log("t: " + t);
        ////Debug.Log("tStep: " + tStep);
        ////Debug.Log("nextDistance: " + nextDistance);
        ////Debug.Log("distanceSum: " + distanceSum);

        //for (int i = 1; i < numUniformPoints; i++)
        //{
        //    Debug.Log("i: " + i);
        //    int breakCounter = 0;
        //    int breakCounterLimit = 400;
        //    while (distanceSum < nextDistance)
        //    {
        //        Debug.Log("distanceSum, nextDistance: " + distanceSum + ", " + nextDistance );
        //        Vector3 curvePoint = GetPoint(t, false);
        //        //distanceSum += Vector3.Magnitude(curvePoint - curvePointPrevious);
        //        float tempDistance = Vector3.Magnitude(curvePoint - curvePointPrevious);
        //        Debug.Log("t: " + t);
        //        Debug.Log("curvePoint: " + curvePoint);
        //        Debug.Log("Temp distance: " + tempDistance);
        //        //distanceSum += 0.2f;
        //        distanceSum += tempDistance;
        //        //realDistanceSum += tempDistance;
        //        //Debug.Log("realDistanceSum: " + realDistanceSum);

        //        curvePointPrevious = curvePoint;
        //        t += tStep;
        //        breakCounter++;

        //        if (breakCounter > breakCounterLimit)
        //        {
        //            Debug.Log("Reached breakcounter");
        //            break;
        //        }
        //    }
        //    Debug.Log("ADD POINT: " + curvePointPrevious);
        //    uniformPoints[i] = curvePointPrevious;

        //    nextDistance += numMetersPerUniformPoint;
        //}
        ////Debug.Log("3");

        //// Do a check to see the distance to last point.
        //float distanceBetweenLastUniformPointAndLastControlPoint = Vector3.Magnitude(curvePointPrevious - points[points.Length - 1]);
        //if (distanceBetweenLastUniformPointAndLastControlPoint > numMetersPerUniformPoint * 0.9f)
        //{
        //    //Debug.Log("Adding a point to uniform curve");

        //    Vector3[] extendedUniformPoints = new Vector3[uniformPoints.Length + 1];
        //    for (int i = 0; i < uniformPoints.Length; i++)
        //    {
        //        extendedUniformPoints[i] = uniformPoints[i];
        //    }
        //    extendedUniformPoints[extendedUniformPoints.Length - 1] = GetPoint(1f, false);
        //    uniformPoints = extendedUniformPoints;
        //}
        ////Debug.Log("4");
    }

    public void SetControlPoint(int index, Vector3 point)
    {
        if (index % 3 == 0)
        {
            Vector3 delta = point - points[index];
            if (loop)
            {
                if (index == 0)
                {
                    points[1] += delta;
                    points[points.Length - 2] += delta;
                    points[points.Length - 1] = point;
                }
                else if (index == points.Length - 1)
                {
                    points[0] = point;
                    points[1] += delta;
                    points[index - 1] += delta;
                }
                else
                {
                    points[index - 1] += delta;
                    points[index + 1] += delta;
                }
            }
            else
            {
                if (index > 0)
                {
                    points[index - 1] += delta;
                }
                if (index + 1 < points.Length)
                {
                    points[index + 1] += delta;
                }
            }
        }
        points[index] = point;
        EnforceMode(index);
    }

    public BezierControlPointMode GetControlPointMode(int index)
    {
        return modes[(index + 1) / 3];
    }

    public void SetControlPointMode(int index, BezierControlPointMode mode)
    {
        int modeIndex = (index + 1) / 3;
        modes[modeIndex] = mode;
        if (loop)
        {
            if (modeIndex == 0)
            {
                modes[modes.Length - 1] = mode;
            }
            else if (modeIndex == modes.Length - 1)
            {
                modes[0] = mode;
            }
        }
        EnforceMode(index);
    }

    private void EnforceMode(int index)
    {
        int modeIndex = (index + 1) / 3;
        BezierControlPointMode mode = modes[modeIndex];
        if (mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Length - 1))
        {
            return;
        }

        int middleIndex = modeIndex * 3;
        int fixedIndex, enforcedIndex;
        if (index <= middleIndex)
        {
            fixedIndex = middleIndex - 1;
            if (fixedIndex < 0)
            {
                fixedIndex = points.Length - 2;
            }
            enforcedIndex = middleIndex + 1;
            if (enforcedIndex >= points.Length)
            {
                enforcedIndex = 1;
            }
        }
        else
        {
            fixedIndex = middleIndex + 1;
            if (fixedIndex >= points.Length)
            {
                fixedIndex = 1;
            }
            enforcedIndex = middleIndex - 1;
            if (enforcedIndex < 0)
            {
                enforcedIndex = points.Length - 2;
            }
        }

        Vector3 middle = points[middleIndex];
        Vector3 enforcedTangent = middle - points[fixedIndex];
        if (mode == BezierControlPointMode.Aligned)
        {
            enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
        }
        points[enforcedIndex] = middle + enforcedTangent;
    }

    public int CurveCount
    {
        get
        {
            return (points.Length - 1) / 3;
        }
    }

    public Vector3 GetPoint(float t, bool inWorldSpaceCoordinates = true)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }

        if (inWorldSpaceCoordinates)
        {
            return transform.TransformPoint(Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t));
        }
        else
        {
            return Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t);
        }
    }

    public Vector3 GetUniformPoint(float t)
    {
        if (uniformPoints == null)
        {
            Debug.LogWarning("There is no uniform curve created");
            return Vector3.zero;
        }

        float indexFloat = t * uniformPoints.Length;
        float indexFloatRemovePart = indexFloat / (float)uniformPoints.Length;
        indexFloat = indexFloat - indexFloatRemovePart;

        int indexLow = Mathf.FloorToInt(indexFloat);
        int indexHigh = indexLow + 1;

        float rest = indexFloat - indexLow;
        float ip = rest / (float)(indexHigh - indexLow);

        Vector3 lowPoint = uniformPoints[indexLow];
        Vector3 highPoint = uniformPoints[indexHigh];
        Vector3 uniformPoint = Vector3.Lerp(lowPoint, highPoint, ip);
        return transform.TransformPoint(uniformPoint);
    }

    public Vector3 GetVelocity(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }
        return transform.TransformPoint(Bezier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], t)) - transform.position;
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }

    public void AddCurve()
    {
        Vector3 point = points[points.Length - 1];
        Array.Resize(ref points, points.Length + 3);
        point.x += 1f;
        points[points.Length - 3] = point;
        point.x += 1f;
        points[points.Length - 2] = point;
        point.x += 1f;
        points[points.Length - 1] = point;

        Array.Resize(ref modes, modes.Length + 1);
        modes[modes.Length - 1] = modes[modes.Length - 2];
        EnforceMode(points.Length - 4);

        if (loop)
        {
            points[points.Length - 1] = points[0];
            modes[modes.Length - 1] = modes[0];
            EnforceMode(0);
        }
    }

    public void Reset()
    {
        points = new Vector3[] {
			new Vector3(1f, 0f, 0f),
			new Vector3(2f, 0f, 0f),
			new Vector3(3f, 0f, 0f),
			new Vector3(4f, 0f, 0f)
		};
        modes = new BezierControlPointMode[] {
			BezierControlPointMode.Free,
			BezierControlPointMode.Free
		};
    }

    void OnDrawGizmos()
    {
        if (displayWhenNotSelected /*&& !Selection.Contains(this.gameObject)*/)
        {
            // Draw spline
            float stepSize = 1.0f - splineSmoothness;

            float t = 0.0f;

            Vector3 prevPoint = GetPoint(t);
            t += stepSize;

            while (t <= 1.0f)
            {
                Vector3 point = GetPoint(t);
                Gizmos.color = splineColor;
                Gizmos.DrawLine(prevPoint, point);

                prevPoint = point;

                t += stepSize;
            }

            Vector3 pos = transform.position;

            // Draw control points
            for (int i = 0; i < points.Length; i = i + 3)
            {
                Gizmos.color = pointSphereColor;
                //Gizmos.DrawSphere(points[i] + pos, pointSphereRadius);
                Gizmos.DrawSphere(transform.TransformPoint(points[i]), pointSphereRadius);
            }

            // Draw uniform points
            if (uniformPoints != null)
            {
                for (int i = 0; i < uniformPoints.Length; i++)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(transform.TransformPoint(uniformPoints[i]), pointSphereRadius * 0.5f);
                }
            }
        }
    }
}