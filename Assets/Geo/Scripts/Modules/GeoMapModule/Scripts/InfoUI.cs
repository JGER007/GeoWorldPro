using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoUI : MonoBehaviour
{
    [SerializeField]
    private GameObject BG01;

    [SerializeField]
    private GameObject BG02; 

    [SerializeField]
    private Text titleTxt;

    [SerializeField]
    private Text infoTxt;

    [SerializeField]
    private Text positionInfo; 

    [SerializeField]
    private Text tipTxt;

    private string tipCity = "数据来源：中国城市统计年鉴2020";
    private string tipProvince = "数据来源：2020年第七次全国人口普查主要数据";

    public void SetInfo(InfoVO infoVO)
    {
        titleTxt.text = infoVO.Tilte;
        positionInfo.text = infoVO.getPositionInfo();
        string info = infoVO.GetInfo();
        infoTxt.text = info;
        tipTxt.gameObject.SetActive(true);

        if(!string.IsNullOrEmpty(info))
        {
            BG01.SetActive(true);
            BG02.SetActive(false);
        }
        else
        {
            BG01.SetActive(false);
            BG02.SetActive(true); 
            tipTxt.gameObject.SetActive(false);
        }


        if (infoVO.Country == "中国")
        {
            if (infoVO.cityVO != null)
            {
                tipTxt.text = tipCity;
            }
            else if (infoVO.provinceDataVO != null)
            {
                tipTxt.text = tipProvince;
            }
            else
            {
                tipTxt.gameObject.SetActive(false);
            }
        }
        else
        {
            tipTxt.gameObject.SetActive(false);
        }
        
    }
}
