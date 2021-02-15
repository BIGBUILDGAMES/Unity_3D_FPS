using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineRotate : MonoBehaviour
{
    public float speed = 120;
    public Transform playerTransform;

    float mouseX; // 마우스 X각도
    float mouseZ; // 마우스 X각도

    float rotateX; // 마우스 X각도
    float rotateY; // 마우스 X각도
    float rotateZ; // 마우스 X각도

    void LateUpdate()
    {
        float horizontal = Input.GetAxis("Mouse X"); // 마우스의 X축 움직임을 감지
        float vertical = Input.GetAxis("Mouse Y"); // 마우스의 Y축 움직임을 감지

        mouseX += horizontal * speed * Time.deltaTime; // 마우스 X각도를 누적해준다.
        mouseZ += vertical * speed * Time.deltaTime; // 마우스 Y각도를 누적해준다.

        //if (mouseZ >= 90)   // mouseY가 90도 이상이면
        //{
        //    mouseZ = 90;    // mouseY 90도 고정
        //}
        //else if (mouseZ <= -90)   // mouseY가 90도 이상이면
        //{
        //    mouseZ = -90;    // mouseY 90도 고정
        //}
        //transform.eulerAngles = new Vector3(
        //    gameObject.transform.rotation.x,
        //    mouseX + 137.8f,
        //    gameObject.transform.rotation.z - 90f);       

        //transform.eulerAngles = new Vector3(
        //    gameObject.transform.rotation.x,
        //    mouseX - 180,
        //    gameObject.transform.rotation.z);\

        //transform.eulerAngles = new Vector3(mouseZ - rotateX, gameObject.transform.rotation.y, mouseZ - rotateZ);

        //transform.rotation = transform.rotation * Quaternion.Euler(playerTransform.rotation.x, playerTransform.rotation.y, playerTransform.rotation.z); // 타겟으로 회전 

        //playerTransform.eulerAngles = new Vector3(
        //    playerTransform.transform.rotation.x,
        //    mouseX,
        //    playerTransform.transform.rotation.z);
        transform.Rotate(Vector3.right, mouseZ, Space.World);  //  세로
        transform.Rotate(Vector3.up, mouseX, Space.World);  // 가로

        
    }

    //public Transform target; // 바라볼 타겟 
    //public Transform target2; // 바라볼 타겟 
    //public Vector3 relativeVec; //물체에 대한 상대적인 백터 
    //private Animator anim; // 애니메이션 
    //private Transform spine; // 아바타의 상체 
    //bool aimMode = false;
    //// Use this for initialization 
    //void Start()
    //{
    //    anim = GetComponent<Animator>();
    //    spine = anim.GetBoneTransform(HumanBodyBones.Spine); // 상체값 가져오기 (허리 위) 
    //}

    //void LateUpdate()
    //{
    //    //if(Input.GetMouseButtonDown(1)) // 좌클릭 눌리는 동안 
    //    //{ 
    //    //    Debug.Log("바라보기"); 
    //    //    StartCoroutine(aimModeOn()); 
    //    //} 
    //    //else if(Input.GetMouseButtonUp(1)) // 좌클릭을 떄면 
    //    //{ 
    //    //    Debug.Log("해제"); 
    //    //    StartCoroutine(aimModeOff()); } 
    //    //if(aimMode == true) //에임모드가 활성화 되면 
    //    //{ 
    //    spine.LookAt(target.position); //플레이어의 상체부분이 타겟 위치 보기 
    //    spine.rotation = spine.rotation * Quaternion.Euler(relativeVec); // 타겟으로 회전 
    //                                                                         //} 
    //}
    //IEnumerator aimModeOn() // 에임모드가 켜지면 지정대기시간 이후 aimMode = true 
    //{ Debug.Log("조준"); 
    //    yield return new WaitForSeconds(0.07f); 
    //    aimMode = true; } 
    //IEnumerator aimModeOff() //에임모드가 꺼지면 지정대기시간이후 aimMode = false 
    //{ 
    //    yield return new WaitForSeconds(1.0f); 
    //    aimMode = false; 
    //}

}
