using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CasetteInsert : MonoBehaviour
{

    public PlayableDirector playableDirector;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(playableDirector != null)
            {
                playableDirector.Play();
            }
        }
    }
}
