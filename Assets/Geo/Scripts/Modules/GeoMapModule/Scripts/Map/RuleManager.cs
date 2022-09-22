using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WPM;

public class RuleManager : IManager
{

    private WorldMapGlobe worldMapGlobe;

    private List<LineMarkerAnimator> lines = null;

    public WorldMapGlobe WorldMapGlobe { get => worldMapGlobe; set => worldMapGlobe = value; }

    private Transform ruleContainer;
    private Vector3 currRulePoint = Vector3.zero;
    public void InitManager(Transform container = null)
    {
        currRulePoint = Vector3.zero;
        ruleContainer = container;
        lines = new List<LineMarkerAnimator>();
    }

    public void RemoveRule()
    {
        currRulePoint = Vector3.zero;
        lines.Clear();
        worldMapGlobe.ClearMarkers();
        worldMapGlobe.ClearLineMarkers();
        clearText();
        
    }

    public void AddRulePoint(Vector3 clickLocation) 
    {
        float km = 50;
        worldMapGlobe.AddMarker(MARKER_TYPE.CIRCLE_PROJECTED, clickLocation, km, 0f, 1f, Color.white);
        if (currRulePoint != Vector3.zero)
        {
            float lineWidth = 0.05f;  //0.035f * worldMapGlobe.GetZoomLevel()/ 1.3f;
            LineMarkerAnimator lineMarkerAnimator = worldMapGlobe.AddLine(currRulePoint, clickLocation, Color.white, 0, 0, lineWidth, 0);
            lineMarkerAnimator.LineMarkerCallBack = LineMarkerCallBack;
            lines.Add(lineMarkerAnimator);
        }
        currRulePoint = clickLocation;

        updateLines();
    }

    private void updateLines()
    {
        float zoom = worldMapGlobe.GetZoomLevel();
        float lineWidth = (2.3f-zoom)*0.035f * zoom / 1.3f;
        lineWidth = 0.05f;
        Debug.Log("Camera.main.transform.position.sqrMagnitude:" + Camera.main.transform.position.sqrMagnitude);

        foreach(LineMarkerAnimator lineMarkerAnimator in lines)
        {
            lineMarkerAnimator.lineRenderer.startWidth = lineWidth;
            lineMarkerAnimator.lineRenderer.endWidth = lineWidth;
        }
    }



    private void LineMarkerCallBack(Vector3[] vertices)
    {
        int centerIndex = vertices.Length / 2;
        Vector3 centerPoint = vertices[centerIndex];
        float dis = worldMapGlobe.calc.Distance(vertices[0], vertices[vertices.Length - 1])/1000;
        string disstr = dis.ToString("#.##") + " KM";

        float fontScale =  0.015F * worldMapGlobe.GetZoomLevel()/ 1.3f ; 
        TextMesh textMesh = worldMapGlobe.AddText(disstr, centerPoint, Color.blue, fontScale, null,FontStyle.Bold);
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
