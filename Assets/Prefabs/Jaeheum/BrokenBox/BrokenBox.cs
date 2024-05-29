using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenBox : MonoBehaviour
{
    public Collider[] colliders;
    public float Mass = 1;
    public float Drag = 2; 

    private void Awake()
    {
        colliders  = gameObject.GetComponentsInChildren<Collider>();
        foreach(Collider item in colliders)
        {
           Rigidbody rb = item.GetComponent<Rigidbody>();
            if ( rb != null )
            {
                rb.constraints = RigidbodyConstraints.FreezeAll; // 전부 고정 상태
                rb.mass = Mass;
                rb.drag = Drag;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Weapon") {  // Weapon 태그로 지정된 오브젝트가 건드리면
            colliders = gameObject.GetComponentsInChildren<Collider>();
            foreach(Collider item in colliders)
            {
                Rigidbody rb = item.GetComponent<Rigidbody>();
                if( rb != null )
                {
                    rb.constraints = RigidbodyConstraints.None; // rigidbody.constraints 체크 해제 
                    Destroy(gameObject, 30); // 파괴된 오브젝트는 사라지도록 하겠음 
                }
            }

            Debug.Log("접촉됨");
        }
    }
}
