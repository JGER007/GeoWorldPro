using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UniLang;
using UnityEngine.UI;
using System.IO;

public class GeoCountryName : MonoBehaviour
{
    [SerializeField]
    private Slider translateSlider;
    [SerializeField]
    private Text translateText;

    private Dictionary<string, ZH_ENElement> zh_ENDic;

    private ZH_ENList zH_ENList;

    private List<string> enList;
    private int index = 0;

    private int totalCount;

    
    // Start is called before the first frame update
    void Start()
    {
        initZH_ENData();
        ReplaceCountryEnToZn();
        ReplaceProvinceEnToZn();
        ReplaceCityEnToZn();
    }

    private void initZH_ENData()
    {
        string zh_enData = Resources.Load<TextAsset>("Geodata/zh_en").text;
        string zhLabes = "";
        zH_ENList = JsonUtility.FromJson<ZH_ENList>(zh_enData);
        zh_ENDic = new Dictionary<string, ZH_ENElement>();
        foreach (ZH_ENElement zH_ENElement in zH_ENList.ZH_EN)
        {
            //Debug.Log(zH_ENElement.EN);
            zh_ENDic.Add(zH_ENElement.EN, zH_ENElement);
            zhLabes = zhLabes + zH_ENElement.ZH;
        }

        File.WriteAllText("Geodata/zhlabels.txt", zhLabes);
    }


    public void ReplaceCityEnToZn() 
    {
        string provinceData = Resources.Load<TextAsset>("Geodata/cities10").text;
        string[] cities = provinceData.Split('|');
        List<string> zhcities = new List<string>();
        for (int i = 0; i < cities.Length; i++)
        {
            string cityConfig = cities[i];
            string[] parts = cityConfig.Split('$');

            string city = parts[0];
            string province = parts[1];
            string country = parts[2];

            if (zh_ENDic.ContainsKey(city))
            {
                parts[0] = zh_ENDic[city].ZH;
            }

            if (zh_ENDic.ContainsKey(province))
            {
                parts[1] = zh_ENDic[province].ZH;
            }

            if (zh_ENDic.ContainsKey(country))
            {
                parts[2] = zh_ENDic[country].ZH;
            }
            zhcities.Add(string.Join("$", parts));
        }
        File.WriteAllText("Geodata/cities10.txt", string.Join("|", zhcities));
    }


        public void ReplaceProvinceEnToZn() 
    {
        string provinceData = Resources.Load<TextAsset>("Geodata/provinces10").text;
        string[] provinces = provinceData.Split('|');

        List<string> zhprovinces = new List<string>();
        for (int i = 0; i < provinces.Length; i++)
        {
            string provinceConfig = provinces[i];
            string[] parts = provinceConfig.Split('$');
            string province = parts[0];
            string country = parts[1];

            if (zh_ENDic.ContainsKey(province))
            {
                parts[0] = zh_ENDic[province].ZH;
            }

            if (zh_ENDic.ContainsKey(country))
            {
                parts[1] = zh_ENDic[country].ZH;
            }
            zhprovinces.Add(string.Join("$", parts));
        }
        File.WriteAllText("Geodata/provinces10.txt", string.Join("|", zhprovinces));

    }

    public void ReplaceCountryEnToZn()
    {
        string countryData = Resources.Load<TextAsset>("Geodata/countries10").text;
        string[] countries = countryData.Split('|');
        List<string> zhcountries = new List<string>();
        for (int i = 0; i < countries.Length; i++)
        {
            string countryConfig = countries[i];
            string[] parts = countryConfig.Split('$');
            string continent = parts[0];
            string country = parts[1];

            if (zh_ENDic.ContainsKey(continent) )
            {
                parts[0] = zh_ENDic[continent].ZH;
            }

            if (zh_ENDic.ContainsKey(country))
            {
                parts[1] = zh_ENDic[country].ZH;
            }
            zhcountries.Add(string.Join("$", parts));

            translateText.text = "ReplaceCountryEnToZn(" + i + "/" + countries.Length + ")";
        }
        string zhcountriestr = string.Join("|", zhcountries);
        Debug.Log(zhcountriestr);
        File.WriteAllText("Geodata/countries10.txt", zhcountriestr);
        
    }

    public void DealZH_CNDic() 
    {
        zH_ENList = new ZH_ENList();
        zH_ENList.ZH_EN = new List<ZH_ENElement>();
        zh_ENDic = new Dictionary<string, ZH_ENElement>();
        enList = new List<string>();
        //parseZH_EN();
        parseCountryInfo();
        parseProvinceInfo();
        parseCityInfo();

        index = 0;
        totalCount = enList.Count;
        translateSlider.maxValue = totalCount;
        setTranslateProgress(index);

        translate();
    }

