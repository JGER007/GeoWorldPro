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
    /// չʾ�ޱ�ǩ��Ϣ
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

                if(!key.Contains("��"))
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
    /// ��ʼ���ޱ�ǩ��Ϣ
    /// ���ޣ�(-0.3, 0.4, 0.0)
    /// ŷ�ޣ�(-0.2, 0.4, 0.3)
    /// ���ޣ�(-0.1, 0.1, 0.5)
    /// �����ޣ�(0.3, 0.4, -0.1)
    /// �����ޣ�(0.4, -0.1, 0.3)
    /// �����ޣ�(-0.3, -0.2, -0.3)
    /// �ϼ��ޣ�(0.0, -0.5, 0.0)
    /// </summary>
    private void initContinentCenterDic() 
    {
        continentCenterDic = new Dictionary<string, Vector3>();
        continentCenterDic.Add("����",new Vector3(-0.4f, 0.3f, 0f));
        continentCenterDic.Add("ŷ��", new Vector3(-0.15f, 0.4f, 0.27f));
        continentCenterDic.Add("����", new Vector3(-0.18f, 0.1f, 0.47f));
        continentCenterDic.Add("������", new Vector3(0.321f, 0.4f, 0f));
        continentCenterDic.Add("������", new Vector3(0.432f, -0.08f, 0.2685f));
        continentCenterDic.Add("������", new Vector3(-0.34f, -0.167f, -0.34f));
        continentCenterDic.Add("�ϼ���", new Vector3(0.0f, -0.5f, 0.0f));

        continentCenterDic.Add("�����³ʿɽ", new Vector3(-0.2443f, 0.3577f, 0.2541f));
        continentCenterDic.Add("��������", new Vector3(-0.2551f, 0.3919f, 0.1835f));
        continentCenterDic.Add("������ɽ", new Vector3(-0.1949f, 0.4496f, 0.1099f));
        continentCenterDic.Add("����ʿ�˺�", new Vector3(-0.2624f, 0.2562f, 0.3433f));
        continentCenterDic.Add("�����亣Ͽ���ں���Ͽ��", new Vector3(-0.164f, 0.3453f, 0.325f));
        continentCenterDic.Add("����Ͽ", new Vector3(0.0569f, 0.4311f, 0.2511f));
        continentCenterDic.Add("�������˺�", new Vector3(0.497f, 0.0404f, 0.0573f));
        continentCenterDic.Add("���Ͽ", new Vector3(0.0304f, 0.4679f, -0.1803f));
        continentCenterDic.Add("���뺣", new Vector3(-0.4156f, -0.1079f, -0.2594f));
        continentCenterDic.Add("����������", new Vector3(-0.3545f, -0.0889f, -0.3433f));
        continentCenterDic.Add("���׿˺�Ͽ", new Vector3(0.2352f, -0.4282f, 0.1153f));
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
    /// ��ӵ�����
    /// </summary>
    /// <param name="localHitPosition">���򱾵�����</param>
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
