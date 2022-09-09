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
    private InfoUI infoUI;

    private Vector3 openPose;
    private Vector3 closePose;

    private StyleEnum currStyleEnum = StyleEnum.默认模式;
    private Camera mainCamera;

    private WorldMapGlobeControl worldMapGlobeControl;

    void Start()
    {
        mainCamera = Camera.main;
        //InitUI();
    }

    float deltTime = 0;
    void Update()
    {
        deltTime += Time.deltaTime;
        if(deltTime >0.05f)
        {
            deltTime = 0;
            Vector3 dir = mainCamera.transform.position;
            Vector2 v2 = new Vector2(dir.x, dir.y);
            float angle = Vector2.Angle(Vector2.up, v2);
            Vector3 eulerAngles = new Vector3(0, 0, angle);
            compassBtn.transform.eulerAngles = eulerAngles;
        }
        
    }

    public override void InitUI()
    {
        base.InitUI();
        initOperateUIPose();
        infoUI.gameObject.SetActive(false);
        initOperateUI();

        EventUtil.AddListener(GlobalEvent.Module_TO_UI_Action ,onModuleAction);

        worldMapGlobeControl = FindObjectOfType<WorldMapGlobeControl>();
        worldMapGlobeControl.onLatLonUpdate = onLatLonUpdate;

        initModeUI();

        toolUI.InitUI();
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

            if(modeListToggle.name == "默认模式")
            {
                defaultToggle = modeListToggle;
            }
        }
        defaultToggle.isOn = true;
    }

    private Toggle currToggle;

    private void onModelListToggleValueChanged(Toggle toggle) 
    {
        currToggle = toggle;
        Text label = toggle.transform.Find("Label").GetComponent<Text>();
        if (toggle.isOn)
        {
            if (!seletModeTxt.gameObject.activeSelf)
            {
                seletModeTxt.gameObject.SetActive(true);
            }
            seletModeTxt.text = label.text;
            label.color = modeLightColor;
            //Invoke("delayChangeToStyle", 0.1f);
            StartCoroutine(ChangeToStyle());
        }
        else
        {
            label.color = Color.white;
        }
    }

    IEnumerator ChangeToStyle()
    {
        //Loading.SetActive(true);
        yield return new WaitForEndOfFrame();
        EventUtil.DispatchEvent(GlobalEvent.UI_TO_Module_Action, "Style", currToggle.name);
        //yield return new WaitForSeconds(1);
        //Loading.SetActive(false);
        yield return null;
    }

    private void delayChangeToStyle()
    {
        EventUtil.DispatchEvent(GlobalEvent.UI_TO_Module_Action, "Style", currToggle.name);
    }

    /// <summary>
    /// 打开关闭地球风格界面
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
    /// 初始化右侧列表打开、关闭的位置
    /// </summary>
    private void initOperateUIPose()
    {
        openPose = operateUITran.localPosition; 
        closePose = openPose; 
        closePose.x = openPose.x + 100;
    }

    /// <summary>
    /// 设置右侧列表打开关闭标志
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
    /// 模块事件处理
    /// </summary>
    /// <param name="eventArgs"></param>
    private void onModuleAction(CustomEventArgs eventArgs)
    {
        string action = ((string)eventArgs.args[0]).ToLower();
        //省份、城市信息展示
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
        else if(action == "toggle")
        {
            ///洲、国家、省份、城市
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
        if(!isOn && (action == "Province" || action == "City"))
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
