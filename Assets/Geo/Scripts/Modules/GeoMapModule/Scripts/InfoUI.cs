using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoUI : MonoBehaviour
{
    [SerializeField]
    private Text titleTxt;

    [SerializeField]
    private Text infoTxt;

    [SerializeField]
    private Text tipTxt;

    private string tipCity = "数据来源：中国城市统计年鉴2020";
    private string tipProvince = "数据来源：2020年第七次全国人口普查主要数据";

    public void SetInfo(InfoVO infoVO)
    {
        titleTxt.text = infoVO.Tilte;
        infoTxt.text = infoVO.GetInfo();
        tipTxt.gameObject.SetActive(true);
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
