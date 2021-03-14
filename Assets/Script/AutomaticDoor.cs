using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    private Animator doorAnimator;    // Door의 애니메이터

    void Start()
    {
        doorAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // 문이 열림
        doorAnimator.SetBool("Open the Door", true);
    }

    private void OnTriggerExit(Collider other)
    {
        // 문이 닫힘
        doorAnimator.SetBool("Open the Door", false);
    }
}
