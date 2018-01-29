using UnityEngine;
using UnityEngine.UI;

public class SensorDeviceCanvas : MonoBehaviour {

    public Text title;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Populate(SensorDevice device)
    {
        //Debug.Log("Populate canvas with : " + electricDevice.gameObject.name);

        title.text = device.name;

    }

}
