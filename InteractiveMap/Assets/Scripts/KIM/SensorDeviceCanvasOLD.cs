using UnityEngine;
using UnityEngine.UI;

public class SensorDeviceCanvasOLD : MonoBehaviour {

    public GameObject SensorDeviceUi;
    public GameObject SensorDevicePanel;
    GameObject[] sensorDevices;

    Text tempText;
    Text humText;
    Text dbText;

    private int index = 0;
    private int sensorDeviceCount = 0;
    private GameObject myGameObject;

    void Start()
    {
        
        sensorDevices = GameObject.FindGameObjectsWithTag("SensorDevice");

        foreach (GameObject sensordevice in sensorDevices)
        {
            Debug.Log("temperature: " + sensordevice.GetComponent<SensorDevice>().temperature);

            var newSensorDevice = Instantiate(SensorDeviceUi, transform.position, Quaternion.identity);
            newSensorDevice.transform.parent = gameObject.transform;
            newSensorDevice.name = "SensorDevicePanel" + index;

            tempText = GameObject.Find("SensorDevicePanel" + index + "/tempText").GetComponent<Text>();
            humText = GameObject.Find("SensorDevicePanel" + index + "/humText").GetComponent<Text>();
            dbText = GameObject.Find("SensorDevicePanel" + index + "/dbText").GetComponent<Text>();

            //tempText.text = sensordevice.GetComponent<SensorDevice>().temperature.ToString();

            index++;            
        }

        sensorDeviceCount = index;
        Debug.Log(sensorDevices.Length);
    }

    /*
     * 
    void Update() {

        for(index = 0; index < sensorDeviceCount; index++) {
            
            myGameObject = sensorDevices[index];
        
            tempText = GameObject.Find("SensorDevicePanel" + index + "/tempText").GetComponent<Text>();
            humText = GameObject.Find("SensorDevicePanel" + index + "/humText").GetComponent<Text>();
            dbText = GameObject.Find("SensorDevicePanel" + index + "/dbText").GetComponent<Text>();

            tempText.text = "temp: " + sensorDevices[index].GetComponent<SensorDevice>().temperature;
            humText.text = "hum: " + sensorDevices[index].GetComponent<SensorDevice>().humidity;
            dbText.text = "decibel: " + sensorDevices[index].GetComponent<SensorDevice>().decibel;

            index++;
        }

    }

    */
}
