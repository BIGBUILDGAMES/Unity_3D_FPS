using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float rotateSpeed = 120;

    float mouseX; // 마우스 X각도
    float mouseZ; // 마우스 X각도

    void LateUpdate()
    {
        float horizontal = Input.GetAxis("Mouse X"); // 마우스의 X축 움직임을 감지
        float vertical = Input.GetAxis("Mouse Y"); // 마우스의 Y축 움직임을 감지

        mouseX += horizontal * rotateSpeed * Time.deltaTime; // 마우스 X각도를 누적해준다.
        mouseZ += vertical * rotateSpeed * Time.deltaTime; // 마우스 Y각도를 누적해준다.

        //if (mouseZ >= 90)   // mouseY가 90도 이상이면
        //{
        //    mouseZ = 90;    // mouseY 90도 고정
        //}
        //else if (mouseZ <= -90)   // mouseY가 90도 이상이면
        //{
        //    mouseZ = -90;    // mouseY 90도 고정
        //}
        transform.eulerAngles = new Vector3(
            gameObject.transform.rotation.x,
            mouseX,
            gameObject.transform.rotation.z);
    }
}
