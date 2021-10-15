using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tripwire : MonoBehaviour
{
    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Player")){
            //alarm
        }
    }
}
