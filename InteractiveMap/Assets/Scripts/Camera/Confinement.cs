using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confinement : MonoBehaviour {

    //public Transform confinedTransform;
    [SerializeField]
    private CameraController cameraController;

    //private Vector3 closestPointToConfinement;
    private bool isInside;

    private BoxCollider box;

	// Use this for initialization
	void Start () {
        box = GetComponent<BoxCollider>();
        if (box == null)
        {
            Debug.LogError("No box collider component");
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (!isInside)
        {
            Vector3 closestPointToConfinement = box.ClosestPoint(cameraController.pivot.position);
            //Debug.Log("closestPointToConfinement: " + closestPointToConfinement);
            cameraController.SetTargetWorldLocation(closestPointToConfinement);
        }
	}

    void OnTriggerExit(Collider other)
    {
        if (other.transform.GetInstanceID() == cameraController.pivot.GetInstanceID())
        {
            isInside = false;
            Vector3 closestPointToConfinement = box.ClosestPoint(cameraController.pivot.position);
            //Debug.Log("closestPointToConfinement: " + closestPointToConfinement);
            cameraController.SetTargetWorldLocation(closestPointToConfinement);


            //GameObject go = Instantiate(new GameObject("exitPoint"));
            //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //go.transform.localScale = new Vector3(40, 40, 40);
            //go.transform.localPosition = closestPointToConfinement;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetInstanceID() == cameraController.pivot.GetInstanceID())
        {
            isInside = true;
        }
    }
}
