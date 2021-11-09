using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Pickup : MonoBehaviour
{
    Transform originalParent;
    private void Start()
    {
        originalParent = transform.parent;
    }
    public bool Grab(GameObject grabber){
        transform.position = grabber.transform.position;
        transform.parent = grabber.transform;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<VelocityEstimator>().BeginEstimatingVelocity();
        return true;
    }

    public bool Drop(){
        transform.parent = originalParent;
        var rigid = GetComponent<Rigidbody>();
        var estimator = GetComponent<VelocityEstimator>();
        rigid.isKinematic = false;
        estimator.FinishEstimatingVelocity();
        rigid.velocity = estimator.GetVelocityEstimate();
        rigid.angularVelocity = estimator.GetAngularVelocityEstimate();

        return false;
    }
}
