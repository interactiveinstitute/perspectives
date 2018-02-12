using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;


public class MouseClick : MonoBehaviour
{

	public Texture2D cursorTexture;
	CursorMode cursorMode = CursorMode.Auto;
	Vector2 hotSpot = Vector2.zero;

	Runlevel runlevelOn;
	Runlevel runlevelOff;

	float onPower;
	float offPower;
	Text powertext;
	Button[] powerbutton;
	public GameObject houseinfo;


	void Start () {

		powerbutton = houseinfo.GetComponentsInChildren<Button>();
		powertext = houseinfo.GetComponentInChildren<Text>();
		runlevelOn = this.GetComponentInParent<ElectricDevice> ().runlevels[2];
		runlevelOff = this.GetComponentInParent<ElectricDevice> ().runlevels[1];

	

	
	}
	void Update()
	{
		onPower = runlevelOn.Power;
		offPower = runlevelOff.Power;
	}



	void OnMouseDown ()
	{
		houseinfo.SetActive(true);
		powerbutton[0].onClick.AddListener(turnOffBuildning);
		powerbutton[1].onClick.AddListener(turnOnBuildning);
		powerbutton[2].onClick.RemoveListener(turnOnBuildning);
		powerbutton[2].onClick.RemoveListener(turnOnBuildning);
		powerbutton[2].onClick.AddListener(closeMenu);

	}

	void turnOffBuildning (){

		this.GetComponentInParent<ElectricDevice> ().Off ();

	}

	void turnOnBuildning (){

		this.GetComponentInParent<ElectricDevice> ().On ();
	}

	void closeMenu (){

		houseinfo.SetActive(false);
	}

	void OnMouseEnter()
	{
		Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
		powertext.text = "On = "+ onPower.ToString() + " Standby = "+ offPower.ToString();
	}

	void OnMouseExit()
	{
		Cursor.SetCursor(null, Vector2.zero, cursorMode);
		powertext.text = "";

	}

}
