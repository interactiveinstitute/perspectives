using UnityEngine;
using System;
//using TimeSeries;

public class bikeController2 : DataNode {

	private int SubpropertyId = 0;
	private string Unit = "";
	private int decimals = 0;
	public double scale = 1;
	private double value;

    double northingOffset = -6579336.14918;
	double eastingOffset = -149729.914346;

	public double latitude;
	public double longitude;

	static GisConvert.ConversionParameters conversionParams;


	// Use this for initialization
	void Start () {
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



    override public void TimeDataUpdate(Subscription Sub, DataPoint data) {

		Debug.Log ("lat: " + data.Values[0]);
        Debug.Log("long: " + data.Values[1]);

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
		if (data.Values[SubpropertyId] != null) {

			 newtext = Math.Round(data.Values[SubpropertyId]/scale,decimals).ToString() + " " + Unit;

            //lat
			latitude = data.Values[0];

            //long
            longitude = data.Values[1];

			// print (value);

		}

	}
}
