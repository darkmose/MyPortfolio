﻿using UnityEngine;

public class ExplosionsShaderColorGradient : MonoBehaviour
{
    public string ShaderProperty = "_TintColor";
    public int MaterialID = 0;
    public Gradient Color = new Gradient();
    public float TimeMultiplier = 1;

    private bool canUpdate;
    private Material matInstance;
    private int propertyID;
    private float startTime;
    private Color oldColor;

    // Use this for initialization
    private void Start()
    {
        var mats = GetComponent<Renderer>().materials;
        if (MaterialID >= mats.Length)
            Debug.Log("ShaderColorGradient: Material ID more than shader materials count.");
        matInstance = mats[MaterialID];
        if (!matInstance.HasProperty(ShaderProperty))
            Debug.Log("ShaderColorGradient: Shader not have \"" + ShaderProperty + "\" property");
        propertyID = Shader.PropertyToID(ShaderProperty);
        oldColor = matInstance.GetColor(propertyID);
    }

    private void OnEnable()
    {
        startTime = Time.time;
        canUpdate = true;
    }

    private void Update()
    {
        var time = Time.time - startTime;
        if (canUpdate) {
            var eval = Color.Evaluate(time / TimeMultiplier);
            matInstance.SetColor(propertyID, eval * oldColor);
        }
        if (time >= TimeMultiplier)
            canUpdate = false;
    }
}