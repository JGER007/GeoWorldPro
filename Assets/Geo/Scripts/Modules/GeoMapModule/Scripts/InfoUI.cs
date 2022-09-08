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

    private string tipCity = "������Դ���й�����ͳ�����2020";
    private string tipProvince = "������Դ��2020����ߴ�ȫ���˿��ղ���Ҫ����";

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


        if (infoVO.Country == "�й�")
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
