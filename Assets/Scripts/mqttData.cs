using UnityEngine;
using System.Collections;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using UnityEngine.UI;
using System;

	public class mqttData : MonoBehaviour {

		private MqttClient client;

		public Text mspPowerText;
		//public Text mspPercentText;
		public String mspPowerTopic = "/Testsites/MSP/power";
		public Transform powerDial;

		
		public static float mspPowerFloat;
		private int mspPowerInt;
		private String mqttMsg;
		private String mqttTopic;
		private String mspPower;
		private String mspPercent;
		private String mspLabel = "<size=16>Munktell Science Park<br></size>";

		// public String mqttServerAdress = "213.168.249.129";

		// Use this for initialization
		void Start () {


			//var euler = transform.eulerAngles;
			//euler.z = Random.Range(0.0, 360.0);
			//powerDial.transform.eulerAngles = euler;	
			
			// create client instance 
			client = new MqttClient(IPAddress.Parse("213.168.249.129"),1883 , false , null ); 

			// register to message received 
			client.MqttMsgPublishReceived += client_MqttMsgPublishReceived; 

			string clientId = Guid.NewGuid().ToString(); 
			client.Connect(clientId);

			// subscribe to the topic mspPowerTopic with QoS 2 
		client.Subscribe(new string[] { "/Testsites/MSP/power" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
		client.Subscribe(new string[] { "/open/mspPowerFloat" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }); 

		}


		void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) { 

			// Debug.Log("Topic: " + e.Topic);
			// Debug.Log("Message: " + e.Message);

			if (e.Topic == "/Testsites/MSP/power") {
				mqttMsg = System.Text.Encoding.UTF8.GetString(e.Message);
				mspPower = mqttMsg;
			}

			if (e.Topic == "/open/mspPowerFloat"){
				String s = System.Text.Encoding.UTF8.GetString(e.Message);
				mspPowerFloat = float.Parse(s);
			}

		} 


		// Update is called once per frame
		void Update () {
			mspPowerText.text = mspPower + " W";

			//mspPercentText.text = mspPercent + " %";
			//int randomNumber = UnityEngine.Random.Range(0, -180);
			//powerDial.euler.z = UnityEngine.Random.Range(0.0, 360.0);
			//powerDial.transform.eulerAngles = new Vector3(0,0,myRotation);
			//powerDial.transform.Rotate = new Vector3(0.0f,0.0f,randomNumber);
		}
	}

