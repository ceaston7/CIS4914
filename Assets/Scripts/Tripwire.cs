using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tripwire : MonoBehaviour
{
    public CapsuleCollider collider;

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Player")){
            GetComponent<AudioSource>().Play();
        }
    }
}
