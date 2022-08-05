//***************************************************
// Des：双指缩放3D物体
// Author:KaKa 
// Email：jiangjian@prismostudio.io
// Mobile：17321367005
// CreateTime：2021/01/28 18:40:11
// Version：v 0.1
// @CopyRight：上海琉森教育科技有限公司
// ***************************************************
namespace com.frame.tool
{
    using UnityEngine;

    public class ZoomControl : MonoBehaviour
    {
        /// <summary>
        /// 上次触摸点1(手指1)
        /// </summary>
        private Touch oldTouch1;
        /// <summary>
        /// 上次触摸点2(手指2)
        /// </summary>
        private Touch oldTouch2;

        /// <summary>
        /// 用于显示滑动距离
        /// </summary>
        private float oldDis = 0;
        private float newDis = 0;
        private float scaler = 0;


        private float min = 0.5f;
        private float max = 3f;


        private void Update ()
        {
            //没有触摸
            if (Input.touchCount > 1)
            {
                //多点触摸, 放大缩小
                Touch newTouch1 = Input.GetTouch (0);
                Touch newTouch2 = Input.GetTouch (1);

                //第2点刚开始接触屏幕, 只记录，不做处理
                if (newTouch2.phase == TouchPhase.Began)
                {
                    oldTouch2 = newTouch2;
                    oldTouch1 = newTouch1;
                    return;
                }

                //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型
                float oldDistance = Vector2.Distance (oldTouch1.position, oldTouch2.position);
                float newDistance = Vector2.Distance (newTouch1.position, newTouch2.position);
                oldDis = oldDistance;
                newDis = newDistance;

                //两个距离之差，为正表示放大手势， 为负表示缩小手势
                float offset = newDistance - oldDistance;

                //放大因子， 一个像素按 0.01倍来算(100可调整)
                float scaleFactor = offset / 300f;
                Vector3 localScale = transform.localScale;
                Vector3 scale = new Vector3 (localScale.x + scaleFactor,
                                            localScale.y + scaleFactor,
                                            localScale.z + scaleFactor);
                scaler = scaleFactor;

                //允许模型最小缩放到 0.5 倍最大放大3倍
                if (scale.x > min)
                {
                    //实用差值运算，模型平滑缩放
                    transform.localScale = Vector3.Lerp (transform.localScale, new Vector3 (Mathf.Clamp (localScale.x + scaleFactor, min, max),
                                                       Mathf.Clamp (localScale.y + scaleFactor, min, max),
                                                       Mathf.Clamp (localScale.z + scaleFactor, min, max)), 1f);
                }
                //记住最新的触摸点，下次使用
                oldTouch1 = newTouch1;
                oldTouch2 = newTouch2;
            }
        }
    }
}