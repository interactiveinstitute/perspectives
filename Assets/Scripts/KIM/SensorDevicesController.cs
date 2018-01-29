using UnityEngine;
using UnityEngine.UI;

public class SensorDevicesController : MonoBehaviour {

    public DeviceCanvasController canvasController;

    //public SensorDeviceCanvas canvas;

    public SensorDevice selectedDevice = null;

    public Text nameText;
    public Text descriptionText;


    // Use this for initialization
    void Start () {

    }


    // Update is called once per frame
    void Update() {

    }


    public void Clicked(SensorDevice device)
    {
        canvasController.Populate(device);
        selectedDevice = device;
    }

    public void Deselect()
    {
        selectedDevice = null;
        canvasController.DeselectDevice();
    }
}
