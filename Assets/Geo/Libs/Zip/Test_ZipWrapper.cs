using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;

public class Test_ZipWrapper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.dataPath);
        //Test_Zip();
        //Test_UnZip();
    }

    #region Test_Zip


    void Test_Zip()
    {
        string[] zipFilePaths = new string[] 
        {
            Application.dataPath+"/TilesCache"
        };

        string zipOutputPath = Application.streamingAssetsPath + "/TilesCache.zip";
        ZipWrapper.Zip(zipFilePaths, zipOutputPath, "password", new ZipCallback());
    }



    public class ZipCallback : ZipWrapper.ZipCallback
    {
        public override bool OnPreZip(ZipEntry _entry)
        {
            Debug.Log("OnPreZip Name?? " + _entry.Name);
            Debug.Log("OnPreZip IsFile??" + _entry.IsFile);
            return base.OnPreZip(_entry);
        }

        public override void OnPostZip(ZipEntry _entry)
        {
            Debug.Log("OnPostZip Name?? " + _entry.Name);
        }

        public override void OnFinished(bool _result)
        {
            Debug.Log("OnZipFinished _result?? " + _result);
        }

    }

    #endregion

    #region Test_UnZip

    void Test_UnZip()
    {

        string zipFilePath = Application.streamingAssetsPath + "/TilesCache.zip";
        string zipOutputPath = Application.streamingAssetsPath ;

        //ZipWrapper.UnzipFile(zipFilePath, zipOutputPath, "LS123456", new UnzipCallback());
    }

    public class UnzipCallback : ZipWrapper.UnzipCallback
    {
        public override bool OnPreUnzip(ZipEntry _entry)
        {
            Debug.Log("OnPreUnzip Name?? " + _entry.Name);
            Debug.Log("OnPreUnzip IsFile??" + _entry.IsFile);
            return base.OnPreUnzip(_entry);
        }

        public override void OnPostUnzip(ZipEntry _entry)
        {
            Debug.Log("OnPostUnzip Name?? " + _entry.Name);
            base.OnPostUnzip(_entry);
        }

        public override void OnFinished(bool _result)
        {
            Debug.Log("OnUnZipFinished _result?? " + _result);
            base.OnFinished(_result);
        }
    }

    #endregion
}