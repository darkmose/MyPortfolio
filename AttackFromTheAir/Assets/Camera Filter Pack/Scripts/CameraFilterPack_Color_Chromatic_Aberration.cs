////////////////////////////////////////////
// CameraFilterPack - by VETASOFT 2020 /////
////////////////////////////////////////////

using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu ("Camera Filter Pack/Colors/Chromatic_Aberration")]
public class CameraFilterPack_Color_Chromatic_Aberration : MonoBehaviour {
#region Variables
public Shader SCShader;
private float TimeX = 1.0f;
 
private Material SCMaterial;
[Range(-0.02f, 0.02f)]
public float Offset = 0.02f;

#endregion

#region Properties
Material material
{
get
{
if(SCMaterial == null)
{
SCMaterial = new Material(SCShader);
SCMaterial.hideFlags = HideFlags.HideAndDontSave;	
}
return SCMaterial;
}
}
#endregion
void Start () 
{
SCShader = Shader.Find("CameraFilterPack/Color_Chromatic_Aberration");

if(!SystemInfo.supportsImageEffects)
{
enabled = false;
return;
}
}

void OnRenderImage (RenderTexture sourceTexture, RenderTexture destTexture)
{
if(SCShader != null)
{
TimeX+=Time.deltaTime;
if (TimeX>100)  TimeX=0;
material.SetFloat("_TimeX", TimeX);
material.SetFloat("_Distortion", Offset);
material.SetVector("_ScreenResolution",new Vector4(sourceTexture.width,sourceTexture.height,0.0f,0.0f));
Graphics.Blit(sourceTexture, destTexture, material);
}
else
{
Graphics.Blit(sourceTexture, destTexture);	
}


}


void Update () 
{

#if UNITY_EDITOR
if (Application.isPlaying!=true)
{
SCShader = Shader.Find("CameraFilterPack/Color_Chromatic_Aberration");

}
#endif

}

void OnDisable ()
{
if(SCMaterial)
{
DestroyImmediate(SCMaterial);	
}

}


}