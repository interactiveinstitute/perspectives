using UnityEngine;
using System.Collections;

public class Billboard_wc : MonoBehaviour {
	public bool isActive = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(isActive)
			transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }
}
