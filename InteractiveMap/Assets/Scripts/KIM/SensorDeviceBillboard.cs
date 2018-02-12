using UnityEngine;
using System.Collections;

public class SensorDeviceBillboard : MonoBehaviour {
	public bool isActive = true;

    [Header("Constant scale")]
    public bool constantScale;
    public float scale;

    [Tooltip("If the distance is closer than this, then stop scaling transform (if constantScale is true)")]
    public float minDistanceFixedScale = 50.0f;

    private Camera mainCamera;

	// Use this for initialization
	void Start () {
        mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {

		if(isActive)
            transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);

        if (constantScale) {

            float distance = Vector3.Distance(mainCamera.transform.position, transform.position);
            if (distance > minDistanceFixedScale)
            {
                transform.localScale = scale * distance * Vector3.one;
            }
            else
            {
                transform.localScale = scale * minDistanceFixedScale * Vector3.one;
            }
        }
    }
}
