using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * 플레이어가 e키를 누르면 테이프가 들어가는 애니메이션이 재생.
 * 테이프 애니메이션 재생 후 power_indicator 트리거 활성화, e키 누를 수 있음 (전원 킴)
 * 전원 켜지면 영상이 재생됨 (cctv monitor)
 */

//의사코드
//e키 누름 -> 테이프 뚜껑 들어가면서 테이프들어가는 애니메이션 재생 (애니메이션을 하나로 할건지 동시에 재생시킬건지)
//e키 눌러서 전원 킴 (라이트 들어옴)
//테이프 애니메이션이 재생되었고 전원이 들어왔다면 cctv에서 영상이 나옴 (총 30초)

public class VHSPlayer : MonoBehaviour
{
    //플레이어가 레이캐스트를 쏨. 테이프 획득 여부와 상관없이 플레이어가 근처에 오면
    //테이프를 삽입해주세요 라는 ui가 뜸

    //private Animation animation;
    public Animator animator;

    private void Start()
    {
        //animation = GetComponent<Animation>();
        animator = GetComponent<Animator>();    
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            animator.SetTrigger("TapeTake");
        }
        Debug.Log("되고 있는겨?");
    }



}
