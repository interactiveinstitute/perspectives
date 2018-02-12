using UnityEngine;
using UnityEngine.UI;

public class SensorDevicesController : MonoBehaviour {

    public DeviceCanvasController canvasController;
    public CameraController cameraController;

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


    public void Select(SensorDevice device)
    {
        if (selectedDevice != device) // A new device will be selected
        {
            if (selectedDevice)
            {
                Deselect();
            }

            if (cameraController.MoveTo(device.transform))
            {
                device.Select();

                canvasController.Populate(device);
                selectedDevice = device;
            }

            
        }
        else { // The selected device is already selected, so deselect it
            Deselect();
            
        }

        //if (selectedDevice == null)
        //{
        //    device.Select();

        //    canvasController.Populate(device);
        //    selectedDevice = device;

        //    cameraController.MoveTo(device.transform);
        //}
        //else if (selectedDevice != device)
        //{
        //    Deselect();

            
        //}
        //else if (selectedDevice == device)
        //{
        //    Deselect();
        //}
    }

    private void Deselect()
    {
        selectedDevice.Deselect();
        selectedDevice = null;
        canvasController.DeselectDevice();
        cameraController.StopFollow();
        GameObject.Find("TitleText").GetComponent<titleTextController>().updateTitle("FadeOutIn", "");
    }
}
