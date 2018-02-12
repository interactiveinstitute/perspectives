using UnityEngine;

public class ElectricDeviceEventHandler : MonoBehaviour
{

    //public CameraController camController;

    //public float decibel = 0.0f;
    //public bool isInFocus = false;
    //public bool isSelected = false;
    //SpriteRenderer focusIndicator;

    //Color myColor;

    public ElectricDevicesController devicesController;
    public ElectricDevice device;

    void Start()
    {
        if (devicesController == null)
        {
            devicesController = GetComponentInParent<ElectricDevicesController>();
        }
        if (device == null)
        {
            device = GetComponentInParent<ElectricDevice>();
        }
    }

    void OnMouseOver()
    {
        device.StartHover();
    }

    void OnMouseExit()
    {
        device.StopHover();
    }

    void OnMouseDown()
    {
        devicesController.Select(device);
    }
}
