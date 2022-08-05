using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndScaleController : MonoBehaviour
{
    float speed = 2000.0f;
    float x;
    float y; 

    void Update()
    {
        if (Input.GetMouseButton(0))
        {//鼠标按着左键移动 
            y = -Input.GetAxis("Mouse X") * Time.deltaTime * speed;
            x = -Input.GetAxis("Mouse Y") * Time.deltaTime * speed;
        }
        else
        {
            x = y = 0;
        }

        //旋转角度（增加）
        transform.Rotate(new Vector3(x, y, 0));
        //用于平滑旋转至自定义目标 
        //pinghuaxuanzhuan();
    }

    bool iszhuan = false;
    Quaternion targetRotation;

    void pinghuaxuanzhuan()
    {
        if (iszhuan)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 3);
        }
    }
}
