using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenGlass : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var source = transform.root.GetComponent<AudioSource>();
        if (!source.isPlaying && collision.collider.transform.CompareTag("Player"))
        {
            Debug.Log("Player step");
            source.Play();
        }
    }
}
