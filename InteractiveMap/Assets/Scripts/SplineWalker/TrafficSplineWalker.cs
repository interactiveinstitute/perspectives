using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
//using TimeSeries;


public class TrafficSplineWalker : MonoBehaviour {

	public BezierSpline spline;
	public float time;
	private float duration;
	public float count;
	private float ratio;
	public GameObject[] prefabs;
	private GameObject prefab;
	public Transform instanceholder;


	public float speed;


	private int SubpropertyId = 0;
	private string Unit = "";
	private int decimals = 0;
	// public double scale = 1;
	private double value;


	private List<Transform> transformList = new List<Transform>();
	public bool lookForward;
	public SplineWalkerMode mode;
	public float progress;
	private bool goingForward = true;



	void Start()
	{


		float ratio = 1/count;
		Transform t;

		for (int i = 0; i < count; i++)
		{
			int randomvehicle =  UnityEngine.Random.Range (0, prefabs.Length);

			t = Instantiate (prefabs[randomvehicle].transform, new Vector3 (), Quaternion.identity, instanceholder);
			transformList.Add(t);
	//		progress = progress + ratio;
	//		t.position = spline.GetPoint (progress);
	//		transform.localPosition = t.position;

		}

	}


	public void Update () {
		
		time = duration / count;



		foreach (Transform element in transformList) {

			// duration = mqttData.mspPowerFloat*count;
			// convert double to float


			duration = speed*count;

			ratio = 1/count;
			progress = progress + ratio;

			// Debug.Log("test " + mqttData.mspPowerFloat);
			if (duration > 0) {

				if (goingForward) {
					progress += Time.deltaTime / duration;
					if (progress > 1f) {
						if (mode == SplineWalkerMode.Once) {
							progress = 1f;
						} else if (mode == SplineWalkerMode.Loop) {
							progress -= 1f;
						} else {
							progress = 2f - progress;
							goingForward = false;
						}
					}
				} else {
					progress -= Time.deltaTime / duration;
					if (progress < 0f) {
						progress = -progress;
						goingForward = true;
					}
				}

				element.position = spline.GetPoint (progress);
				transform.localPosition = element.position;
				if (lookForward) {
					element.LookAt (element.position + spline.GetDirection (progress));
					Debug.Log (spline.GetDirection (progress));
				}


			}

		}

	}
	/* override public void TimeDataUpdate(Subscription Sub, DataPoint data) {

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

			 value = data.Values [SubpropertyId] / scale;







			// print (value);

		}



	}
	*/

}