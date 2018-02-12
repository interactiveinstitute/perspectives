using UnityEngine;
using System.Collections;
 
[ExecuteInEditMode]
public class SkyboxMix : MonoBehaviour {
 
    [Range(0f,1f)]
    public float blend;
    RenderTexture skyboxRenderTexture;
    private Material material;

    public bool lightInclinationBlend = true;
    public Light light;

    [Tooltip("X-axis: Light inclination. 0 degrees is horizontal and 90 degrees is straight from above. \n Y-axis: Blend between 0, first skybox, and 1, second skybox.")]
    public AnimationCurve blendByLightInclinationCurve;

    [Tooltip("The other camera that renders a skybox into a render texture, that will be mixed with this one.")]
    public Camera skyboxCamera;
 
    // Creates a private material used to the effect
    void Awake ()
    {
        material = new Material( Shader.Find("Hidden/SkyboxMix") );

        int width = skyboxCamera.pixelWidth;
        int height = skyboxCamera.pixelHeight;
        skyboxRenderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        skyboxCamera.targetTexture = skyboxRenderTexture;
    }

    void Start()
    {
        if (lightInclinationBlend && !light)
        {
            Light[] lights = Light.GetLights(LightType.Directional, LayerMask.NameToLayer("Everything"));

            if (lights.Length > 0)
            {
                light = lights[0];
            }
            else
            {
                Debug.LogWarning("Couldn't retrieve a directional light for calculating skybox mixing");
                lightInclinationBlend = false;
            }
        }
    }
 
    // Postprocess the image
    void OnRenderImage (RenderTexture source, RenderTexture destination)
    {
        if (lightInclinationBlend)
        {
            Vector3 lightDir = light.transform.forward;
            float inclinationAngle = Vector3.Angle(lightDir, Vector3.up) - 90f; // angle is  zero when sun is from the side and 90 from straight above
            blend = blendByLightInclinationCurve.Evaluate(inclinationAngle);
        }
        
        material.SetFloat("_blend", blend);
        material.SetTexture("_SkyboxTex", skyboxRenderTexture);
        Graphics.Blit(source, destination, material);
    }
}