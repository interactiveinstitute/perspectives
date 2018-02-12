using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFromLatLon : MonoBehaviour {

    //double northingOffset = -6579336.14918;
    //double eastingOffset = -149729.914346;

    public double latitude;
    public double longitude;

    public CoordinateSettings coordinateSettings;

    static GisConvert.ConversionParameters conversionParams;

	// Use this for initialization
	void Start () {
        conversionParams = GisConvert.MakeConversionParameters(coordinateSettings.gridType);
	}
	
	// Update is called once per frame
	void Update () {

        double northing = 0.0;
        double easting = 0.0;

        GisConvert.GeodeticToGrid(latitude, longitude, conversionParams, out northing, out easting);

        float northingWithOffset = (float)(northing + coordinateSettings.northingOffset);
        float eastingWithOffset = (float)(easting + coordinateSettings.eastingOffset);
        //Debug.Log("Update northing easting: " + northing + " " + easting);

        transform.position = new Vector3(eastingWithOffset, transform.position.y, northingWithOffset);
	}
}
