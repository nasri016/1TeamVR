using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    //1. 게임 시작 시 라디오 오디오 소리 출력 , ui 출력
    //2. 플레이어가 e키로 라디오를 끄면 소리랑 라디오 애니메이션이 꺼짐. 그리고 라디오가 꺼지는 애니메이션이 재생.

    public AudioSource audioSource; // Unity 인스펙터에서 설정할 수 있도록 public으로 선언
    public Animator animator;       // Unity 인스펙터에서 설정할 수 있도록 public으로 선언
    public GameObject uiElement;    // 라디오 UI 요소

    void Start()
    {
        // 게임 시작 시 오디오를 재생하고 UI를 활성화
        audioSource.Play();
        uiElement.SetActive(true);
    }

    public void TurnOffRadio()
    {
        // 오디오 정지 및 애니메이션 실행
        audioSource.Stop();
        animator.SetTrigger("turnOff");
    }
}
