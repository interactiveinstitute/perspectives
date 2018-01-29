using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFromLatLon : MonoBehaviour {

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
	void Update () {

        double northing = 0.0;
        double easting = 0.0;

        GisConvert.GeodeticToGrid(latitude, longitude, conversionParams, out northing, out easting);

        float northingWithOffset = (float)(northing + northingOffset);
        float eastingWithOffset = (float)(easting + eastingOffset);

        transform.position = new Vector3(eastingWithOffset, transform.position.y, northingWithOffset);
	}
}
