using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class injectNewTitle : MonoBehaviour {

    public string newTitle = "My new title";
    private string title;
    int i = 1;
    private GameTime gameTime;

    void Start()
    {
        title = GameObject.Find("TitleText").GetComponent<titleTextController>().newTitle;
        gameTime = GameObject.Find("GameTime").GetComponent<GameTime>();

    }

    public void injectNewtitle() {

        title = newTitle + " " + i;
        GameObject.Find("TitleText").GetComponent<titleTextController>().updateTitle("FadeOutIn", title);
        gameTime.JumpTo(1515672001);
        i++;

    }
}
