using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapGlobeEarth_HD : MonoBehaviour
{
    void OnEnable()
    {
        Renderer mr = GetComponent<MeshRenderer>();
        Material[] materials = mr.materials;
        
        Shader shader = Shader.Find("Stagit/EarthShaderHd");

        foreach(Material material in materials)
        {
            material.shader = shader;
            material.renderQueue = 1980;
        }
    }
}
