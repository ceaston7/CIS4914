using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tripwire : MonoBehaviour
{
    [SerializeField]
    AudioSource source;
    public void OnTriggerEnter(Collider collider){
        if(collider.gameObject.CompareTag("Player")){
            source.Play();
        }
    }
}
