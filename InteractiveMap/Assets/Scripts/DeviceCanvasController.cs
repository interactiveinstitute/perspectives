using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DeviceCanvasController : MonoBehaviour {

    public Text title;
	public Text sensordata;
    public Text description;

    string deviceName;
    string deviceDescription;
    //public Button onButton;
    //public Button offButton;

    double lastTimestamp;

    Canvas canvas;
    GraphicRaycaster rayCaster;

    //private ElectricDevice selectedElectricDevice = null;
    //private SensorDevice selectedSensorDevice = null;

    private DataNode selectedDataNode = null;

	// Use this for initialization
	void Start () {
        canvas = GetComponent<Canvas>();
        rayCaster = GetComponent<GraphicRaycaster>();
	}
	
	// Update is called once per frame
	void Update () {

        //bool sensorSelected = false;

        //if (selectedElectricDevice || selectedSensorDevice)
        //{
        //    sensorSelected = true;
        //}

        //if (!sensorSelected && canvas.enabled)
        //{
        //    canvas.enabled = false;
        //}

        if (selectedDataNode)
        {
            //Debug.Log("Selected data node: " + selectedDataNode.name);
            DataPoint dp = selectedDataNode.LastData;
            if (dp != null)
            {
                //Debug.Log(selectedDataNode.name + " timestamp: " + dp.Timestamp);
                if (lastTimestamp != dp.Timestamp)
                {
                    string titleText = selectedDataNode.name + "\n";
                    titleText += "<size=8> Updated " + GameTime.GetInstance().TimestampToDateTime(dp.Timestamp).ToString("HH:mm:ss") + "</size>\n \n";

                    string sensorText = "<b>Sensor data</b> \n";
                    //Debug.Log("DataNode: " + selectedDataNode.name + " " + dp.Timestamp);

                    int numValues = dp.Values.Length;
                    int numColumns = selectedDataNode.Columns.Count;

                    //Debug.Log("Num values: " + dp.Values.Length);
                    //Debug.Log("Num columns: " + selectedDataNode.Columns.Count);
                    for (int i = 0; i < dp.Values.Length; i++)
                    {
                        if (numColumns > i)
                        {
                            sensorText += "<b>" + selectedDataNode.Columns[i] + "</b> \t";
                        }
                        sensorText += dp.Values[i] + "\n";
                    }
                    sensordata.text = sensorText;
                    title.text = titleText;
                    description.text = deviceDescription;

                    lastTimestamp = dp.Timestamp;
                }
            }
        }
        else
        {
            DisableCanvas();
        }
	}


    private void PopulateDataNode(DataNode dataNode)
    {

        selectedDataNode = dataNode;

        //title.text = dataNode.name + "\n " + "<size=8>Updated: " + GameTime.GetInstance().TimestampToDateTime(dataNode.Values[0]).ToString("HH:mm:ss") + "</size>\n \n";
        //sensordata.text = "temperature: " + sensorDevice.temperature.ToString() + "\n" + "Humidity: " + sensorDevice.humidity.ToString() + "\n" + "Decibel: " + sensorDevice.decibel.ToString();

        sensordata.text = "wait for data...";
        //deviceName = device.sensorDeviceName;
        //deviceDescription = device.sensorDeviceDescription;

        lastTimestamp = 0;

        //onButton.onClick.RemoveAllListeners();
        //offButton.onClick.RemoveAllListeners();

        //onButton.gameObject.SetActive(false);
        //offButton.gameObject.SetActive(false);

        canvas.enabled = true;
        GetComponent<GraphicRaycaster>().enabled = true;
    }

    public void Populate(ElectricDevice electricDevice)
    {
        PopulateDataNode(electricDevice);
    }

    public void Populate(SensorDevice sensorDevice)
    {
        PopulateDataNode(sensorDevice);

        deviceName = sensorDevice.sensorDeviceName;
        deviceDescription = sensorDevice.sensorDeviceDescription;
        GameObject.Find("TitleText").GetComponent<titleTextController>().updateTitle("FadeOutIn", deviceName);
    }

    //public void Populate(SensorDevice sensorDevice)
    //{
    //    selectedDataNode = sensorDevice;

    //    Debug.Log("Populate sensordevice");
    //    selectedSensorDevice = sensorDevice;

    //    title.text = sensorDevice.name;
    //    sensordata.text = "temperature: " + sensorDevice.temperature.ToString() + "\n" + "Humidity: " + sensorDevice.humidity.ToString() + "\n" + "Decibel: " + sensorDevice.decibel.ToString();

    //    onButton.onClick.RemoveAllListeners();
    //    offButton.onClick.RemoveAllListeners();

    //    onButton.gameObject.SetActive(false);
    //    offButton.gameObject.SetActive(false);

    //    canvas.enabled = true;
    //    GetComponent<GraphicRaycaster>().enabled = true;
    //}

    //public void DeselectSensorDevice()
    //{
    //    selectedSensorDevice = null;
    //}

    //public void Populate(ElectricDevice electricDevice)
    //{
    //    selectedDataNode = electricDevice;

    //    selectedElectricDevice = electricDevice;

    //    title.text = electricDevice.name;
    //    sensordata.text = " ";
    //    onButton.gameObject.SetActive(true);
    //    offButton.gameObject.SetActive(true);

    //    onButton.onClick.RemoveAllListeners();
    //    offButton.onClick.RemoveAllListeners();
    //    onButton.onClick.AddListener(delegate { electricDevice.On(); });
    //    offButton.onClick.AddListener(delegate { electricDevice.Off(); });

    //    canvas.enabled = true;
    //    GetComponent<GraphicRaycaster>().enabled = true;
    //}

    //public void DeselectElectricDevice()
    //{
    //    selectedElectricDevice = null;
    //}

    public void DeselectDevice()
    {
        selectedDataNode = null;
    }

    void DisableCanvas()
    {
        canvas.enabled = false;
        rayCaster.enabled = false;
    }
}
