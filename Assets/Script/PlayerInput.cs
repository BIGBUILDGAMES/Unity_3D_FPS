using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveAxisName = "Vertical";        // 앞뒤 움직임을 위한 입력축 이름
    public string rotateAsisName = "Horizontal";    // 좌우 회전을 위한 입력축 이름
    public string fireButtonName = "Fire1";         // 발사를 위한 입력 버튼 이름
    public string reloadButtonName = "Reload";      // 재장전을 위한 입력 버튼 이름
    public string WalkButtonName = "Walk";          // 걷기를 위한 입력 버튼 이름
    public string SitButtonName = "Sit";            // 앉기를 위한 입력 버튼 이름
    public string JumpButtonName = "Jump";          // 점프를 위한 입력 버튼 이름
    public string LockOnButtonName = "LockOn";      // 조준을 위한 입력 버튼 이름
    public string FireModeButtonName = "FireMode";  // 사격모드 전환을 위한 입력 버튼 이름

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
