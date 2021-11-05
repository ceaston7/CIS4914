using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCollisionTrigger : MonoBehaviour
{
    public bool soundOverlap;
    public bool soundCutoff;
    public AudioSource source;

    private void Awake()
    {
        if(source == null)
        {
            source = transform.GetComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Debug.Log("player step");
            if (!soundOverlap && !soundCutoff && !source.isPlaying)
            {
                source.Play();
            }
            else if (soundCutoff)
            {
                source.Stop();
                source.Play();
            }
            else if (soundOverlap)
            {
                source.Play();
            }
        }
    }
}
