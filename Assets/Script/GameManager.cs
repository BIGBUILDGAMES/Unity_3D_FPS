using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글턴 접근용 프로퍼티
    public static GameManager instance
    {
        get
        {
            // 만약 싱글턴 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아서 할당
                m_instance = FindObjectOfType<GameManager>();
                // 싱글턴 오브젝트 반환
            }
            return m_instance;
        }
    }

    private static GameManager m_instance; // 싱글턴이 할당될 static 변수

    public bool isGameover { get; private set; }    // 게임오버 상태

    private void Awake()
    {
        if (instance != this)   // 싱글턴 변수 instance가 비어 있는가?
        {
            // instance가 비어 있다면(null) 그곳에 자기 자신을 파괴
            Destroy(gameObject);
        }
    }

    //private void Start()
    //{
    //    // 플레이어 캐릭터의 사망 이벤트 발생 시 게임오버
    //    FindObjectOfType<PlayerHealth>().onDeath += EndGame;
    //}

    // 게임오버 처리
    public void EndGame()
    {
        // 게임오버 상태를 참으로 변경
        isGameover = true;
    }
}
