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

    //The transform of the playspace object
    public Transform playerRoot;

    //Debugging stuff
    public bool debugging;
    List<float> footDists;
    int[] spherecastResults;
    int[] collisionResults;

    float spherecastRadius;
    Vector3 spherecastRadiusVector;
    Calibration calibrationData;
    Vector3 origin;
    Vector3 direction;
    bool sphereHit1;
    bool sphereHit2;
    [SerializeField]
    float raycastDist;

    //How close foot height can be to base height and still be considered on ground
    public float footOnGroundMargin;

    private void Start()
    {
        spherecastRadius = transform.GetChild(0).localScale.x / 2.1f;
        spherecastRadiusVector = new Vector3(0, 0, spherecastRadius);
        calibrationData = GetComponent<Calibration>();
        if(footOnGroundMargin == 0){
            footOnGroundMargin = 0.05f;
        }
        footDists = new List<float>();
        spherecastResults = new int[2] { 0, 0 };
        collisionResults = new int[2] { 0, 0 };
    }

    private void FixedUpdate()
    {
        FootOnGroundCheck();
        SpherecastCheck();
        if (colliding)
            collisionResults[0]++;
        else
            collisionResults[1]++;

        grounded = !(footOnGround && !spherecastGround && !colliding);
    }

    private void OnCollisionStay(Collision collision)
    {
        colliding = !collision.gameObject.CompareTag("Player");
    }

    //Check to see if foot is physically on the ground in meatspace
    bool FootOnGroundCheck()
    {
        float footDist = Mathf.Abs(calibrationData.baseHeight - playerRoot.InverseTransformPoint(transform.position).y);
        footDists.Add(footDist);
        bool a = footDist < footOnGroundMargin;
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

        if (spherecastGround)
            spherecastResults[0]++;
         else
            spherecastResults[1]++;

        return sphereHit1 || sphereHit2;
    }

    private void OnDestroy()
    {
        if (debugging)
        {
            float sum = 0;
            foreach (float a in footDists)
            {
                sum += a;
            }

            float avgDist = sum / footDists.Count;

            Debug.Log(name + "\navgFoG dist: " + avgDist + 
                        "\nSpherecasts true: " + spherecastResults[0] + ", false: " + spherecastResults[1] +
                        "\nCollisions true: " + collisionResults[0] + ", false: " + collisionResults[1]);
        }
    }

    private float HighestValue(List<float> values){
        float highest = 0;
        foreach(var value in values){
            if (value > highest)
                highest = value;
        }
        return highest;
    }
}
