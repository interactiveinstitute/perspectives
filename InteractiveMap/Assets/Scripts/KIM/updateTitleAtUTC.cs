using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class updateTitleAtUTC : MonoBehaviour {

    public string eventName;
    [TextArea]
    public string eventDescription;
    [Space(20)]

    public bool updateTitle;
    public int updateTime;
    private GameTime gameTime;

    // Use this for initialization
    void Start () {

        gameTime = GameObject.Find("GameTime").GetComponent<GameTime>();

    }
	
	// Update is called once per frame
	void Update () {

        if (updateTitle && gameTime.time > updateTime)
        {
            string newTitle = eventName + "\n<size=12>" + eventDescription + "</size>";
            GameObject.Find("TitleText").GetComponent<titleTextController>().updateTitle("FadeOutIn", newTitle);
            updateTitle = false;
        }

    }
}
