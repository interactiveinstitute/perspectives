using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TimeSlider : MonoBehaviour {

    static float NumDaysPerYearAvg = 365.25f;

    public static DateTime ConvertFromUnixTimestamp(double timestamp)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return origin.AddSeconds(timestamp);
    }

    public static double ConvertToUnixTimestamp(DateTime date)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan diff = date.ToUniversalTime() - origin;
        return Math.Floor(diff.TotalSeconds);
    }

    public ScrollRect scroll;
    public RectTransform timeScrollTransform;
    public RectTransform gridTransform;
    public RectTransform image1Transform;
    public RectTransform image2Transform;

    public Text indicatorDateTimeText;
    public Text indicatorGameTimeDebugDateTimeText;

    [System.Serializable]
    public class SliderDateTime
    {
        public int year;
        public int month;
        public int day;
        public int hour;
        public int minute;
        public int second;
    }
    public SliderDateTime startSliderDateTime;
    DateTime startDateTime;

    public GameTime gameTime;

    public AnimationCurve moveToCurve;

    float totalProgress;
    float lastTotalProgress;

    float im1StartPosX;

    float timeSliderWidth;
    float imageWidth;

    RectTransform currentImage;
    //public RectTransform image2Transform;

	// Use this for initialization
	void Start () {
        timeSliderWidth = timeScrollTransform.rect.width;
        imageWidth = image1Transform.rect.width;

        //Debug.Log("timeSliderTransform witdth: " + timeSliderWidth);
        //Debug.Log("gridTransform witdth: " + gridTransform.rect.width);
        //Debug.Log("image1Transform witdth: " + imageWidth);
        //Debug.Log("imageWidth: " + imageWidth);
        //Debug.Log("timeSliderWidth: " + timeSliderWidth);
        //Debug.Log("image2Transform witdth: " + image2Transform.rect.width);

        im1StartPosX = image1Transform.localPosition.x;

        startDateTime = new DateTime(startSliderDateTime.year, startSliderDateTime.month, startSliderDateTime.day, startSliderDateTime.hour, startSliderDateTime.minute, startSliderDateTime.second);

        //Debug.Log("UTC: " + ConvertToUnixTimestamp( startDateTime ) );

        gameTime.JumpTo(ConvertToUnixTimestamp(startDateTime));
        //gameTime.SetStartTime(ConvertToUnixTimestamp(startDateTime));
    }
	
	// Update is called once per frame
    void Update()
    {
        float gridPos = gridTransform.localPosition.x;
        //gridTransform.localPosition = new Vector3(gridTransform.localPosition.x + 0.01f, gridTransform.localPosition.y, gridTransform.localPosition.z);

        totalProgress = -gridPos / imageWidth;



        float im1Pos = Mathf.Floor(totalProgress) * imageWidth;

        image1Transform.localPosition = new Vector3(im1Pos + im1StartPosX, image1Transform.localPosition.y, image1Transform.localPosition.z);

        float currentYearProgress = totalProgress - Mathf.Floor(totalProgress);

        // Move the second images before or after the current one
        if (currentYearProgress >= 0.5f)
        {
            image2Transform.localPosition = image1Transform.localPosition + new Vector3(image1Transform.rect.width, 0f, 0f);
        }
        else
        {
            image2Transform.localPosition = image1Transform.localPosition - new Vector3(image1Transform.rect.width, 0f, 0f);
        }

        lastTotalProgress = totalProgress;

        int yearIndex = Mathf.FloorToInt(totalProgress);
        int year = yearIndex + startDateTime.Year;

        int numDaysClickedYear = 365;
        if (year % 4 == 0)
        {
            numDaysClickedYear++;
        }

        float daysProgressf = totalProgress * numDaysClickedYear;
        int daysProgressi = Mathf.FloorToInt(daysProgressf);

        DateTime currentDateTime = startDateTime.AddDays(daysProgressi);
        gameTime.JumpTo(ConvertToUnixTimestamp(currentDateTime));

        indicatorDateTimeText.text = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");

        DateTime gameTimeDateTime = ConvertFromUnixTimestamp(gameTime.time);

        indicatorGameTimeDebugDateTimeText.text = gameTimeDateTime.ToString("yyyy-MM-dd HH:mm:ss");

	}

    void MoveTo(DateTime dateTime)
    {
        TimeSpan span = dateTime - startDateTime;
        float spanProgress = span.Days / NumDaysPerYearAvg;
        float newGridPos = -(spanProgress ) * imageWidth;
        //Debug.Log("span.Days: " + span.Days);
        //Debug.Log("spanProgress: " + spanProgress);

        //Vector3 gridPos = gridTransform.localPosition;
        //gridPos.x = newGridPos;
        //gridTransform.localPosition = gridPos;


        StartCoroutine(MoveGrid(newGridPos));
    }

    void MoveTo(float progress)
    {

    }

    IEnumerator MoveGrid( float endPosX )
    {
        //Debug.Log("MoveGrid: " + endPosX);
        Vector3 startPos = gridTransform.localPosition;
        Vector3 endPos = startPos;
        endPos.x = endPosX;
        //Debug.Log("StartPos, EndPos: " + startPos + ", " + endPosX);


        float t = moveToCurve.keys[0].time;
        float t1 = moveToCurve.keys[moveToCurve.length-1].time;
        //Debug.Log("Length: " + (t1 - t ) );

        while (t < t1)
        {
            float ip = moveToCurve.Evaluate(t);
            //Debug.Log("ip: " + ip);
            gridTransform.localPosition = Vector3.Lerp(startPos, endPos, ip);

            //Debug.Log("x: " + gridTransform.localPosition.x);

            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

    }

    public void MouseClick(BaseEventData baseEventData)
    {
        PointerEventData pointerEvent = baseEventData as PointerEventData;

        float clickDistance = Vector2.Distance(pointerEvent.pressPosition, pointerEvent.position);

        if (clickDistance < 10f)
        {
            Vector2 localCursor;
            bool conversionOK = RectTransformUtility.ScreenPointToLocalPointInRectangle(timeScrollTransform, pointerEvent.position, pointerEvent.pressEventCamera, out localCursor);

            if (!conversionOK)
            {
                Debug.Log("Rect conversion NOT OK!");
            }

            float xClick = localCursor.x;
            float xClickNormalized = xClick / timeSliderWidth;

            float timeSliderProgressSpan = timeSliderWidth / imageWidth;
            float clickProgressDiff = xClickNormalized * timeSliderProgressSpan;

            float clickProgress = totalProgress + clickProgressDiff;

            int yearIndex = Mathf.FloorToInt(clickProgress);

            int year = yearIndex + startDateTime.Year;

            int numDaysClickedYear = 365;
            if (year % 4 == 0)
            {
                numDaysClickedYear++;
            }

            float daysProgress = clickProgress * numDaysClickedYear;

            int daysInt = Mathf.FloorToInt(daysProgress);

            DateTime clickedDateTime = startDateTime.AddDays(daysInt);

            //Debug.Log("Clicked datetime: " + clickedDateTime.ToString("yyyy-MM-dd") + " days added: " + daysInt + " (num days this year: " + numDaysClickedYear + ")" + " clickProgress: " + clickProgress);

            MoveTo(clickedDateTime);
        }
    }
}
