using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using MyUserSettings;

public class PlayerControl : MonoBehaviour
{
    public Transform camera;
    Rigidbody rigid;
    
    public bool menuIsOpen = false;
    [SerializeField]
    Canvas pauseMenu;

    public Transform leftFoot;
    public Calibration leftFootCalibrationData;
    public Transform rightFoot;
    public Calibration rightFootCalibrationData;

    bool walkButtonIsDown = false;
    bool isGrounded = true;
    bool isFootOnGround = true;
    public bool useGravity = false;
    float spherecastRadius = 0.05f;
    Vector3 spherecastRadiusVector;
    float raycastDist = 0.02f;

    Vector3 lastLeftFootPos = new Vector3();
    Vector3 lastRightFootPos = new Vector3();
    public SteamVR_Action_Boolean walkButton;
    public SteamVR_Action_Boolean menuButton;
    public SteamVR_Input_Sources controller;

    void Start()
    {
        MyUserSettings.MyUserSettings.LocomotionMode = Locomotion.walk;
        rigid = GetComponent<Rigidbody>();
        switch (MyUserSettings.MyUserSettings.LocomotionMode)
        {
            case Locomotion.blink:
                break;
            case Locomotion.slide:
                break;
            case Locomotion.walk:
                //walking button
                walkButton.AddOnChangeListener(WalkButtonChange, controller);
                break;
        }

        menuButton.AddOnStateDownListener(OpenMenu, controller);
        leftFootCalibrationData = leftFoot.GetComponent<Calibration>();
        rightFootCalibrationData = rightFoot.GetComponent<Calibration>();
        spherecastRadius = leftFoot.GetChild(0).localScale.x / 2.0f;
        spherecastRadiusVector = new Vector3(0, 0, spherecastRadius);
    }

    void Update()
    {
        if (!menuIsOpen)
        {
            if (walkButtonIsDown)
            {
                walk();
                //update last foot positions after walk
                lastLeftFootPos.Set(leftFoot.position.x, leftFoot.position.y, leftFoot.position.z);
                lastRightFootPos.Set(rightFoot.position.x, rightFoot.position.y, rightFoot.position.z);
            }

            isGrounded = GroundedCheck();
            isFootOnGround = FootOnGroundCheck();

            if (useGravity)
            {
                isGrounded = GroundedCheck();
                isFootOnGround = FootOnGroundCheck();
                if (isFootOnGround)
                {
                    if (!isGrounded)
                    {
                        rigid.useGravity = true;
                    }
                    else
                    {
                        rigid.useGravity = false;
                    }
                }
                else
                {
                    rigid.useGravity = false;
                }
            }
        }
    }

    private void FixedUpdate()
    {

    }

    //TODO: Walking backward?
    private void walk()
    {
        var camera2dForward = new Vector3(camera.forward.x, 0.0f, camera.forward.z);
        camera2dForward.Normalize();
        //TODO: See if a minimum movement is required to keep from slowly walking while standing still
        transform.position += camera2dForward * (Mathf.Abs(leftFoot.position.y - lastLeftFootPos.y) + Mathf.Abs(rightFoot.position.y - lastRightFootPos.y));
    }

    void WalkButtonChange(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState){
        walkButtonIsDown = newState;
    }

    void OpenMenu(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        menuIsOpen = true;
        Time.timeScale = 0;
        pauseMenu.transform.position = camera.transform.position + camera.transform.forward * 3.0f;
        pauseMenu.transform.forward = camera.transform.forward;
        pauseMenu.GetComponent<MenuManager>().OpenMenu();
    }

    //Check to see if both feet are physically off the ground, meaning the user is jumping in real-space
    //Return true if at least one foot is on the ground
    bool FootOnGroundCheck(){
        bool right = rightFootCalibrationData.baseHeight - rightFoot.transform.position.y > 0.1;
        bool left = leftFootCalibrationData.baseHeight - leftFoot.transform.position.y > 0.1;
        Debug.Log("right ground: " + right);
        Debug.Log("left ground: " + left);
        if (rightFootCalibrationData.baseHeight - rightFoot.transform.position.y > 0.1
            && leftFootCalibrationData.baseHeight - leftFoot.transform.position.y > 0.1)
            return false;
        else
            return true;
    }

    //Check if the user's feet are on a surface in VR space
    //Return true if at least one foot is on the ground
    bool GroundedCheck(){
        RaycastHit rightRaycastHit, leftRaycastHit;

        Ray rightRay = new Ray(rightFoot.transform.position, Vector3.down);
        Ray leftRay = new Ray(rightFoot.transform.position, Vector3.down);

        bool rightHit = Physics.Raycast(rightRay, out rightRaycastHit, 0.01f);
        bool leftHit = Physics.Raycast(leftRay, out leftRaycastHit, 0.01f);

        bool rightSphereHit1 = Physics.SphereCast(rightFoot.transform.position + rightFoot.transform.TransformPoint(spherecastRadiusVector), spherecastRadius, Vector3.down, out rightRaycastHit, raycastDist);
        Debug.Log("right1: " + rightSphereHit1);
        if(rightSphereHit1){
            Debug.Log("Hit " + rightRaycastHit.transform.gameObject.name);
        }
        bool rightSphereHit2 = Physics.SphereCast(rightFoot.transform.position - rightFoot.transform.TransformPoint(spherecastRadiusVector), spherecastRadius, Vector3.down, out rightRaycastHit, raycastDist);
        Debug.Log("right2: " + rightSphereHit2);
        if (rightSphereHit2)
        {
            Debug.Log("Hit " + rightRaycastHit.transform.gameObject.name);
        }
        bool leftSphereHit1 = Physics.SphereCast(leftFoot.transform.position + leftFoot.transform.TransformPoint(spherecastRadiusVector), spherecastRadius, Vector3.down, out leftRaycastHit, raycastDist);
        Debug.Log("left1: " + leftSphereHit1);
        if (leftSphereHit1)
        {
            Debug.Log("Hit " + leftRaycastHit.transform.gameObject.name);
        }
        bool leftSphereHit2 = Physics.SphereCast(leftFoot.transform.position - leftFoot.transform.TransformPoint(spherecastRadiusVector), spherecastRadius, Vector3.down, out leftRaycastHit, raycastDist);
        Debug.Log("left2: " + leftSphereHit2);
        if (leftSphereHit2)
        {
            Debug.Log("Hit " + leftRaycastHit.transform.gameObject.name);
        }

        return rightSphereHit1 || rightSphereHit2 || leftSphereHit1 || leftSphereHit2;
    }
}
