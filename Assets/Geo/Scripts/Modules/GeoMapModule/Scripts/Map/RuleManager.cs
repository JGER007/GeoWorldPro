using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WPM;

public class RuleManager : IManager
{

    private WorldMapGlobe worldMapGlobe;

    public WorldMapGlobe WorldMapGlobe { get => worldMapGlobe; set => worldMapGlobe = value; }

    private Transform ruleContainer;
    private Vector3 currRulePoint = Vector3.zero;
    public void InitManager(Transform container = null)
    {
        currRulePoint = Vector3.zero;
        ruleContainer = container;
    }

    public void RemoveRule()
    {
        currRulePoint = Vector3.zero;
        worldMapGlobe.ClearMarkers();
        worldMapGlobe.ClearLineMarkers();
        clearText();
    }

    public void AddRulePoint(Vector3 clickLocation) 
    {
        float km = 50;
        worldMapGlobe.AddMarker(MARKER_TYPE.CIRCLE_PROJECTED, clickLocation, km, 0.2f, 0.975f, Color.white);
        
        if (currRulePoint != Vector3.zero)
        {
            LineMarkerAnimator lineMarkerAnimator = worldMapGlobe.AddLine(currRulePoint, clickLocation, Color.white, 0, 0, 0.05F, 0);
            lineMarkerAnimator.LineMarkerCallBack = LineMarkerCallBack;
        }
        currRulePoint = clickLocation;
    }

    private void LineMarkerCallBack(Vector3[] vertices)
    {
        int centerIndex = vertices.Length / 2;
        Vector3 centerPoint = vertices[centerIndex];
        float dis = worldMapGlobe.calc.Distance(vertices[0], vertices[vertices.Length - 1])/1000;
        string disstr = dis.ToString("#.##") + " KM";
        TextMesh textMesh = worldMapGlobe.AddText(disstr, centerPoint, Color.blue, 0.015F,null,FontStyle.Bold);
        textMesh.transform.SetParent(ruleContainer);
    }

    private void clearText()
    {
        TextMesh[] textMeshes = ruleContainer.GetComponentsInChildren<TextMesh>();
        for (int k = 0; k < textMeshes.Length; k++)
        {
            GlobalMonoTool.Destroy(textMeshes[k].gameObject);
        }
    }

    public void OnQuit()
    {
        
    }

   
}
