using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateButton : MonoBehaviour
{
    [SerializeField]
    List<GameObject> deactivate;
    bool alreadyCollided = false;

    public void OnCollisionEnter(Collision collision)
    {
        if(!alreadyCollided && collision.collider.CompareTag("Player")){
            foreach(GameObject d in deactivate){
                d.SetActive(false);
            }
            alreadyCollided = true;
        }
    }

    public void OnCollisionExit(Collision collision){
        if (collision.collider.CompareTag("Player"))
            alreadyCollided = false;
    }
}
