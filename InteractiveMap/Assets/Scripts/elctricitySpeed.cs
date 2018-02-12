
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class elctricitySpeed : MonoBehaviour
{
	private Animator anim;
	public Slider slider;   //Assign the UI slider of your scene in this slot 

	// Use this for initialization
	void Start()
	{
		anim = GetComponent<Animator>();

	}

	// Update is called once per frame
	void Update()
	{
		// anim.Play("cable_el");
		anim.speed = slider.value;
		// anim.Play("cable_el", -1, slider.normalizedValue);
	}
}