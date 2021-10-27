using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tripwire : MonoBehaviour
{
    public GameObject end;
    public CapsuleCollider collider;
    public GameObject cylinder;

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Player")){
            GetComponent<AudioSource>().Play();
        }
    }

    void stretch(){
        
    }

    void Start(){
        stretch();
    }
}
