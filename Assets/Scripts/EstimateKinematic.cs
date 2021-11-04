using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstimateKinematic : MonoBehaviour
{
    public Vector3 linVelocity;
    public Vector3 linAcceleration;
    public Vector3 rotVelocity;
    public Vector3 rotAcceleration;
    public float mass;

    public Vector3 force { get { return mass * linAcceleration; } }

    Vector3 lastPosition;
    Quaternion lastRotation;
    Vector3 lastLinVelocity;
    Vector3 lastLinAcceleration;
    Vector3 lastRotVelocity;
    Vector3 lastRotAcceleration;
    [SerializeField]
    Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        linVelocity = Vector3.zero;
        linAcceleration = Vector3.zero;
        rotVelocity = Vector3.zero;
        rotAcceleration = Vector3.zero;
        lastLinVelocity = Vector3.zero;
        lastLinAcceleration = Vector3.zero;
        lastRotVelocity = Vector3.zero;
        lastRotAcceleration = Vector3.zero;
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        lastLinVelocity = linVelocity;
        linVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastLinAcceleration = linAcceleration;
        linAcceleration = (linVelocity - lastLinVelocity) / Time.deltaTime;

        lastRotVelocity = rotVelocity;
        rotVelocity = Quaternion.RotateTowards(lastRotation, transform.rotation, 360.0f).eulerAngles / Time.deltaTime;
        lastRotAcceleration = rotAcceleration;
        rotAcceleration = (rotVelocity - lastRotVelocity)/Time.deltaTime;
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            ContactPoint contact;
            if (collision.rigidbody != null)
            {
                for (int i = 0; i < collision.contactCount; i++)
                {
                    contact = collision.GetContact(i);

                    //Actual force applied is the force in the direction of the collision normal (dot product) 
                    //divided by the number of contact points with the object
                    var linForce = (force * -Vector3.Dot(linAcceleration.normalized, contact.normal))/collision.contactCount;
                    Debug.Log("LINFORCE: " + linForce);
                    collision.rigidbody.AddForceAtPosition(linForce, contact.point);
                }
            }
        }
    }
}
