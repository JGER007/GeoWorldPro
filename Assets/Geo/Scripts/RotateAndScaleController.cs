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
        {//��갴������ƶ� 
            y = -Input.GetAxis("Mouse X") * Time.deltaTime * speed;
            x = -Input.GetAxis("Mouse Y") * Time.deltaTime * speed;
        }
        else
        {
            x = y = 0;
        }

        //��ת�Ƕȣ����ӣ�
        transform.Rotate(new Vector3(x, y, 0));
        //����ƽ����ת���Զ���Ŀ�� 
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
