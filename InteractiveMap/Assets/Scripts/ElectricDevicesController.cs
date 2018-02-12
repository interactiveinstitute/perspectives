using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricDevicesController : MonoBehaviour {

    public DeviceCanvasController canvasController;

    public CameraController cameraController;

    public ElectricDevice[] devices;

    public ElectricDevice selectedDevice = null;

	// Use this for initialization
	void Start () {

        int numChildren = transform.childCount;

        devices = new ElectricDevice[numChildren];

        for (int i = 0; i < transform.childCount; i++)
        {
            devices[i] = transform.GetChild(i).GetComponent<ElectricDevice>();
        }
	}

    public void Select(ElectricDevice device)
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
        else
        { // The selected device is already selected, so deselect it
            Deselect();

        }
    }

    private void Deselect()
    {
        selectedDevice.Deselect();
        selectedDevice = null;
        canvasController.DeselectDevice();

        cameraController.StopFollow();
    }
}
