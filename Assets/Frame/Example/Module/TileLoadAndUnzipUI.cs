using com.frame;
using com.frame.ui;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TileLoadAndUnzipUI : PopUpUI
{
    private TilesLoadAndUnzipManager tilesLoadAndUnzipManager;

    private Text TipText;

    private Slider LoadProgressBar;

    public override void InitUI()
    {
        TipText = transform.Find("TipText").GetComponent<Text>();
        LoadProgressBar = transform.Find("LoadProgressBar").GetComponent<Slider>();
        LoadProgressBar.gameObject.SetActive(false);
        TipText.text = "瓦片资源检查中...";

        tilesLoadAndUnzipManager = GetComponent<TilesLoadAndUnzipManager>();

        bool isUnZip = checkIsTileUnzip();
        if (isUnZip)
        {
            EventUtil.DispatchEvent(GlobalEvent.Tiles_Unzip_Finish, true);
        }
        else
        {
            loadTileZip();
        }
    }

    /// <summary>
    /// 加载瓦片资源
    /// </summary>
    private void loadTileZip()
    {
        TipText.text = "开始瓦片资源加载";
        LoadProgressBar.value = 0;
        LoadProgressBar.gameObject.SetActive(true);
        string zipFilePath = Application.persistentDataPath + "/TilesCache.zip";
        string zipOutputPath = Application.persistentDataPath;
        //ZipWrapper.UnzipFile(zipFilePath, zipOutputPath, "LS123456", new UnzipCallback());

        string url = "http://192.168.10.31:81/TilesCache.zip";
        //url = "http://vpn.prismostudio.cn:8001/TilesCache.zip";
        StartCoroutine(LoadAndUnzip(url, zipFilePath, zipOutputPath));
    }



    IEnumerator LoadAndUnzip(string url, string zipPath, string exportPath)
    {
        WWW www = new WWW(url);
        while (!www.isDone)
        {
            int progress = (((int)(www.progress * 100)) % 100);
            TipText.text = "瓦片资源加载:" + progress + "%";
            LoadProgressBar.value = www.progress;
            yield return 1;
        }
        if (www.error != null)
        {
            TipText.text = "瓦片资源加载失败";
        }
        else
        {
            TipText.text = "瓦片资源加载完成";
            LoadProgressBar.value = 1;

            var data = www.bytes;
            File.WriteAllBytes(zipPath, data);

            Invoke("delayUnzip", 0.1f);
            // 酌情使用即可
            //File.Delete(zipPath);
            //Directory.Delete(exportPath, true);
        }
    }


    private bool checkIsTileUnzip()
    {
        bool isUnZip = false;
        if (Directory.Exists(Application.persistentDataPath + "/TilesCache"))
        {
            DirectoryInfo tileDirectoryInfo = new DirectoryInfo(Application.persistentDataPath + "/TilesCache");
            if (tileDirectoryInfo.GetFiles().Length > 0)
            {
                isUnZip = true;
            }
        }
        return isUnZip;
    }


    string unzipTextFormat = "瓦片资源解压:{0}/19554";
    private void delayUnzip()
    {
        LoadProgressBar.value = 0;
        TipText.text = string.Format(unzipTextFormat,0);
        Invoke("UnZipTiles", 0.2f);
    }

    private void UnZipTiles()
    {
        tilesLoadAndUnzipManager.UnZipTiles(unzipProcess);
    }

    private void unzipProcess (int fileCount)
    {
        LoadProgressBar.value = fileCount/19554.0f;
        if(fileCount >= 19554)
        {
            TipText.text = "瓦片资源解压完成";
        }
        else
        {
            TipText.text = string.Format(unzipTextFormat, fileCount);
        }
    }

    /// <summary>UI退出</summary>
    public override void OnQuitGame()
    {
        Destroy(gameObject);
    }
}
