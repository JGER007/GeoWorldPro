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
    private Text latLonTxt;

    [SerializeField]
    private Transform operateUITran;

    private bool openFlag = true;

    [SerializeField]
    private Dropdown styleDropDown;
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

        closeBtn.gameObject.SetActive(true);

        closeBtn.onClick.AddListener(delegate { setOpenFlag(false); });
        openBtn.onClick.AddListener(delegate { setOpenFlag(true); });
        openFlag = true;

        styleDropDown.onValueChanged.AddListener(delegate { onStyleDropDownValueChanged(); });

        countryToggle.onValueChanged.AddListener(delegate { onToggleValueChanged("Country", countryToggle.isOn); });

        provinceToggle.onValueChanged.AddListener(delegate { onToggleValueChanged("Province", provinceToggle.isOn); });

        cityToggle.onValueChanged.AddListener(delegate { onToggleValueChanged("City", cityToggle.isOn); });

        EventUtil.AddListener(GlobalEvent.Module_TO_UI_Action ,onModuleAction);

        worldMapGlobeControl = FindObjectOfType<WorldMapGlobeControl>();
        worldMapGlobeControl.onLatLonUpdate = onLatLonUpdate;
        //operateUITran.transform.localPosition = new Vector3(-25, -30, 0);

        styleDropDown.value = 0;
    }

    private void onLatLonUpdate(string obj)
    {
        latLonTxt.text = obj;
    }

    private void initoperateUIPose()
    {
        openPose = operateUITran.localPosition; 
        closePose = openPose; 
        closePose.x = openPose.x + 413;
    }

    private void setOpenFlag(bool flag)
    {
        if(openFlag != flag)
        {
            openFlag = flag;
            tweenMove();
        }
    }

    private void onModuleAction(CustomEventArgs eventArgs)
    {
        string action = (string)eventArgs.args[0];
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
        else if(action == "style")
        {
            StyleEnum styleEnum = (StyleEnum)eventArgs.args[1];
            foreach(OptionData option in styleDropDown.options)
            {
                if(option.text == styleEnum.ToString())
                {
                    styleDropDown.value = styleDropDown.options.IndexOf(option);
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

    private void onStyleDropDownValueChanged()
    {
        string styleName = styleDropDown.options[ styleDropDown.value].text;
        EventUtil.DispatchEvent(GlobalEvent.UI_TO_Module_Action, "Style", styleName);
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
