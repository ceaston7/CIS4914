using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool grounded = false;

    bool colliding = false;
    bool footOnGround = false;
    bool spherecastGround = false;

    float spherecastRadius;
    Vector3 spherecastRadiusVector;
    Calibration calibrationData;
    Vector3 origin;
    Vector3 direction;
    bool sphereHit1;
    bool sphereHit2;
    [SerializeField]
    float raycastDist;

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

    private void OnCollisionStay(Collision collision)
    {
        colliding = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        colliding = false;
    }

    //Check to see if foot is physically on the ground
    bool FootOnGroundCheck()
    {
        bool a = Mathf.Abs(calibrationData.baseHeight - transform.root.InverseTransformPoint(transform.position).y) < 0.1f;
        //Debug.Log(gameObject.name + " on ground: " + a);
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
