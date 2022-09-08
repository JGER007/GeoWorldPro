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
        TipText.text = "��Ƭ��Դ�����...";

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
    /// ������Ƭ��Դ
    /// </summary>
    private void loadTileZip()
    {
        TipText.text = "��ʼ��Ƭ��Դ����";
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
            TipText.text = "��Ƭ��Դ����:" + progress + "%";
            LoadProgressBar.value = www.progress;
            yield return 1;
        }
        if (www.error != null)
        {
            TipText.text = "��Ƭ��Դ����ʧ��";
        }
        else
        {
            TipText.text = "��Ƭ��Դ�������";
            LoadProgressBar.value = 1;

            var data = www.bytes;
            File.WriteAllBytes(zipPath, data);

            Invoke("delayUnzip", 0.1f);
            // ����ʹ�ü���
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


    string unzipTextFormat = "��Ƭ��Դ��ѹ:{0}/19554";
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
            TipText.text = "��Ƭ��Դ��ѹ���";
        }
        else
        {
            TipText.text = string.Format(unzipTextFormat, fileCount);
        }
    }

    /// <summary>UI�˳�</summary>
    public override void OnQuitGame()
    {
        Destroy(gameObject);
    }
}
