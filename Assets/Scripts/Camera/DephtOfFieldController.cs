using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class DephtOfFieldController : MonoBehaviour {

    public CameraController cameraController;
    public PostProcessingBehaviour behaviour;

    private PostProcessingProfile profile;

    public bool automaticDistance = true;
    public float focusDistance;
    private float realDistance;

    [Range(0.1f, 32.0f)]
    public float aperture;

    public bool distanceBasedAperture = true;

    public AnimationCurve apertureByDistanceCurve;

    public bool useCameraFov;

    [Range(1f, 300f)]
    public float focalLength;

    public bool distanceBasedFocalLength = true;

    public AnimationCurve focalLengthByDistanceCurve; 

    [Range(0f, 1f)]
    public float distanceInterpolator = 0.5f;

    private DepthOfFieldModel.Settings settings;

	// Use this for initialization
	void Start ()
    {
        profile = Instantiate(behaviour.profile);
        behaviour.profile = profile;
        settings = profile.depthOfField.settings;

        focusDistance = settings.focusDistance;
        aperture = settings.aperture;
        focalLength = settings.focalLength;
        useCameraFov = settings.useCameraFov;
	}
	
	// Update is called once per frame
	void Update () {
        if (automaticDistance)
        {
            focusDistance = Vector3.Magnitude(cameraController.cam.transform.position - cameraController.interactionPoint);
        }
        //realDistance = Mathf.Lerp(realDistance, distance, distanceInterpolator);
        float diff = realDistance - focusDistance;
        realDistance -= diff * distanceInterpolator;

        settings.focusDistance = realDistance;

        if (distanceBasedAperture)
        {
            aperture = apertureByDistanceCurve.Evaluate(realDistance);
        }
        settings.aperture = aperture;
        settings.useCameraFov = useCameraFov;

        if (distanceBasedFocalLength)
        {
            focalLength = focalLengthByDistanceCurve.Evaluate(realDistance);
        }
        settings.focalLength = focalLength;

        profile.depthOfField.settings = settings;
	}
}
