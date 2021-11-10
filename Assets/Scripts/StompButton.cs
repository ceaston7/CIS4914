using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompButton : MonoBehaviour
{
    public Rigidbody weight;
    public Transform weightTransform;
    float initialHeight;

    private void Start()
    {
        initialHeight = weightTransform.position.y;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("HIT: " + collision.gameObject.name);
        EstimateKinematic estimator = collision.gameObject.GetComponent<EstimateKinematic>();
        Debug.Log("weight height: " + (weightTransform.position.y <= initialHeight));
        if (estimator != null && weightTransform.position.y <= initialHeight){
            Debug.Log("Estimated force: " + estimator.force.y);
            weight.AddForce(Vector3.up * Mathf.Abs(estimator.force.y));
        }
        else{
            Rigidbody rigid = collision.gameObject.GetComponent<Rigidbody>();
            if(rigid != null){
                weight.AddForce(Vector3.up * Mathf.Abs(rigid.velocity.y));
            }
        }
    }
}
