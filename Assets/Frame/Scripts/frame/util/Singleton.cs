//***************************************************
// Des：BoboAR
// Author:KaKa 
// Email：jiangjian@prismostudio.io
// Mobile：17321367005
// CreateTime：2020/12/21 17:46:03
// Version：v 0.1
// @CopyRight：上海琉森教育科技有限公司
// ***************************************************
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace com.frame
{
    public class Singleton<T> where T : class, new()
    {
        private static T instance = default (T);

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T ();
                }
                return instance;
            }
        }

    }

}