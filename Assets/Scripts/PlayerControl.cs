using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using MyUserSettings;

public class PlayerControl : MonoBehaviour
{
    public Transform camera;
    Rigidbody rigid;
    
    bool menuIsOpen = false;
    [SerializeField]
    Canvas pauseMenu;

    public Transform leftFoot;
    public Transform rightFoot;
    bool walkButtonIsDown = false;
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
                Debug.Log("setting action");
                //walking button
                walkButton.AddOnChangeListener(WalkButtonChange, controller);
                break;
        }

        menuButton.AddOnStateDownListener(OpenMenu, controller);
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
        }
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

    void WalkButtonChange(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState){
        walkButtonIsDown = newState;
        Debug.Log("walkButton: " + walkButtonIsDown);
    }

    void OpenMenu(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        menuIsOpen = true;
        Time.timeScale = 0;
        pauseMenu.transform.position = camera.transform.position + camera.transform.forward * 3.0f;
        pauseMenu.transform.forward = camera.transform.forward;
        pauseMenu.GetComponent<MenuManager>().OpenMenu();
    }
}
