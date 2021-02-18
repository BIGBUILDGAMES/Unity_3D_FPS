using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사용자 입력에 따라 플레이어 캐릭터를 움직이는 스크립트
public class PlayerMovement : MonoBehaviour
{
    public float speed = 4f;

    private PlayerInput playerInput;    // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playeRigidbody;  // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator;    // 플레이어 캐릭터의 애니메이터

    public Transform cameraArm;

    void Start()
    {
        // 사용할 컨포넌트들의 참조 가져오기
        playerInput = GetComponent<PlayerInput>();
        playeRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // 움직임 실행
        Move();

        // 입력값에 따라 애니메이터의 Move 파라미터값 변경
        playerAnimator.SetFloat("Vertical", playerInput.vertical);
        playerAnimator.SetFloat("Horizontal", playerInput.horizontal);

        // 앉기
        playerAnimator.SetBool("SitDown", playerInput.sitDown);
    }

    private void Move()
    {
        //float movez = playerInput.vertical;
        //float movex = playerInput.horizontal;
        //// 상대적으로 이동할 거리 계산
        //Vector3 distanceZ =
        //    movez * transform.forward * speed * Time.fixedDeltaTime;

        //Vector3 distanceX =
        //    movex * transform.right * speed * Time.fixedDeltaTime;

        //// 리지드바디를 이용해 게임 오브젝트 위치 변경
        //if ((movez > 0f || movez < 0f) && (movex > 0f || movex < 0f))
        //    playeRigidbody.MovePosition(playeRigidbody.position + (distanceZ + distanceX) / 1.4142135623f);
        //else
        //    playeRigidbody.MovePosition(playeRigidbody.position + distanceZ + distanceX);

        //// 보는 방향으로 회전
        //if (playerInput.move2)
        //{
        //    transform.rotation = cameraArm.transform.rotation;
        //}

        Vector2 moveInput = new Vector2(playerInput.horizontal, playerInput.vertical);

        bool isMove = moveInput.magnitude != 0f;

        if (isMove)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
            speed = 4;
            playerAnimator.SetBool("Run", false);

            if (Input.GetKey(KeyCode.W) && playerInput.run)
            {
                playerAnimator.SetBool("Run", true);
                speed = 18;
                moveDir = lookForward * moveInput.y;
            }          

            if ((moveInput.y > 0f || moveInput.y < 0f) && (moveInput.x > 0f || moveInput.x < 0f))
                playeRigidbody.MovePosition(playeRigidbody.position + moveDir * Time.deltaTime * speed / 1.4142135623f);
            else
                playeRigidbody.MovePosition(playeRigidbody.position + moveDir * Time.deltaTime * speed);

            //transform.position += moveDir * Time.deltaTime * 5f;
            //transform.forward = lookForward;
        }
        //else if (playerInput.vertical > 0 && playerInput.run) // 달리기
        //{
        //    Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
        //    Vector3 moveDir = lookForward * moveInput.y;

        //    playeRigidbody.MovePosition(playeRigidbody.position + moveDir * Time.deltaTime * 8f);
        //}
    }
}
