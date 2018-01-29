using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using TimeSeries;

public class BikeDataBufferTest : DataSeriesBuffer
{

    double northingOffset = -6579336.14918;
    double eastingOffset = -149729.914346;

    public double latitude;
    public double longitude;

    static GisConvert.ConversionParameters conversionParams;

    // Use this for initialization
    void Start()
    {
        conversionParams = GisConvert.MakeSweref99L1800();
    }

    // Update is called once per frame
    void Update()
    {

        double northing = 0.0;
        double easting = 0.0;

        GisConvert.GeodeticToGrid(latitude, longitude, conversionParams, out northing, out easting);

        float northingWithOffset = (float)(northing + northingOffset);
        float eastingWithOffset = (float)(easting + eastingOffset);

        transform.position = new Vector3(eastingWithOffset, transform.position.y, northingWithOffset);
    }

    override public void TimeDataUpdate(Subscription Sub, DataPoint data)
    {

        //Debug.Log("lat: " + data.Values[0]);
        //Debug.Log("long: " + data.Values[1]);

        //if (NodeName == "Heating Energy")
        //	print ("Heating Energy data recived");

        if (data == null)
            return;

        if (data.Values == null)
            return;


            //lat
            latitude = data.Values[0];

            //long
            longitude = data.Values[1];

            // print (value);


    }

}
