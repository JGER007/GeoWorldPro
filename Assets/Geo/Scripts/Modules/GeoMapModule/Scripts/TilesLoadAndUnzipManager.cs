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
    #region unzip


    public void UnZipTiles()
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
            ZipWrapper.UnzipFile(zipFilePath, zipOutputPath, null, new UnzipCallback());
        }
       
    }

    public class UnzipCallback : ZipWrapper.UnzipCallback
    {
        public override bool OnPreUnzip(ZipEntry _entry)
        {
            //Debug.Log("OnPreUnzip Name£º " + _entry.Name);
            //Debug.Log("OnPreUnzip IsFile£º" + _entry.IsFile);
            return base.OnPreUnzip(_entry);
        }

        public override void OnPostUnzip(ZipEntry _entry)
        {
            //Debug.Log("OnPostUnzip Name£º " + _entry.Name);
            base.OnPostUnzip(_entry);
        }

        public override void OnFinished(bool _result)
        {
            //Debug.Log("OnUnZipFinished _result£º " + _result);
            base.OnFinished(_result);
            EventUtil.DispatchEvent(GlobalEvent.Tiles_Unzip_Finish, _result);
        }
    }
    #endregion
}
