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
    /// ��ӵ�����
    /// </summary>
    /// <param name="localHitPosition">���򱾵�����</param>
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
    /// ��ȡ��Ϸ��Ļ�ڵ�����
    /// </summary>
    /// <param name="rect">��ȡ������Ļ���½�Ϊ0��</param>
    /// <param name="fileName">�ļ���</param>
    /// <param name="callBack">��ͼ��ɻص�</param>
    /// <returns></returns>
    public IEnumerator ScreenCapture(Rect rect, string fileName)
    {
        yield return new WaitForEndOfFrame();//�ȵ�֡��������Ȼ�ᱨ��
        Texture2D tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.ARGB32, false);//�½�һ��Texture2D����
        tex.ReadPixels(rect, 0, 0);//��ȡ���أ���Ļ���½�Ϊ0��
        tex.Apply();//����������Ϣ

        //���������ݣ�ת����һ��pngͼƬ
        byte[] bytes = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(fileName, bytes);//д������
        Debug.Log(string.Format("��ȡ��һ��ͼƬ: {0}", fileName));
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();//ˢ��Unity���ʲ�Ŀ¼
#endif
    }

}
