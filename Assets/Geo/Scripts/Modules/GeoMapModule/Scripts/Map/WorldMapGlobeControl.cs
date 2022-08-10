using com.frame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WPM;

public class WorldMapGlobeControl : MonoBehaviour
{
    [SerializeField]
    private GameObject earthHD;

    [SerializeField]
    private GameObject normalEarth;


    [SerializeField]
    private GameObject globalLight;

    [SerializeField]
    private GameObject sunLight;

    [SerializeField]
    private GameObject cloulds;

    [SerializeField]
    private GameObject surfaces; 

    private Material clouldsMat = null;

    public Action<string> onLatLonUpdate = null;


    private WorldMapGlobe worldMapGlobe;

    private StyleEnum earthStyle;
    private Camera mainCamera;
    // Start is called before the first frame update

    private bool initFlag = false;
    public void Init() 
    {
        mainCamera = Camera.main;
        global();
        initFlag = true;
    }


    private void global()  
    {
        ShowCloulds(true);
        normalEarth.SetActive(false);
        sunLight.SetActive(false);
        globalLight.SetActive(true);
        earthHD.SetActive(true);
    }

    private void night() 
    {
        ShowCloulds(false);
        normalEarth.SetActive(false);
        sunLight.SetActive(false);
        globalLight.SetActive(false);
        earthHD.SetActive(true);
    }

    private void nature() 
    {
        ShowCloulds(false);
        normalEarth.SetActive(false);
        sunLight.SetActive(true);
        globalLight.SetActive(false);
        earthHD.SetActive(true);
    }

    public void ShowSurface(bool isShow)
    {
        transform.Find("Surfaces").gameObject.SetActive(isShow);
    }



    public void ChangeMapByStyle(StyleEnum style) 
    {
        earthStyle = style;
        if(style == StyleEnum.�ƹ�ģʽ)
        {
            night();
        }
        else if (style == StyleEnum.����Ӱ��_����)
        {
            global();
        }
        else
        {
            nature();
            Country zhCountry = worldMapGlobe.GetCountry("�й�");
            if (style == StyleEnum.����ģ��)
            {
                zhCountry.labelVisible = true;
                worldMapGlobe.showCountryNames = true;
                earthHD.SetActive(true);
            }
            else
            {
                if(worldMapGlobe.showProvinces || worldMapGlobe.showCities)
                {
                    zhCountry.labelVisible = false;
                }
                earthHD.SetActive(false);
            }
        }
    }

    public void ShowCloulds(bool flag)
    {
        if(clouldsMat == null)
        {
            clouldsMat = Resources.Load<Material>("Materials/HighCloud");
            cloulds.GetComponent<MeshRenderer>().material = clouldsMat;
        }
        cloulds.gameObject.SetActive(flag);

    }


    #region ���¾�γ����Ϣ
    private float nextLangLongUpdate = 0;
    private float langLongUpdateRate = 0.1f;
    private float lastDis = 0;


    public WorldMapGlobe WorldMapGlobe { get => worldMapGlobe; set => worldMapGlobe = value; }

    void Update()
    {
        // Check whether a country or city is selected, then show a label
        if (initFlag && WorldMapGlobe.mouseIsOver)
        {
            if (Time.time > nextLangLongUpdate)
            {
                onLatLonUpdate?.Invoke(WorldMapGlobe.calc.prettyCurrentLatLon);
                nextLangLongUpdate = Time.time + langLongUpdateRate;
            }
        }

        float dis = mainCamera.transform.position.magnitude;
        if(lastDis != dis)
        {
            lastDis = dis;
            //Debug.Log("lastDis:" + lastDis);
            worldMapGlobe.countryLabelsSize = 0.25f - (60.1f - dis) * 0.025f;
            worldMapGlobe.cityIconSize = 1 - (60.1f - dis) * 0.1f;
        }

        


        if (earthStyle == StyleEnum.����Ӱ��_����)
        {
            if (mainCamera.transform.position.magnitude <= 51.5f)
            {
                EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "style", StyleEnum.����Ӱ��);
                //ChangeMapByStyle(StyleEnum.����Ӱ��);
            }
            
        }
        else if (earthStyle == StyleEnum.����Ӱ��)
        {
            if (mainCamera.transform.position.magnitude > 51.5f)
            {
                EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "style", StyleEnum.����Ӱ��_����);
                //ChangeMapByStyle(StyleEnum.����Ӱ��);
            }
        }
    }
    #endregion
}
