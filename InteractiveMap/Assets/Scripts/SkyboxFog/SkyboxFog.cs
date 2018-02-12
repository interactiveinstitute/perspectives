using UnityEngine;
using System.Collections;
 
[ExecuteInEditMode]
public class SkyboxFog : MonoBehaviour {
 
    // just a comment

    [Range(0f,1f)]
    public float intensity;
    RenderTexture skyboxRenderTexture;
    private Material material;


    [Tooltip("The camera that renders the skybox into a render texture")]
    public string cameraSearchString = "Camera Second Skybox";

    public Camera skyboxCamera;

    public float blendStart = 200.0f;
    public float blendEnd = 2000.0f;

    //public Shader skyboxFogShader;
 
    // Creates a private material used to the effect
    void Awake ()
    {
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;

        Shader skyboxFogShader = Shader.Find("Hidden/SkyboxFog");
        if (skyboxFogShader == null)
        {
            Debug.Log("Couldn't find skybox fog shader");
        }
        material = new Material(skyboxFogShader);

        

        if (!skyboxCamera)
        {
            skyboxCamera = GameObject.Find(cameraSearchString).GetComponent<Camera>();
        }

        if (skyboxCamera)
        {
            int width = skyboxCamera.pixelWidth;
            int height = skyboxCamera.pixelHeight;
            skyboxRenderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
            skyboxCamera.targetTexture = skyboxRenderTexture;
        }
        else
        {
            Debug.LogError("Missing reference to skybox camera");
        }
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //if (intensity == 0)
        //{
        //    Graphics.Blit(source, destination);
        //    return;
        //}
        material.SetFloat("_fogIntensity", intensity);
        material.SetTexture("_SkyboxTex", skyboxRenderTexture);
        material.SetFloat("_fogStart", blendStart);
        material.SetFloat("_fogEnd", blendEnd);
        Graphics.Blit(source, destination, material);
    }
}