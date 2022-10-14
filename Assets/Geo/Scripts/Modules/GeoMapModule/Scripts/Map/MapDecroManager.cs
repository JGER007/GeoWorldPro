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
                continentlabel.transform.Find("Text").GetComponent<TextMeshPro>().text = key;
                continentlabel.name = key;
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
            Vector3 hitPosition = getHitPoint();
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

#endif
    }

    private Vector3 getHitPoint() 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, int.MaxValue))
        {
            //_worldMapGlobe.ZoomTo(0.1f);
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
        marker.transform.localPosition = localHitPosition;
        marker.transform.localScale = Vector3.one * 0.003F;
        marker.transform.LookAt(mapMarkerContainer.transform.position);
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
