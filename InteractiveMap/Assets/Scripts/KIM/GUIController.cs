using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {

	Text timeText;
	Text dateText;
	//double now = GameTime.GetInstance().time;
	GameTime myTime = new GameTime();


	// Use this for initialization
	void Start () {

		timeText = GameObject.Find("timeText").GetComponent<Text>();
		dateText = GameObject.Find("dateText").GetComponent<Text>();
		//double now = GameTime.GetInstance().time;
		//myTime = 
	}

	// Update is called once per frame
	void Update () {
		//HH:mm:ss
		double now = GameTime.GetInstance().time;
		timeText.text = GameTime.GetInstance().TimestampToDateTime(now).ToString("HH:mm:ss");
		dateText.text = GameTime.GetInstance().TimestampToDateTime(now).ToString("dd MMM yyyy");
		}
}
