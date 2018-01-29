using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DeviceCanvasController : MonoBehaviour {

    public Text title;
	public Text sensordata;
    //public Button onButton;
    //public Button offButton;

    double lastTimestamp;

    public Canvas canvas;

    //private ElectricDevice selectedElectricDevice = null;
    //private SensorDevice selectedSensorDevice = null;

    private DataNode selectedDataNode = null;

	// Use this for initialization
	void Start () {
        canvas = GetComponent<Canvas>();
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
                if (lastTimestamp != dp.Timestamp)
                {
                    string titleText = selectedDataNode.name + "\n";
                    titleText += "<size=10> Updated " + GameTime.GetInstance().TimestampToDateTime(dp.Timestamp).ToString("HH:mm:ss") + "</size>\n \n";

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
                        sensorText +=   dp.Values[i] + "\n";
                    }
                    sensordata.text = sensorText;
                    title.text = titleText;

                    lastTimestamp = dp.Timestamp;
                }
            }
        }
	}


    public void Populate(DataNode dataNode)
    {
        selectedDataNode = dataNode;

        //title.text = dataNode.name + "\n " + "<size=8>Updated: " + GameTime.GetInstance().TimestampToDateTime(dataNode.Values[0]).ToString("HH:mm:ss") + "</size>\n \n";
        //sensordata.text = "temperature: " + sensorDevice.temperature.ToString() + "\n" + "Humidity: " + sensorDevice.humidity.ToString() + "\n" + "Decibel: " + sensorDevice.decibel.ToString();

        sensordata.text = "wait for data...";

        //onButton.onClick.RemoveAllListeners();
        //offButton.onClick.RemoveAllListeners();

        //onButton.gameObject.SetActive(false);
        //offButton.gameObject.SetActive(false);

        canvas.enabled = true;
        GetComponent<GraphicRaycaster>().enabled = true;
    }

    private void Populate(ElectricDevice electricDevice)
    {

    }

    private void Populate(SensorDevice sensorDevice)
    {

    }

    public void Close()
    {
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

    public void DisableCanvas()
    {
        canvas.enabled = false;
        GetComponent<GraphicRaycaster>().enabled = false;
    }
}
