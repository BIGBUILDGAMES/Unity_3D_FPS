using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveZAxisName = "Vertical";       // 앞뒤 움직임을 위한 입력축 이름
    public string moveXAxisName = "Horizontal";     // 좌우 회전을 위한 입력축 이름
    public string fireButtonName = "Fire1";         // 발사를 위한 입력 버튼 이름
    public string reloadButtonName = "Reload";      // 재장전을 위한 입력 버튼 이름
    public string WalkButtonName = "Walk";          // 걷기를 위한 입력 버튼 이름
    public string SitButtonName = "Sit";            // 앉기를 위한 입력 버튼 이름
    public string JumpButtonName = "Jump";          // 점프를 위한 입력 버튼 이름
    public string LockOnButtonName = "LockOn";      // 조준을 위한 입력 버튼 이름
    public string FireModeButtonName = "FireMode";  // 사격모드 전환을 위한 입력 버튼 이름

    public float vertical { get; private set; }    // 감지된 움직임 입력값
    public float horizontal { get; private set; }  // 감지된 회전 입력값
    public bool fire { get; private set; }      // 감지된 발사 입력값
    public bool reload { get; private set; }    // 감지된 재장전 입력값

    void Update()
    {
        // 게임오버 상태에서는 사용자 입력을 감지하지 않음
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            vertical = 0;
            horizontal = 0;
            fire = false;
            reload = false;
            return;
        }

        // move에 관한 입력 감지
        vertical = Input.GetAxis(moveZAxisName);
        // rotate에 관한 입력 감지
        horizontal = Input.GetAxis(moveXAxisName);
        // fire에 관한 입력 감지
        fire = Input.GetButton(fireButtonName);
        // reload에 관한 입력 감지
        reload = Input.GetButtonDown(reloadButtonName);
    }
}
