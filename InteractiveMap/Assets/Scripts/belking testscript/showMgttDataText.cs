using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
//using TimeSeries;

public class showMgttDataText : DataNode {

	private int SubpropertyId = 0;
	private string Unit = "";
	private int decimals = 0;
	public double scale = 1;
	private double value;

	public Text MqttDataText;
	private String MqttDataInput;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		MqttDataText.text = MqttDataInput + " W";
		
	}




	override public void TimeDataUpdate(Subscription Sub, DataPoint data) {

		string newtext = "";

		//if (NodeName == "Heating Energy")
		//	print ("Heating Energy data recived");

		if (data == null)
			return;

		if (data.Values == null)
			return;

		if (SubpropertyId >= data.Values.Length)
			return; 

		//Debug.Log (data.Values.Length);
		//Debug.Log (SubpropertyId);
		if (data.Values [SubpropertyId] != null) {

			newtext = Math.Round(data.Values[SubpropertyId]/scale,decimals).ToString() + " " + Unit;

			MqttDataInput = newtext;

			value = data.Values [SubpropertyId] / scale;







			// print (value);

		}



	}
}
