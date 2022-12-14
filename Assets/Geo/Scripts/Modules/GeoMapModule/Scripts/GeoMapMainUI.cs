using com.frame;
using DigitalRuby.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WPM;
using static UnityEngine.UI.Dropdown;

public class GeoMapMainUI : ModuleUI
{
    [SerializeField]
    private Toggle continentToggle;
    [SerializeField]
    private Toggle countryToggle;
    [SerializeField]
    private Toggle provinceToggle;
    [SerializeField]
    private Toggle cityToggle;

    [SerializeField]
    private Button closeBtn;
    [SerializeField]
    private Button openBtn;
    [SerializeField]
    private Button compassBtn; 
    [SerializeField]
    private ToolUI toolUI; 

    private Text seletModeTxt;
    [SerializeField]
    private GameObject Loading;

    [SerializeField]
    private Toggle modelToggle; 
    [SerializeField]
    private GameObject modeList;

    [SerializeField]
    private GameObject modeContent; 

    private Toggle[] modeListToggles;

    [SerializeField]
    private Transform operateUITran;

    [SerializeField]
    private Color modeLightColor; 

    private bool openFlag = true;

    
    [SerializeField]
    private List<string> styles;

    [SerializeField]
    private ClouldUI clouldUI;

    [SerializeField]
    private InfoUI infoUI;

    private Vector3 openPose;
    private Vector3 closePose;

    private StyleEnum currStyleEnum = StyleEnum.Ĭ??ģʽ;
    private Camera mainCamera;

    private Vector3 screenCenter;

