using UnityEngine;
using System.Collections;

public class CameraFacingBillboard : MonoBehaviour
{
    Camera mainCamera;


    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
            mainCamera.transform.rotation * Vector3.up);
    }
}