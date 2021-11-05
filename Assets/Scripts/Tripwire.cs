using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tripwire : MonoBehaviour
{
    [SerializeField]
    AudioSource source;
    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Player")){
            source.Play();
        }
    }
}
