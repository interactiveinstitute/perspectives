using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxFollowMainCamera : MonoBehaviour {

    [SerializeField]
    Transform camTransform;

    private Transform skyboxesTransform;

	// Use this for initialization
	void Start () {
        if (!camTransform)
        {
            camTransform = Camera.main.transform;
        }
        skyboxesTransform = transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        skyboxesTransform.rotation = camTransform.rotation;
	}
}
