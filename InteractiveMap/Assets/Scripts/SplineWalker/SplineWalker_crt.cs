using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
//using TimeSeries;


public class SplineWalker_crt : DataNode
{
    public BezierSpline spline;
    private float energy;
    public float count;
    private float ratio;
    public GameObject prefab;

    public float floatValue;

    public int SubpropertyId = 0;
    private string Unit = "";
    private int decimals = 0;
    public double scale = 1;
    private double value;
    public bool turnOff = false;

    private List<Transform> transformList = new List<Transform>();
    public bool lookForward;
    public SplineWalkerMode mode;
    public float progress;
    private bool goingForward = true;

    float splineLength;

    void Start()
    {
        if (spline.createUniformSpline)
        {
            spline.CreateUniformSpline();
        }

        splineLength = spline.GetLength();

        float ratio = 1 / count;
        Transform t;

        for (int i = 0; i < count; i++)
        {
            t = Instantiate(prefab.transform, new Vector3(), Quaternion.identity);
            transformList.Add(t);
            //		progress = progress + ratio;
            //		t.position = spline.GetPoint (progress);
            //		transform.localPosition = t.position;
        }
    }


    public void Update()
    {
        foreach (Transform element in transformList)
        {
            // energy = mqttData.mspPowerFloat*count;
            // convert double to float
            floatValue = (float)value;

            if (turnOff == false)
            {
                energy = floatValue;
            }
            else
            {
                energy = 0f;
            }

            ratio = 1 / count;
            progress = progress + ratio;

            // Debug.Log("test " + mqttData.mspPowerFloat);
            if (energy > 0)
            {
                if (goingForward)
                {
                    float deltaProgress = Time.deltaTime * energy * ratio / splineLength;
                    progress += deltaProgress;

                    if (progress > 1f)
                    {
                        if (mode == SplineWalkerMode.Once)
                        {
                            progress = 1f;
                        }
                        else if (mode == SplineWalkerMode.Loop)
                        {
                            progress -= 1f;
                        }
                        else
                        {
                            progress = 2f - progress;
                            goingForward = false;
                        }
                    }
                }
                else
                {
                    progress -= Time.deltaTime / energy;
                    if (progress < 0f)
                    {
                        progress = -progress;
                        goingForward = true;
                    }
                }

                if (spline.createUniformSpline)
                {
                    element.position = spline.GetUniformPoint(progress);
                }
                else
                {
                    element.position = spline.GetPoint(progress);
                }

                transform.localPosition = element.position;

                if (lookForward)
                {
                    transform.LookAt(element.position + spline.GetDirection(progress));
                }
            }
        }
    }

    override public void TimeDataUpdate(Subscription Sub, DataPoint data)
    {

        string newtext = "";

        //if (NodeName == "Heating Energy")
        //	print ("Heating Energy data recived");

        if (data == null)
            return;

        if (data.Values == null)
            return;

        if (SubpropertyId >= data.Values.Length)
            return;

        //Debug.Log (data.Values.Length);
        //Debug.Log (SubpropertyId);
        if (data.Values[SubpropertyId] != null)
        {

            newtext = Math.Round(data.Values[SubpropertyId] / scale, decimals).ToString() + " " + Unit;

            value = data.Values[SubpropertyId] / scale;







            // print (value);

        }





    }


    public void ElectricityActivate()
    {

        turnOff = false;
    }
    public void ElectricityDectivate()
    {

        turnOff = true;
    }


}