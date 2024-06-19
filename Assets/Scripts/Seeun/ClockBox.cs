using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockBox : MonoBehaviour
{
    //public Animator animator;
    public GameObject ui;
    public Animator animator;
    public Collider selfCollider;
    public Collider ClockHandCollider;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ui.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                ui.SetActive(false);
                animator.SetTrigger("isOpen");
                selfCollider.enabled = false;
                StartCoroutine(ClockHandDelay());
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

    IEnumerator ClockHandDelay()
    {
        yield return new WaitForSeconds(1);
        ClockHandCollider.enabled = true;
    }
}
