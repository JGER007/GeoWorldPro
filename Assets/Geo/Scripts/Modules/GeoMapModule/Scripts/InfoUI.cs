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

    private string tipCity = "������Դ���й�����ͳ�����2020";
    private string tipProvince = "������Դ��2020����ߴ�ȫ���˿��ղ���Ҫ����";

    public void SetInfo(InfoVO infoVO)
    {
        titleTxt.text = infoVO.Tilte;
        infoTxt.text = infoVO.GetInfo();
        tipTxt.gameObject.SetActive(true);
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
