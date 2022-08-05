using System;
using System.IO;
using UnityEngine;

/// <summary>存放路径</summary>
public class Paths
{

    /// <summary>相册路径</summary>
    public static string AlbumPath
    {
        get
        {
            string photoAlbumPath = string.Empty;
            string photoPath = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    //photoAlbumPath = Application.persistentDataPath.Substring(0, Application.persistentDataPath.IndexOf("Android", StringComparison.Ordinal));
                    //photoPath = photoAlbumPath + "DCIM/Camera/" + "Bobo_" + GetTimeStamp();
                    //判断目录是否存在，不存在则会创建目录
                    photoPath = Application.persistentDataPath + "/Bobo_" + GetTimeStamp();
                    Debug.LogError(" ----------------------路经-----" + photoPath);
                    break;
                case RuntimePlatform.IPhonePlayer:
                    photoPath = Application.persistentDataPath + "/Bobo_" + GetTimeStamp();
                    break;
                default:
                    if (!Directory.Exists(Application.dataPath + "/PhotoAlbum"))
                    {
                        Directory.CreateDirectory(Application.dataPath + "/PhotoAlbum");
                    }
                    photoPath = Application.dataPath + "/PhotoAlbum/Bobo_" + GetTimeStamp();
                    break;
            }

            return photoPath;
        }
    }


    /// <summary>
    /// 获取时间戳
    /// </summary>
    /// <param name="bflag"></param>
    /// <returns></returns>
    public static long GetTimeStamp (bool bflag = false)
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime (1970, 1, 1, 0, 0, 0, 0);
        long ret;
        if (bflag) ret = Convert.ToInt64 (ts.TotalSeconds);
        else ret = Convert.ToInt64 (ts.TotalMilliseconds);
        return ret;
    }

}
