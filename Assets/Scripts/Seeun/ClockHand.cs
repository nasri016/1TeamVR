using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class ClockHand : MonoBehaviour
{
    public Collider ClockHourHandCollider;
    public GameObject ui;

    public Collider Clock;

    private void OnTriggerStay(Collider Player)
    {
        if (ClockHourHandCollider.enabled)
        {
            ui.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                
                Destroy(gameObject);
                ui.SetActive(false);

                ClockHourHandCollider.enabled = false;
                Clock.enabled = true;
            }
        }
    }
}
