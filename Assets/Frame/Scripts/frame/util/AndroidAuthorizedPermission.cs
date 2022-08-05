//***************************************************
// Des：Android 动态权限检查
// Author:KaKa 
// Email：jiangjian@prismostudio.io
// Mobile：17321367005
// CreateTime：2020/12/15 10:16:06
// Version：v 0.1
// @CopyRight：上海琉森教育科技有限公司
// ***************************************************
using System.Collections;
using UnityEngine;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

namespace com.frame
{
    public class AndroidAuthorizedPermission : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start ()
        {
            StartCoroutine (WaitPermissions ());
        }

        private IEnumerator WaitPermissions ()
        {
#if PLATFORM_ANDROID
            while (!Permission.HasUserAuthorizedPermission (Permission.ExternalStorageRead))
            {
                Permission.RequestUserPermission (Permission.ExternalStorageRead);
                yield return new WaitForSeconds (1f);
            }
            while (!Permission.HasUserAuthorizedPermission (Permission.ExternalStorageWrite))
            {
                Permission.RequestUserPermission (Permission.ExternalStorageWrite);
                yield return new WaitForSeconds (1f);
            }
            while (!Permission.HasUserAuthorizedPermission (Permission.Camera))
            {
                Permission.RequestUserPermission (Permission.Camera);
                yield return new WaitForSeconds (1f);
            }
#endif

            yield return null;
        }
    }
}
