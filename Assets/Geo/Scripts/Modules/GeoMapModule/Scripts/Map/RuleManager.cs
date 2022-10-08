using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WPM;

public class RuleManager : IManager
{
    private WorldMapGlobe worldMapGlobe;
    private List<LineMarkerAnimator> lines = null;
    private List<GameObject> ruleMarkers = null;

    public WorldMapGlobe WorldMapGlobe { get => worldMapGlobe; set => worldMapGlobe = value; }

    private Transform ruleContainer;
    private Vector3 currRulePoint = Vector3.zero;

    private GameObject ruleTextMeshPerfab;
    private GameObject rulePointMarkerPerfab;

    private float markerScale = 1;

    private Camera mainCamera;
    private float mainCameraDis;

    //³õÊ¼Ïß¿í
    private float initLineWidth = 30;
    public void InitManager(Transform container = null)
    {
        markerScale = 1;
        currRulePoint = Vector3.zero;
        ruleContainer = container;
        lines = new List<LineMarkerAnimator>();
        ruleMarkers = new List<GameObject>();
        ruleTextMeshPerfab = ruleContainer.transform.Find("RuleTextMeshPerfab").gameObject;
        rulePointMarkerPerfab = ruleContainer.transform.Find("RulePointMarkerPerfab").gameObject;

        mainCamera = Camera.main;
        mainCameraDis = 132273000f; //mainCamera.transform.position.sqrMagnitude;
    }

    public void RemoveRule()
    {
        currRulePoint = Vector3.zero;
        lines.Clear();
        worldMapGlobe.ClearLineMarkers();
        foreach (GameObject marker in ruleMarkers)
        {
            GlobalMonoTool.Destroy(marker);
        }
        ruleMarkers.Clear();
    }

    public void AddRulePoint(Vector3 clickLocation) 
    {
        markerScale = mainCamera.transform.position.sqrMagnitude / mainCameraDis;
        markerScale = markerScale * 0.8f;
        markerScale = markerScale > 0.01f ? markerScale : 0.01f;
        addMarker(clickLocation);
        if (currRulePoint != Vector3.zero)
        {
            float lineWidth = initLineWidth* markerScale;  
            LineMarkerAnimator lineMarkerAnimator = worldMapGlobe.AddLine(currRulePoint, clickLocation, Color.white, 0, 0, lineWidth, 0);
            lineMarkerAnimator.LineMarkerCallBack = LineMarkerCallBack;
            lines.Add(lineMarkerAnimator);
        }
        currRulePoint = clickLocation;

        updateLines();
        updateMarkers();
    }

    private void addMarker(Vector3 centerPoint)
    {
        GameObject marker = GlobalMonoTool.Instantiate<GameObject>(rulePointMarkerPerfab);
        marker.transform.SetParent(ruleContainer);
        marker.SetActive(true);
        marker.transform.localPosition = centerPoint;
        marker.transform.localScale = Vector3.one * markerScale;
        marker.transform.LookAt(ruleContainer.position, ruleContainer.up);
        marker.name = "rule_point";
        ruleMarkers.Add(marker);
    }

    private void updateMarkers()
    {
        foreach (GameObject marker in ruleMarkers)
        {
            marker.transform.localScale = Vector3.one * markerScale;
        }
    }

    private void updateLines()
    {
        float zoom = worldMapGlobe.GetZoomLevel();
        float lineWidth = initLineWidth * markerScale;
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
        addTextMeshPro(disstr, centerPoint);
    }

    private void addTextMeshPro(string disstr, Vector3 centerPoint)
    {
        GameObject ruleTextMesh = GlobalMonoTool.Instantiate<GameObject>(ruleTextMeshPerfab);
        TextMeshPro textMesh = ruleTextMesh.GetComponentInChildren<TextMeshPro>();
        ruleTextMesh.SetActive(true);
        ruleTextMesh.transform.SetParent(ruleContainer);
        ruleTextMesh.transform.localPosition = centerPoint;
        ruleTextMesh.transform.localScale = Vector3.one * markerScale;
        ruleTextMesh.transform.LookAt(ruleContainer.position, ruleContainer.up);
        ruleTextMesh.name = disstr;
        textMesh.text = disstr;
        ruleMarkers.Add(ruleTextMesh);
    }

    public void OnQuit()
    {
        
    }

   
}
