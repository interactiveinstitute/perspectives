using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricDevicesController : MonoBehaviour {

    public DeviceCanvasController canvasController;

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

    // Update is called once per frame
    void Update()
    {
        //if (!selectedDevice && canvasController.canvas.enabled)
        //{
        //    canvasController.DisableCanvas();
        //}
	}

    public void Clicked(ElectricDevice device)
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
