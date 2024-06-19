using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Paper : MonoBehaviour
{
    public GameObject ui;
    public GameObject paper;
    public Collider paperCollider;

    public Collider box3;
    public Collider box4;
    public Collider box5;
    public Collider box6;
    public Collider box7;
    
    public Collider box9;
    

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ui.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
            if (other.CompareTag("Player"))
            {
                ui.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Destroy(paper);
                    paperCollider.enabled = false;
                    ui.SetActive(false);
                    StartCoroutine(BoxColliderDelay(2));
                }
            }
    }

    IEnumerator BoxColliderDelay(float delay)
    {
        yield return new WaitForSeconds(delay);  // 지정된 시간(초)만큼 대기
        
        box3.enabled = true;
        box4.enabled = true;
        box5.enabled = true;
        box6.enabled = true;
        box7.enabled = true;
        
        box9.enabled = true;
        
    }
}
