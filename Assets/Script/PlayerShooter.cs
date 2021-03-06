﻿using UnityEngine;

// 주어진 Gun 오브젝트를 쏘거나 재장전
// 알맞은 애니메이션을 재생하고 IK를 사용해 캐릭터 양손이 총에 위치하도록 조정
public class PlayerShooter : MonoBehaviour {
    public Gun gun; // 사용할 총
    public Transform gunPivot; // 총 배치의 기준점
    public Transform leftHandMount; // 총의 왼쪽 손잡이, 왼손이 위치할 지점
    public Transform rightHandMount; // 총의 오른쪽 손잡이, 오른손이 위치할 지점

    private PlayerInput playerInput; // 플레이어의 입력
    private PlayerMovement playerMovement; // 플레이어의 입력
    private Animator playerAnimator; // 애니메이터 컴포넌트

    private AudioSource gunAudioPlayer; // 총 소리 재생기
    public AudioClip reloadClip; // 재장전 소리
    public AudioClip reloadVoiceClip; // 재장전 소리

    private void Start() 
    {
        // 사용할 컴포넌트들을 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimator = GetComponent<Animator>();
        gunAudioPlayer = GetComponent<AudioSource>();
    }

    private void OnEnable() {
        // 슈터가 활성화될 때 총도 함께 활성화
        gun.gameObject.SetActive(true);
    }
    
    private void OnDisable() {
        // 슈터가 비활성화될 때 총도 함께 비활성화
        gun.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W) && playerInput.run && !playerMovement.isJumping)    // 달리고 있을경우
        {
            playerAnimator.SetBool("Fire", false);
        }
        else
        {
            if (gun.magAmmo != 0)
            {
                // 탄이 있을때만 발사 애니메이션 재생
                playerAnimator.SetBool("Fire", playerInput.fire);

                // 입력을 감지하고 총 발사
                if (playerInput.fire && !playerInput.reload)
                {
                    // 발사 입력 감지 시 총 발사
                    gun.Fire();
                }
            }
            else if (gun.magAmmo == 0)
            {
                playerAnimator.SetBool("Fire", false);
            }

            if (!playerInput.fire && playerInput.reload)
            {
                // 재장전 입력 감지 시 재장전
                if (gun.Reload())
                {
                    Debug.Log("reload");

                    // 재장전 성공 시에만 재장전 애니메이션 재생
                    playerAnimator.SetTrigger("Reload");
                    gunAudioPlayer.PlayOneShot(reloadVoiceClip);
                }
            }
        }  

        // 남은 탄알 UI 갱신
        UpdateUI();
    }

    // 탄약 UI 갱신
    private void UpdateUI() {
        if (gun != null && UIManager.instance != null)
        {
            // UI 매니저의 탄약 텍스트에 탄창의 탄약과 남은 전체 탄약을 표시
            UIManager.instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
        }
    }

    private void ReloadingSFX()
    {
        gunAudioPlayer.PlayOneShot(reloadClip);
    }

    // 애니메이터의 IK 갱신
    //private void OnAnimatorIK(int layerIndex) 
    //{
    //    // 총의 기준점 gunPivot을 3D 모델의 오른쪽으로 팔꿈치 위치로 이동
    //    gunPivot.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

    //    // IK를 사용하여 왼손의 위치와 회전을 총의 왼쪽 손잡이에 맞춤
    //    playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
    //    playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

    //    playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
    //    playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);

    //    // IK를 사용하여 오른손의 위치와 회전을 총의 오른쪽 손잡이에 맞춤
    //    playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
    //    playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

    //    playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, leftHandMount.position);
    //    playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, leftHandMount.rotation);
    //}
}