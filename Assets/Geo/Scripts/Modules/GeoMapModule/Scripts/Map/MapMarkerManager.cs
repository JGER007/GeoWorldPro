using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WPM;

public class MapMarkerManager :MonoBehaviour, IManager
{
    [SerializeField]
    private GameObject mapMarkerContainer;

    private WorldMapGlobe _worldMapGlobe = null;

    public WorldMapGlobe WorldMapGlobe { get => _worldMapGlobe; set => _worldMapGlobe = value; }

    public void InitManager(Transform container = null)
    {
        
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
            _worldMapGlobe.ZoomTo(0.1f);
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


    private Rect screenRect ;

    

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
