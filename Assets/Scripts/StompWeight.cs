using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompWeight : MonoBehaviour
{
    public Transform ejectionPoint;

    private void FixedUpdate()
    {
        if (transform.position.y >= ejectionPoint.position.y)
            GetComponent<Rigidbody>().useGravity = false;
    }
}
