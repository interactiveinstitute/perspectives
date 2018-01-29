using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class energyControlGui : MonoBehaviour
{
    //ElectricDeviceInteraction[] electricDeviceInteractions;
    public ElectricDevicesController devicesController;

	Text powertext;

	// Use this for initialization
	void Start () {

		powertext = this.GetComponentInChildren<Text>();
		powertext.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		
		powertext.text = "";

        for (int i = 0; i < devicesController.devices.Length; i++)
        {
            ElectricDevice device = devicesController.devices[i];

            if (devicesController.selectedDevice == device)
            {
                powertext.text += "<b>" + device.name + " Power:" + " " + device.Power.ToString() + " kW" + "</b> " + "\n";
            }
            else
            {
                powertext.text += device.name + " Power: " + " " + device.Power.ToString() + " kW" + "\n";
            }
        }
	}
}
