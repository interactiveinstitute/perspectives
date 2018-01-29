using UnityEngine;

public class SensorDeviceEventHandler : MonoBehaviour {

    public CameraController camController;

    public float decibel = 0.0f;
    public bool isInFocus = false;
    SpriteRenderer focusIndicator;
    Color myColor;

    public SensorDevicesController deviceController;
    public SensorDevice device;

    void Start() {

        focusIndicator = transform.GetChild(0).GetComponent<SpriteRenderer>();
        focusIndicator.enabled = false;
        myColor = GetComponent<SpriteRenderer>().color;

        if (deviceController == null)
        {
            deviceController = GetComponentInParent<SensorDevicesController>();
        }
        if (device == null)
        {
            device = GetComponentInParent<SensorDevice>();
        }
    }


    void OnMouseOver()
    {
        focusIndicator.enabled = true;

        if (Input.GetMouseButtonDown(0))
        {
            
            if (camController)
            {
                camController.MoveTo(transform);
            }

            deviceController.Clicked(device);

            if (isInFocus == false) {
                isInFocus = true;                            
            }

            if (isInFocus == true) {
                isInFocus = false;
            }
        }
    }

    void OnMouseExit()
    {
        if (isInFocus == false) { 
            focusIndicator.enabled = false;
        }

        //The mouse is no longer hovering over the GameObject so output this message each frame
        //Debug.Log("MouseExited GameObject.");
    }


}
