using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public AudioSource audioSource;
    public Light Light;
    public AudioSource offaudioSource;
    public GameObject ui;
    public Collider radioCollider;
    public Collider paperCollider;

    //private bool isInside = false; //플레이어가 콜라이더 안에 있는지의 여부를 추적

    void Start()
    {
        // 게임 시작 시 오디오를 재생
        audioSource.Play();
        //paperCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ui.SetActive(true);    
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ui.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (radioCollider.enabled)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                audioSource.Stop();
                offaudioSource.Play();
                Light.enabled = !Light.enabled;
                
                ui.SetActive(false);
                radioCollider.enabled = false;
                //StartCoroutine(RadioColliderDelay(2));
                StartCoroutine(PaperColliderDelay(2));
            }
        }
    }

    IEnumerator PaperColliderDelay(float delay)
    {
        yield return new WaitForSeconds(delay);  // 지정된 시간(초)만큼 대기
        paperCollider.enabled = true;            // 지연 후 콜라이더 활성화
    }
}
