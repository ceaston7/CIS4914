using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using MyUserSettings;

public class PlayerControl : MonoBehaviour
{
    public Transform leftFoot;
    public Transform rightFoot;
    public Transform camera;
    Rigidbody rigidbody;
    bool walkButtonIsDown;
    Vector3 lastLeftFootPos = new Vector3();
    Vector3 lastRightFootPos = new Vector3();
    public SteamVR_Action_Boolean walkButton;
    public SteamVR_Input_Sources controller;

    void Start()
    {
        MyUserSettings.MyUserSettings.LocomotionMode = Locomotion.walk;
        walkButtonIsDown = false;
        rigidbody = GetComponent<Rigidbody>();
        switch (MyUserSettings.MyUserSettings.LocomotionMode)
        {
            case Locomotion.blink:
                break;
            case Locomotion.slide:
                break;
            case Locomotion.walk:
                Debug.Log("setting action");
                //walking button
                walkButton.AddOnChangeListener(walkButtonChange, controller);
                break;
        }
    }

    void Update()
    {
        if (walkButtonIsDown){
            walk();
        }
        //update last foot positions after walk
        lastLeftFootPos.Set(leftFoot.position.x, leftFoot.position.y, leftFoot.position.z);
        lastRightFootPos.Set(rightFoot.position.x, rightFoot.position.y, rightFoot.position.z);
    }

    private void FixedUpdate()
    {

    }

    //TODO: Walking backward?
    private void walk()
    {
        Debug.Log("In walk");
        var camera2dForward = new Vector3(camera.forward.x, 0.0f, camera.forward.z);
        camera2dForward.Normalize();
        Debug.Log("camera forward: " + camera2dForward.ToString("F3"));
        //TODO: See if a minimum movement is required to keep from slowly walking while standing still
        Debug.Log((camera2dForward * (Mathf.Abs(leftFoot.position.y - lastLeftFootPos.y) + Mathf.Abs(rightFoot.position.y - lastRightFootPos.y))).ToString("F3"));
        transform.position += camera2dForward * (Mathf.Abs(leftFoot.position.y - lastLeftFootPos.y) + Mathf.Abs(rightFoot.position.y - lastRightFootPos.y));
    }

    void walkButtonChange(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState){
        walkButtonIsDown = newState;
        Debug.Log("walkButton: " + walkButtonIsDown);
    }
}
