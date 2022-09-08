using com.frame;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TilesLoadAndUnzipManager :MonoBehaviour
{
   
    public Action<bool> OnTilesUnZipFinished;

    public Action<bool> OnTilesUnZipProcess; 
    #region unzip


    public void UnZipTiles(Action<int> unZipProcessAction)
    {
        bool isUnZip = false;
        if (Directory.Exists(Application.persistentDataPath + "/TilesCache"))
        {
            DirectoryInfo tileDirectoryInfo = new DirectoryInfo(Application.persistentDataPath + "/TilesCache");
            if (tileDirectoryInfo.GetFiles().Length > 0)
            {
                isUnZip = true;
                EventUtil.DispatchEvent(GlobalEvent.Tiles_Unzip_Finish, true);
            }
        }

        if(!isUnZip)
        {
            string zipFilePath = Application.persistentDataPath + "/TilesCache.zip";
            string zipOutputPath = Application.persistentDataPath;

            UnzipCallback unzipCallback = new UnzipCallback();
            FindObjectOfType<ZipWrapper>().UnzipFile(zipFilePath, zipOutputPath, null, new UnzipCallback(), unZipProcessAction);
        }
    }

    public class UnzipCallback : ZipWrapper.UnzipCallback
    {
        //int count;
        public override bool OnPreUnzip(ZipEntry _entry)
        {
            return base.OnPreUnzip(_entry);
        }

        public override void OnPostUnzip(ZipEntry _entry)
        {
            //Debug.Log(count++);
            base.OnPostUnzip(_entry);
        }

        public override void OnFinished(bool _result)
        {
            base.OnFinished(_result);
            EventUtil.DispatchEvent(GlobalEvent.Tiles_Unzip_Finish);
        }
    }
    #endregion
}
