using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SensorDevice : DataNode {

    public string sensorDeviceName;
    public string sensorDeviceDescription;
    //public GameObject sensorDeviceCanvas;
    public bool isMobile = false;
    public float posLerpSpeed = 0.5f;
    private Vector3 deviceDes;
    public Color myColor;

    public Vector3 myLineDes;
    public LineRenderer myLine;
    public Vector3[] myLinePositions;
    public int myLineIndex = 0;
    public float myLineScale = 10f;

    //declear vars and set initial lat long position for sensorbikes so they don`t start at 0,0,0
    public double latitude = 59.40393;
    public double longitude = 17.94877;
    public float temperature = 0.0f;
    public float humidity = 0.0f;
    public float decibel = 0.0f;

    private TrailRenderer myTrailRenderer;

    private float eastingWithOffset;
    private float northingWithOffset;

    public CoordinateSettings coordinateSettings;

    //double northingOffset = -6579336.14918;
    //double eastingOffset = -149729.914346;


    public SpriteRenderer spriteHoverIndicator;
    public SpriteRenderer spriteSelectedIndicator;
    public SpriteRenderer spriteReleaseIndicator;

	static GisConvert.ConversionParameters conversionParams;

    public SensorDevicesController devicesController;

	// Use this for initialization
	void Start () {

        if (isMobile == false) {
            this.GetComponent<TrailRenderer>().enabled = false;
        }
        
        if(sensorDeviceName == null)
            sensorDeviceName = transform.name;

        myColor = GetComponentInChildren<SpriteRenderer>().color;
        myLine = GetComponent<LineRenderer>();
        //myLine.positionCount = 20;
        //GetComponent<TrailRenderer>().startColor = GetComponent<SpriteRenderer>().color;

        deviceDes = transform.position;
        conversionParams = conversionParams = GisConvert.MakeConversionParameters(coordinateSettings.gridType);

        if (devicesController == null)
        {
            devicesController = GetComponentInParent<SensorDevicesController>();
        }

        Deselect();
        StopHover();
	}



    // Update is called once per frame
    void Update()
    {
        if (isMobile)
        {
            transform.position = Vector3.Lerp(transform.position, deviceDes, posLerpSpeed * Time.deltaTime);

            ////Debug.Log("myLineIndex: " + myLineIndex);
            //myLine.SetPosition(myLineIndex, new Vector3(transform.position.x, (decibel / myLineScale), transform.position.z));
        }
        else
        {
            transform.position = deviceDes;
        }
    }

    override public void TimeDataUpdate(Subscription Sub, DataPoint data) {

		if (data.Values[0] != null) {

            double northing = 0.0;
            double easting = 0.0;

            //tempText.text = latitude.ToString();
            GisConvert.GeodeticToGrid(latitude, longitude, conversionParams, out northing, out easting);

            northingWithOffset = (float)(northing + coordinateSettings.northingOffset);
            eastingWithOffset = (float)(easting + coordinateSettings.eastingOffset);

            latitude = data.Values[0];
            longitude = data.Values[1];
            temperature = (float)data.Values[2];
            humidity = (float)data.Values[3];
            decibel = (float)data.Values[4];

            //set the new destination for the bike
            deviceDes = new Vector3(eastingWithOffset, transform.position.y, northingWithOffset);

            
            /*if (isMobile) {

                //myLineDes = transform.localPosition;
                //Debug.Log("new line des:" + myLineDes);
                myLineDes = new Vector3(transform.position.x, decibel/myLineScale, transform.position.z);
                myLine.SetPosition(myLineIndex, myLineDes);
                myLineIndex++;
                if (myLineIndex == myLine.positionCount)
                {
                    myLineIndex = 0;
                }
                //myLine.positionCount = myLineIndex;
                //Debug.Log("Set pos count: " + myLine.positionCount);
            }*/
            

            LastData = data;
        }

	}

    public void StartHover()
    {
        //PointerEventData pointerEvent = baseEventData as PointerEventData;
        spriteHoverIndicator.enabled = true;
        //Debug.Log("StartHover");
    }

    public void StopHover()
    {
        spriteHoverIndicator.enabled = false;
        //Debug.Log("StopHover");
    }

    public void Deselect()
    {
        spriteReleaseIndicator.enabled = false;
        spriteSelectedIndicator.enabled = false;
    }

    public void Select()
    {
        //if (devicesController.selectedDevice == this)
        //{
        //    Deselect();
        //}
        //else
        //{
        spriteReleaseIndicator.enabled = true;
        spriteSelectedIndicator.enabled = true;
        //}
    }
}