    void Start()
    {
        mainCamera = Camera.main;
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2,0);
    }

    float deltTime = 0;
    void Update()
    {
        deltTime += Time.deltaTime;
        if(deltTime >0.05f)
        {
            deltTime = 0;
            Vector3 polePosition = WorldMapGlobeControl.Instance.GetPolePosition();
            Vector3 screenPos = mainCamera.WorldToScreenPoint(polePosition) - screenCenter;
            float angle = angle_360(screenPos);
            Vector3 eulerAngles = new Vector3(0, 0, angle);
            compassBtn.transform.eulerAngles = eulerAngles;
        }
        
    }

    public float angle_360(Vector2 v2)
    {
        float angle = Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
        if (angle < 0)
            angle = 360 + angle;

        return angle;
    }

    public override void InitUI()
    {
        base.InitUI();
        initOperateUIPose();
        infoUI.gameObject.SetActive(false);
        initOperateUI();

        EventUtil.AddListener(GlobalEvent.Module_TO_UI_Action ,onModuleAction);
        WorldMapGlobeControl.Instance.onLatLonUpdate = onLatLonUpdate;

        initModeUI();

        toolUI.InitUI();

        clouldUI.InitUI();
    }

    
    public void ShowClouldUI(ClouldVO clouldVO)
    {
        clouldUI.ShowClould(clouldVO);
    }

    public void HideClouldUI()
    {
        clouldUI.HideClould();
    }

    private void initModeUI()
    {
        modeContent.SetActive(modelToggle.isOn);

        seletModeTxt = modelToggle.transform.Find("Text").GetComponent<Text>();
        seletModeTxt.gameObject.SetActive(false);

        modelToggle.onValueChanged.AddListener(delegate { onModelValueChanged(); });

        modeListToggles = modeList.GetComponentsInChildren<Toggle>();
        Toggle defaultToggle = modeListToggles[0];


        foreach (Toggle modeListToggle in modeListToggles)
        {
            modeListToggle.onValueChanged.AddListener(delegate { onModelListToggleValueChanged(modeListToggle); });

            if(modeListToggle.name == "Ĭ??ģʽ")
            {
                defaultToggle = modeListToggle;
            }
        }
        defaultToggle.isOn = true;
    }

    private Toggle currToggle;

    private void onModelListToggleValueChanged(Toggle toggle) 
    {
        Text label = toggle.transform.Find("Label").GetComponent<Text>();
        if (toggle.isOn)
        {
            currToggle = toggle;
            if (!seletModeTxt.gameObject.activeSelf)
            {
                seletModeTxt.gameObject.SetActive(true);
            }
            seletModeTxt.text = label.text;
            label.color = modeLightColor;
            StartCoroutine(ChangeToStyle());
        }
        else
        {
            label.color = Color.white;
        }
    }

    IEnumerator ChangeToStyle()
    {
        if(currToggle.name == StyleEnum.?Ʋ?ģʽ.ToString() )
        {
            operateUITran.gameObject.SetActive(false);
        }
        else
        {
            if(!operateUITran.gameObject.activeSelf)
            {
                operateUITran.gameObject.SetActive(true);
            }
        }
        yield return new WaitForEndOfFrame();
        EventUtil.DispatchEvent(GlobalEvent.UI_TO_Module_Action, "Style", currToggle.name);
        yield return null;
    }

    /// <summary>
    /// ?򿪹رյ???????????
    /// </summary>
    private void onModelValueChanged()
    {
        modeContent.SetActive(modelToggle.isOn);
        if(!modelToggle.isOn)
        {
            modelToggle.transform.Find("Text").GetComponent<Text>().color = Color.grey;
           
        }
        else
        {
            showCurrStyle();
            modelToggle.transform.Find("Text").GetComponent<Text>().color = modeLightColor;
        }
    }

    private void initOperateUI()
    {
        closeBtn.gameObject.SetActive(true);
        closeBtn.onClick.AddListener(delegate { setOpenFlag(false); });
        openBtn.onClick.AddListener(delegate { setOpenFlag(true); });

        compassBtn.onClick.AddListener(delegate { onCompassBtnClick();  });
        openFlag = true;

        initoperateToggles();
    }

    private void onCompassBtnClick()
    {
        EventUtil.DispatchEvent(GlobalEvent.UI_TO_Module_Action, "Compass");
    }

    private void initoperateToggles()
    {
        continentToggle.onValueChanged.AddListener(delegate { StartCoroutine(OnToggleValueChanged("Continent", continentToggle.isOn)); });
        countryToggle.onValueChanged.AddListener(delegate { StartCoroutine(OnToggleValueChanged("Country", countryToggle.isOn)); });
        provinceToggle.onValueChanged.AddListener(delegate { StartCoroutine(OnToggleValueChanged("Province", provinceToggle.isOn)); });
        cityToggle.onValueChanged.AddListener(delegate { StartCoroutine(OnToggleValueChanged("City", cityToggle.isOn)); });
    }

    private void onLatLonUpdate(string value)
    {
        toolUI.ShowLatLon(value);
    }

    /// <summary>
    /// ??ʼ???Ҳ??б??򿪡??رյ?λ??
    /// </summary>
    private void initOperateUIPose()
    {
        openPose = operateUITran.localPosition; 
        closePose = openPose; 
        closePose.x = openPose.x + 100;
    }

    /// <summary>
    /// ?????Ҳ??б??򿪹رձ?־
    /// </summary>
    /// <param name="flag"></param>
    private void setOpenFlag(bool flag)
    {

        if(!flag)
        {
            modelToggle.isOn = false;
        }

        if(openFlag != flag)
        {
            openFlag = flag;
            tweenMove();
        }
    }

    /// <summary>
    /// ģ???¼?????
    /// </summary>
    /// <param name="eventArgs"></param>
    private void onModuleAction(CustomEventArgs eventArgs)
    {
        string action = ((string)eventArgs.args[0]).ToLower();
        //ʡ?ݡ???????Ϣչʾ
        if (action == "info")
        {
            InfoVO infoVO = (InfoVO)eventArgs.args[1];
            if(infoVO != null)
            {
                infoUI.gameObject.SetActive(true);
                infoUI.SetInfo(infoVO);
            }
            else
            {
                infoUI.gameObject.SetActive(false);
            }
        }
        else if(action == "continentinfo")
        {
            ContinentVO continentVO = (ContinentVO)eventArgs.args[1];
            if (continentVO != null)
            {
                infoUI.gameObject.SetActive(true);
                infoUI.SetContinentInfo(continentVO);
            }
            else
            {
                infoUI.gameObject.SetActive(false);
            }

        }
        else if(action == "toggle")
        {
            ///?ޡ????ҡ?ʡ?ݡ?????
            if(eventArgs.args.Length >= 3)
            {
                Toggle toggle = null;
                string toggleName = (string)eventArgs.args[1];
                if (toggleName == "Country")
                {
                    toggle = countryToggle;
                }
                else if (toggleName == "Province")
                {
                    toggle = provinceToggle;
                }
                else if (toggleName == "City")
                {
                    toggle = cityToggle;
                }
                else if (toggleName == "Continent")
                {
                    toggle = continentToggle;
                }

                bool isOn = (bool)eventArgs.args[2];
                if (toggle != null)
                {
                    toggle.isOn = isOn;
                }
            }
        }
        else if(action == "style")
        {
            currStyleEnum = (StyleEnum)eventArgs.args[1];
            showCurrStyle();
        }
        else if (action == "earthcloud")
        {
            bool flag = (bool)eventArgs.args[1];
            string cloudName = (string)eventArgs.args[2];

            if (flag)
            {
                ClouldVO clouldVO = AppConfigManager.Instance.GetClouldVO(cloudName);
                clouldUI.ShowClould(clouldVO);
            }
            else
            {
                clouldUI.HideClould();
            }
        }
        
    }

    private void showCurrStyle()
    {
        foreach (Toggle modeListToggle in modeListToggles)
        {
            if (modeListToggle.name == currStyleEnum.ToString())
            {
                modeListToggle.isOn = true;
                return;
            }
        }
    }

    IEnumerator  OnToggleValueChanged(string action,bool isOn)  
    {
        if(!isOn && (action == "Country" || action == "Province" || action == "City"))
        {
            infoUI.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(0.1f);
        EventUtil.DispatchEvent(GlobalEvent.UI_TO_Module_Action, action, isOn);
        yield return null;
    }


    public override void OnQuit()
    {
        base.OnQuit();
        EventUtil.RemoveListener(GlobalEvent.Module_TO_UI_Action, onModuleAction);
    }

    private void tweenMove() 
    {
        System.Action<ITween<Vector3>> updateCirclePos = (t) =>
        {
            operateUITran.localPosition = t.CurrentValue;
        };

        System.Action<ITween<Vector3>> circleMoveCompleted = (t) =>
        {
            closeBtn.gameObject.SetActive(openFlag);
            openBtn.gameObject.SetActive(!openFlag);
        };

        Vector3 currentPos = operateUITran.localPosition;
        Vector3 startPos = openPose;
        Vector3 endPos = closePose;
        if (openFlag)
        {
            startPos = closePose;
            endPos = openPose;
        }
        operateUITran.gameObject.Tween("MoveCircle", startPos, endPos, 0.5F, TweenScaleFunctions.CubicEaseIn, updateCirclePos, circleMoveCompleted);
    }
}
