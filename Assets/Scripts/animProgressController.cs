using UnityEngine;

[RequireComponent(typeof(Animator))]
public class animProgressController : MonoBehaviour
{

    Animator animator;
    public bool changeTitleAtStartTime;
    public string animName;
    [TextArea]
    public string animDescription;
    [Space (20)]
    public string animStateName;
    public float animDuration;

    private GameTime gameTime;

    public bool useSlider = false;

    [Range(0f,100f)]
    [SerializeField]
    private double animProgress;    // Progress of the animation

    private double animProgPerSec;  // How much does the animaion progress per second
    private double animTimeSpan;    // The lenght of the animation in seconds

    [Header("Start and end of animation")]
    [Tooltip("UTC animation start, calculate www.unixtimestamp.com")]
    public int startTime;
    [Tooltip("UTC animation end, calculate www.unixtimestamp.com")]
    public int endTime;


    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = 1f;

        gameTime = GameObject.Find("GameTime").GetComponent<GameTime>();

        if (endTime <= startTime) {
            startTime = (int)gameTime.time;
            endTime = startTime + 3600; // add 1 hour in secs
        }

        animTimeSpan = endTime - startTime;
        animProgPerSec = animDuration / animTimeSpan;
    }


    // Update is called once per frame
    void Update()
    {
             
        if (useSlider == false)
        {
            animProgress = animDuration - ((endTime - gameTime.time) * animProgPerSec);            
        }

        animator.PlayInFixedTime(animStateName, -1, (float)animProgress);
        
        if (animProgress > animDuration) {
            animProgress = animDuration;
        } else if (animProgress < 0.0f) {
            animProgress = 0.0f;
        }

        if (changeTitleAtStartTime && gameTime.time > startTime ) {
            string newTitle = animName + "\n <size=12>" + animDescription + "</size>";
            GameObject.Find("TitleText").GetComponent<titleTextController>().updateTitle("FadeOutIn", newTitle);
            changeTitleAtStartTime = false;
        }

    }


}