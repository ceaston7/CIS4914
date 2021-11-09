using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateButton : MonoBehaviour
{
    [SerializeField]
    List<GameObject> deactivate;
    bool alreadyCollided = false;

    private void OnTriggerEnter(Collider collider)
    {
        Debug.LogError("Already Collided: " + alreadyCollided);
        if(!alreadyCollided && collider.CompareTag("Player")){
            foreach(GameObject d in deactivate){
                d.SetActive(false);
            }
            alreadyCollided = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
            alreadyCollided = false;
    }
}