    private void setTranslateProgress(int progress)
    {
        translateSlider.value = progress;
        if(progress < totalCount)
        {
            translateText.text = "翻译:" + enList[progress] +   "进度:" + progress + "/" + totalCount;
        }
        else 
        {
            translateText.text = "翻译进度:翻译完成" ;

            string json = JsonUtility.ToJson(zH_ENList);
            File.WriteAllText("zh_en.json", json);
            Debug.Log(json);
        }
    }

    private void translate()
    {
        if(index < totalCount)
        {
            string en = enList[index];
            if(!zh_ENDic.ContainsKey(en))
            {
                Translator.Do("en", "zh-cn", en, (zh) => 
                {
                    ZH_ENElement zH_ENElement = new ZH_ENElement();
                    zH_ENElement.ZH = zh;
                    zH_ENElement.EN = en;
                    zh_ENDic.Add(en, zH_ENElement);
                    zH_ENList.ZH_EN.Add(zH_ENElement);
                    Debug.Log(en + ":" + zh);
                    index++;
                    setTranslateProgress(index);
                    translate();
                });
            }
            else
            {
                index++;
                setTranslateProgress(index);
                translate();
            }
           
        }
    }






    private void parseZH_EN()
    {
        JsonData zH_ENJsonData = new JsonData();

        string zh_enData = Resources.Load<TextAsset>("Geodata/Label_ZH_EN").text;
        JsonData jd = JsonMapper.ToObject(zh_enData);

        IDictionary dict = jd as IDictionary; //第一个子元素中包含多个子元素
        foreach (string key in dict.Keys)
        {
            JsonData jsonData = (JsonData)dict[key];
            string[] ens = (jsonData["EN"].ToString()).Split('#');
            string[] zhs = (jsonData["ZH"].ToString()).Split('#');


            Debug.Log("-----------------------------------------------------");
            Debug.Log(key + " ens" + ens.Length + ",zhs:" +  zhs.Length);
            for(int i =0;i< ens.Length; i++)
            {
                Debug.Log(ens[i] + ":" + zhs[i]);
            }

        }
    }

    private void parseCityInfo() 
    {
        string provinceData = Resources.Load<TextAsset>("Geodata/cities10").text;
        string[] cities = provinceData.Split('|');

        //string citystr = "";

        for (int i = 0; i < cities.Length; i++)
        {
            string cityConfig = cities[i];
            string[] parts = cityConfig.Split('$');

            string city = parts[0];
            string province = parts[1];
            string country = parts[2];

            if (country == "China")
            {
                enList.Add(city);
                /*if (!citystr.Contains(citystr))
                {
                    citystr = citystr + "#" + city;
                }*/
            }
        }
        //Debug.Log(citystr);
    }


    private void parseProvinceInfo() 
    {
        string provinceData = Resources.Load<TextAsset>("Geodata/provinces10").text;
        string[] provinces = provinceData.Split('|');

        //string provincestr = "";

        for (int i = 0; i < provinces.Length; i++)
        {
            string provinceConfig = provinces[i];
            string[] parts = provinceConfig.Split('$');

            string province = parts[0];
            string country = parts[1];

            if (country == "China")
            {
                enList.Add(province);
                
                /*if (!provincestr.Contains(province))
                {
                    enList.Add(province);
                    //provincestr = provincestr + "#" + province;
                }*/
            }
        }
        //Debug.Log(provincestr);
    }


    private void parseCountryInfo()
    {
        string countryData = Resources.Load<TextAsset>("Geodata/countries10").text;
        string[] countries = countryData.Split('|');

        //string continentstr = "";
       // string countrystr = "";

        for (int i = 0; i < countries.Length; i++)
        {
            string countryConfig = countries[i];
            string[] parts = countryConfig.Split('$');

            string continent = parts[0];
            string country = parts[1];
            enList.Add(continent);
            enList.Add(country);

            /*
            if (!continentstr.Contains(country))
            {
                continentstr = continentstr + country + "#";
            }
            if (!countrystr.Contains(continent))
            {
                countrystr = countrystr + "#" + continent;
            }*/
        }

        //Debug.Log(continentstr);
        //Debug.Log(countrystr);
    }



}


[Serializable]
public class ZH_EN
{
    public ZH_ENElement continent;
    public ZH_ENElement country;
    public ZH_ENElement privince; 
    public ZH_ENElement city; 
}

[Serializable]
public class ZH_ENList 
{
    public List<ZH_ENElement> ZH_EN;
}

[Serializable]
public class ZH_ENElement
{
    public string EN;
    public string ZH;
}






