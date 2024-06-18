using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class ClockHourHand : MonoBehaviour
{
    public Collider ClockHourHandCollider;
    public GameObject ui;

    private void OnTriggerStay(Collider Player)
    {
        ui.SetActive(true);
        if (Input.GetKeyDown(KeyCode.E))
        {
            gameObject.SetActive(false);
        }
    }

}
