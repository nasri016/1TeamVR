using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public GameObject ui;
    public GameObject clockHand;
    public Collider selfCollider;
    public AudioSource clockSound;
    public Animator animator;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ui.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                ui.SetActive(false);
                clockHand.SetActive(true);
                clockSound.Play();
                animator.SetTrigger("isOpen");
                selfCollider.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ui.SetActive(false);
        }
    }
}
