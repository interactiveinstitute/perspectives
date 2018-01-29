using UnityEngine;
using System.Collections;
 
[ExecuteInEditMode]
public class SkyboxBlend : MonoBehaviour {
 
    [Range(0f,1f)]
    public float intensity;
    RenderTexture skyboxRenderTexture;
    private Material material;

    [Tooltip("The camera that renders the skybox into a render texture")]
    public Camera skyboxCamera;

    public float blendStart = 200.0f;
    public float blendEnd = 2000.0f;
 
    // Creates a private material used to the effect
    void Awake ()
    {
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;

        material = new Material( Shader.Find("Hidden/SkyboxBlend") );

        int width = skyboxCamera.pixelWidth;
        int height = skyboxCamera.pixelHeight;
        skyboxRenderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        skyboxCamera.targetTexture = skyboxRenderTexture;
    }
 
    // Postprocess the image
    void OnRenderImage (RenderTexture source, RenderTexture destination)
    {
        //if (intensity == 0)
        //{
        //    Graphics.Blit(source, destination);
        //    return;
        //}
        material.SetFloat("_effectBlend", intensity);
        material.SetTexture("_SkyboxTex", skyboxRenderTexture);
        material.SetFloat("_blendStart", blendStart);
        material.SetFloat("_blendEnd", blendEnd);
        Graphics.Blit(source, destination, material);
    }
}