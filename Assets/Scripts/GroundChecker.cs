using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    //Publicly accesible state of groundedness of VR user
    public bool grounded = false;

    //Grounded state based off of collision
    public bool colliding = false;

    //Grounded state based off of meatspace position
    public bool footOnGround = false;

    //Grounded state based off of spherecast from VR feet
    public bool spherecastGround = false;

    float spherecastRadius;
    Vector3 spherecastRadiusVector;
    Calibration calibrationData;
    Vector3 origin;
    Vector3 direction;
    bool sphereHit1;
    bool sphereHit2;
    [SerializeField]
    float raycastDist;
    [SerializeField]
    LayerMask mask;

    private void Start()
    {
        spherecastRadius = transform.GetChild(0).localScale.x / 2.1f;
        spherecastRadiusVector = new Vector3(0, 0, spherecastRadius);
        calibrationData = GetComponent<Calibration>();
    }

    private void Update()
    {
        //Debug.Log(gameObject.name + " colliding: " + colliding);
        FootOnGroundCheck();
        SpherecastCheck();
        //Debug.Log(gameObject.name + " spherecast: " + spherecastGround);
        grounded = !(footOnGround && !spherecastGround && !colliding);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger enter");
        colliding = !other.gameObject.CompareTag("NoStand");
        Debug.Log("colliding: " + colliding);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("NoStand"))
            colliding = false;
    }*/

    private void OnCollisionStay(Collision collision)
    {
        colliding = !collision.gameObject.CompareTag("Player");
    }

    //Check to see if foot is physically on the ground in meatspace
    bool FootOnGroundCheck()
    {
        bool a = Mathf.Abs(calibrationData.baseHeight - transform.root.InverseTransformPoint(transform.position).y) < 0.1f;
        footOnGround = a;
        return a;
    }

    bool SpherecastCheck()
    {
        RaycastHit hit;

        origin = transform.TransformPoint(spherecastRadiusVector);
        direction = Vector3.down;
        sphereHit1 = Physics.SphereCast(origin, spherecastRadius, Vector3.down, out hit, raycastDist);
        Debug.DrawRay(origin, direction, Color.red);

        origin = transform.TransformPoint(-spherecastRadiusVector);
        sphereHit2 = Physics.SphereCast(origin, spherecastRadius, Vector3.down, out hit, raycastDist);
        Debug.DrawRay(origin, direction, Color.blue);

        spherecastGround = sphereHit1 || sphereHit2;
        return sphereHit1 || sphereHit2;
    }
}
