using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class titleTextController : MonoBehaviour {

    private Text titleText;
    private GameTime gameTime;
    private Animator animator;
    public string newTitle = "title";
    private string oldTitle = "title";
    public bool isNewTitle =  false;

    // Use this for initialization
    void Start () {

        Text title = GetComponent<Text>();
        gameTime = GameObject.Find("GameTime").GetComponent<GameTime>();
        titleText = GetComponent<Text>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update() {

            if (isNewTitle) { 
                titleText.text = newTitle;
                oldTitle = newTitle;
                isNewTitle = false;
            }

        //titleText.text = gameTime.TimestampToDateTime(gameTime.time).ToString("dd MMMM").ToUpper() + "\n" + "sdfsdfsdf";
        // gameTime.TimestampToDateTime(gameTime.time).ToString("dd . mm");
    }

    public void updateTitle(string animName, string title ) {
        newTitle = title.ToUpper();
        animator.Play(animName, -1, 0f);
        isNewTitle = false;
    }
}
