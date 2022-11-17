using com.frame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WPM;

public class MapContinentManager : MonoBehaviour,IManager
{
    [SerializeField]
    private GameObject worldMapGlobeEarthContinent;

    //洲单独贴图
    private Texture2D continentTexture;

    //洲叠加国家贴图
    private Texture2D continentCountryTexture;
    //洲材质球
    private Material continentMaterial;
    public void InitManager(Transform container = null)
    {
        initContinent();
    }

    /// <summary>
    /// 初始化洲对应的资源
    /// </summary>
    private void initContinent()
    {
        continentMaterial = worldMapGlobeEarthContinent.GetComponent<MeshRenderer>().material;
        continentTexture = Resources.Load<Texture2D>("Textures/Continent");
        continentCountryTexture = Resources.Load<Texture2D>("Textures/Continent_Country");
        continentMaterial.mainTexture = continentTexture;
    }

    

    public void ShowContinent(bool flag)
    {
        worldMapGlobeEarthContinent.SetActive(flag);
    }

    public void DealContinentAndCountry(bool countryFlag)
    {
        if (worldMapGlobeEarthContinent.activeSelf)
        {
            if (countryFlag)
            {
                continentMaterial.mainTexture = continentCountryTexture;
            }
            else
            {
                continentMaterial.mainTexture = continentTexture;
            }


        }
    }

    private WorldMapGlobe worldMapGlobe;
    public WorldMapGlobe WorldMapGlobe { get => worldMapGlobe; set => worldMapGlobe = value; }

    private bool smallFlag = false;
    void Update()
    {
        if (worldMapGlobeEarthContinent.activeSelf && !worldMapGlobe.showCountryNames && !worldMapGlobe.showProvinces && !worldMapGlobe.showCities)
        {
            checkSelectContinent();
        }
    }


    #region Continent

    private Vector3 lastHitPoint = Vector3.zero;
    private void checkSelectContinent()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 earthHitPoint = Vector3.zero;
            if (worldMapGlobe.GetGlobeIntersection(out earthHitPoint))
            {
                Vector3 localEarthPoint = worldMapGlobe.transform.InverseTransformPoint(earthHitPoint);
                Vector2 uv = Conversion.GetUVFromSpherePoint(localEarthPoint);
                int hitW = (int)(continentTexture.width * uv.x);
                int hitH = (int)(continentTexture.height * uv.y);
                Color hitColor = continentTexture.GetPixel(hitW, hitH) * 255;

                Vector3 hitColorValue = new Vector3(hitColor.r, hitColor.g, hitColor.b);

                ContinentVO continentVO = AppConfigManager.Instance.GetMatchContinentVO(hitColorValue);
                if (continentVO != null)
                {
                    EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "continentinfo", continentVO);
                }
                else
                {
                    EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "continentinfo", null);
                }
            }
            else
            {
                EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "continentinfo", null);
            }
        }
    }

    private Vector3 getHitPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.Log("checkSelectContinent worldMapGlobe.cursorLocation:" + worldMapGlobe.cursorLocation);
        
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, int.MaxValue))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    #endregion

    public void OnQuit()
    {
        
    }

}
