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
    private Toggle modelToggle; 
    [SerializeField]
    private GameObject modeList;

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


    private WorldMapGlobeControl worldMapGlobeControl;

    void Start()
    {
        //InitUI();
    }

    public override void InitUI()
    {
        base.InitUI();
        initoperateUIPose();
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
        modeList.SetActive(modelToggle.isOn);

        seletModeTxt = modelToggle.transform.Find("Text").GetComponent<Text>();
        seletModeTxt.gameObject.SetActive(false);

        modelToggle.onValueChanged.AddListener(delegate { onModelValueChanged(); });

        modeListToggles = modeList.GetComponentsInChildren<Toggle>();
        foreach(Toggle modeListToggle in modeListToggles)
        {
            modeListToggle.onValueChanged.AddListener(delegate { onModelListToggleValueChanged(modeListToggle); });
        }
    }

    private void onModelListToggleValueChanged(Toggle toggle) 
    {
        Text label = toggle.transform.Find("Label").GetComponent<Text>();
        if (toggle.isOn)
        {
            if (!seletModeTxt.gameObject.activeSelf)
            {
                seletModeTxt.gameObject.SetActive(true);
            }
            seletModeTxt.text = label.text;
            label.color = modeLightColor;

            EventUtil.DispatchEvent(GlobalEvent.UI_TO_Module_Action, "Style", toggle.name);
        }
        else
        {
            label.color = Color.white;
        }
    }

    private void onModelValueChanged()
    {
        modeList.SetActive(modelToggle.isOn);
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
        continentToggle.onValueChanged.AddListener(delegate { onToggleValueChanged("Continent", countryToggle.isOn); });
        countryToggle.onValueChanged.AddListener(delegate { onToggleValueChanged("Country", countryToggle.isOn); });
        provinceToggle.onValueChanged.AddListener(delegate { onToggleValueChanged("Province", provinceToggle.isOn); });
        cityToggle.onValueChanged.AddListener(delegate { onToggleValueChanged("City", cityToggle.isOn); });
    }

    private void onLatLonUpdate(string value)
    {
        toolUI.ShowLatLon(value);
    }

    private void initoperateUIPose()
    {

        openPose = operateUITran.localPosition; 
        closePose = openPose; 
        closePose.x = openPose.x + 100;
    }

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

    private void onModuleAction(CustomEventArgs eventArgs)
    {
        string action = ((string)eventArgs.args[0]).ToLower();
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

                bool isOn = (bool)eventArgs.args[2];
                if(toggle != null)
                {
                    toggle.isOn = isOn;
                }
            }
            
        }
        else if(action == "style")
        {
            StyleEnum styleEnum = (StyleEnum)eventArgs.args[1];

            foreach (Toggle modeListToggle in modeListToggles)
            {
                if(modeListToggle.name == styleEnum.ToString())
                {
                    modeListToggle.isOn = true;
                    return;
                }
            }
        }
    }

    private void onToggleValueChanged(string action,bool isOn)  
    {
        EventUtil.DispatchEvent(GlobalEvent.UI_TO_Module_Action, action, isOn);

        if(!isOn && (action == "Province" || action == "City"))
        {
            infoUI.gameObject.SetActive(false);
        }

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
