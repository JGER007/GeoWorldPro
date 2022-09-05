using com.frame;
using com.frame.ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolUI : BaseUI
{
    
    private Text LatLonTxt;
    private Toggle LatLonLine;
    private Toggle LatLon;
    private Toggle Rule;

    public override void InitUI()
    {
        LatLonTxt = transform.Find("LatLonTxt").GetComponent<Text>();
        LatLonTxt.text = "";

        LatLonLine = transform.Find("LatLonLine").GetComponent<Toggle>();
        LatLonLine.isOn = false;
        LatLonLine.onValueChanged.AddListener(delegate { onLatLonLineValueChanged(); });

        LatLon = transform.Find("LatLon").GetComponent<Toggle>();
        LatLon.isOn = false;
        LatLon.onValueChanged.AddListener(delegate { onLatLonValueChanged(); });

        Rule = transform.Find("Rule").GetComponent<Toggle>();
        Rule.isOn = false;
        LatLonLine.onValueChanged.AddListener(delegate { onRuleValueChanged(); });

    }

    private void onLatLonLineValueChanged()
    {
        EventUtil.DispatchEvent(GlobalEvent.UI_TO_Module_Action, "LatLonLine", LatLonLine.isOn);
    }

    private void onLatLonValueChanged()
    {
        if(!LatLon.isOn)
        {
            LatLonTxt.text = "";
        }
        LatLonTxt.gameObject.SetActive(LatLon.isOn);
        EventUtil.DispatchEvent(GlobalEvent.UI_TO_Module_Action, "LatLon", LatLon.isOn);
    }

    private void onRuleValueChanged() 
    {
        EventUtil.DispatchEvent(GlobalEvent.UI_TO_Module_Action, "Rule", Rule.isOn);
    }

    public void ShowLatLon(string value)
    {
        LatLonTxt.text = value;
    }

}
