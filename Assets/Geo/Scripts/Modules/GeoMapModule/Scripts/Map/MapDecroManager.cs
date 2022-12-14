using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WPM;

public class MapDecroManager :MonoBehaviour, IManager
{
    [SerializeField]
    private Transform mapDecroContainer;

    [SerializeField]
    private GameObject textLabelPerfab; 


    private Transform mapMarkerContainer;
    private Transform mapContinentLabelContainer; 

    private WorldMapGlobe _worldMapGlobe = null;

    public WorldMapGlobe WorldMapGlobe { get => _worldMapGlobe; set => _worldMapGlobe = value; }

    private Dictionary<string, Vector3> continentCenterDic = null;

    public void InitManager(Transform container = null)
    {
        mapMarkerContainer = mapDecroContainer.Find("Markers");
        mapContinentLabelContainer = mapDecroContainer.Find("ContinentLabels");
        initContinentCenterDic();

        //ShowContinentLabel();
    }

    /// <summary>
    /// 展示洲标签信息
    /// </summary>
    public void ShowContinentLabel()
    {
        mapContinentLabelContainer.gameObject.SetActive(true);
        if(mapContinentLabelContainer.childCount == 0)
        {
            foreach(string key in continentCenterDic.Keys)
            {
                Vector3 continentLocalPosition = continentCenterDic[key];
                GameObject continentlabel = GameObject.Instantiate<GameObject>(textLabelPerfab);
                TextMeshPro textMeshPro = continentlabel.transform.Find("Text").GetComponent<TextMeshPro>();
                textMeshPro.text = key;
                continentlabel.name = key;

                if(!key.Contains("洲"))
                {
                    textMeshPro.fontSize = 10;
                }


                continentlabel.transform.SetParent(mapContinentLabelContainer.transform);
                continentlabel.transform.localPosition = continentLocalPosition;
                continentlabel.transform.localScale = Vector3.one ;
                continentlabel.transform.LookAt(mapContinentLabelContainer.transform.position);
            }
        }

    }

    public void HideContinentLabel()
    {
        mapContinentLabelContainer.gameObject.SetActive(false);
    }

    /// <summary>
    /// 初始化洲标签信息
    /// 亚洲：(-0.3, 0.4, 0.0)
    /// 欧洲：(-0.2, 0.4, 0.3)
    /// 非洲：(-0.1, 0.1, 0.5)
    /// 北美洲：(0.3, 0.4, -0.1)
    /// 南美洲：(0.4, -0.1, 0.3)
    /// 大洋洲：(-0.3, -0.2, -0.3)
    /// 南极洲：(0.0, -0.5, 0.0)
    /// </summary>
    private void initContinentCenterDic() 
    {
        continentCenterDic = new Dictionary<string, Vector3>();
        continentCenterDic.Add("亚洲",new Vector3(-0.4f, 0.3f, 0f));
        continentCenterDic.Add("欧洲", new Vector3(-0.15f, 0.4f, 0.27f));
        continentCenterDic.Add("非洲", new Vector3(-0.18f, 0.1f, 0.47f));
        continentCenterDic.Add("北美洲", new Vector3(0.321f, 0.4f, 0f));
        continentCenterDic.Add("南美洲", new Vector3(0.432f, -0.08f, 0.2685f));
        continentCenterDic.Add("大洋洲", new Vector3(-0.34f, -0.167f, -0.34f));
        continentCenterDic.Add("南极洲", new Vector3(0.0f, -0.5f, 0.0f));

        continentCenterDic.Add("厄尔布鲁士山", new Vector3(-0.2443f, 0.3577f, 0.2541f));
        continentCenterDic.Add("乌拉尔河", new Vector3(-0.2551f, 0.3919f, 0.1835f));
        continentCenterDic.Add("乌拉尔山", new Vector3(-0.1949f, 0.4496f, 0.1099f));
        continentCenterDic.Add("苏伊士运河", new Vector3(-0.2624f, 0.2562f, 0.3433f));
        continentCenterDic.Add("土耳其海峡（黑海海峡）", new Vector3(-0.164f, 0.3453f, 0.325f));
        continentCenterDic.Add("丹麦海峡", new Vector3(0.0569f, 0.4311f, 0.2511f));
        continentCenterDic.Add("巴拿马运河", new Vector3(0.497f, 0.0404f, 0.0573f));
        continentCenterDic.Add("白令海峡", new Vector3(0.0304f, 0.4679f, -0.1803f));
        continentCenterDic.Add("帝汶海", new Vector3(-0.4156f, -0.1079f, -0.2594f));
        continentCenterDic.Add("阿拉佛拉海", new Vector3(-0.3545f, -0.0889f, -0.3433f));
        continentCenterDic.Add("德雷克海峡", new Vector3(0.2352f, -0.4282f, 0.1153f));
    }

    public void OnQuit()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 hitPosition = getHitPoint(false);
            if(hitPosition != Vector3.zero)
            {
                Vector3 localHitPosition = transform.InverseTransformPoint(hitPosition);
                Debug.Log("localHitPosition:" + localHitPosition);
                addMarker(localHitPosition);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            getScreen();
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            Vector3 hitPosition = getHitPoint();
            if (hitPosition != Vector3.zero)
            {
                Vector3 localHitPosition = transform.InverseTransformPoint(hitPosition);
                addMarker(localHitPosition);
            }
        }
#endif
    }

    private Vector3 getHitPoint(bool isCenter = true) 
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 screenPoint = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        if(!isCenter)
        {
            screenPoint = Input.mousePosition;
        }

        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, int.MaxValue))
        {
            //_worldMapGlobe.ZoomTo(0.2f);
            return hit.point;
        }
        return Vector3.zero;
    }

    /// <summary>
    /// 添加地球标记
    /// </summary>
    /// <param name="localHitPosition">地球本地坐标</param>
    private void addMarker(Vector3 localHitPosition) 
    {
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        marker.transform.SetParent(mapMarkerContainer.transform);
        marker.name = "Marker_" + mapMarkerContainer.transform.childCount;
        marker.transform.localPosition = localHitPosition;
        marker.transform.localScale = Vector3.one * 0.0015F;
        marker.transform.LookAt(mapMarkerContainer.transform.position);

        //Vector3 latlonPoint = Conversion.GetLatLonFromSpherePoint(localHitPosition);
        //Debug.Log("addMarker latlonPoint:" + latlonPoint.ToString());
    }

    private void getScreen()
    {
        Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
        Debug.Log(screenRect);
        StartCoroutine(ScreenCapture(screenRect, "aaaa.png"));
    }


    /// <summary>
    /// 截取游戏屏幕内的像素
    /// </summary>
    /// <param name="rect">截取区域：屏幕左下角为0点</param>
    /// <param name="fileName">文件名</param>
    /// <param name="callBack">截图完成回调</param>
    /// <returns></returns>
    public IEnumerator ScreenCapture(Rect rect, string fileName)
    {
        yield return new WaitForEndOfFrame();//等到帧结束，不然会报错
        Texture2D tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.ARGB32, false);//新建一个Texture2D对象
        tex.ReadPixels(rect, 0, 0);//读取像素，屏幕左下角为0点
        tex.Apply();//保存像素信息

        //将纹理数据，转化成一个png图片
        byte[] bytes = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(fileName, bytes);//写入数据
        Debug.Log(string.Format("截取了一张图片: {0}", fileName));
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();//刷新Unity的资产目录
#endif
    }

}
