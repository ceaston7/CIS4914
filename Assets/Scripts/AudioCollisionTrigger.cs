using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCollisionTrigger : MonoBehaviour
{
    public bool soundOverlap;
    public bool soundCutoff;
    private void OnCollisionEnter(Collision collision)
    {
        var source = transform.root.GetComponent<AudioSource>();
        if (collision.collider.transform.CompareTag("Player")) {
            if (!soundOverlap && !soundCutoff && !source.isPlaying)
            {
                source.Play();
            }
            else if(soundCutoff)
            {
                source.Stop();
                source.Play();
            }
            else if(soundOverlap){
                source.Play();
            }
        }
    }
}
