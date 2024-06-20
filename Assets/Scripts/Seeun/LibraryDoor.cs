using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryDoor : MonoBehaviour
{
    public AudioSource doorCloseAudio;
    public GameObject openUi;
    public GameObject cloeUi;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                openUi.SetActive(false);
                doorCloseAudio.Play();
                StartCoroutine(CloseUiStart());
                
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(OpenUiStart());
        }
    }

    IEnumerator OpenUiStart()
    {
        openUi.SetActive(true);
        yield return new WaitForSeconds(1);
        openUi.SetActive(false);
    }
    IEnumerator CloseUiStart()
    {
        cloeUi.SetActive(true);
        yield return new WaitForSeconds(3);
        cloeUi.SetActive(false);
    }

}
