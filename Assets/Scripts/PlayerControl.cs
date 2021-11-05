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
    public GroundChecker leftFootGroundChecker;
    public Transform rightFoot;
    public Calibration rightFootCalibrationData;
    public GroundChecker rightFootGroundChecker;

    [SerializeField]
    GameObject leftHand;
    [SerializeField]
    GameObject rightHand;

    bool walkButtonIsDown = false;
    bool isGrounded = true;
    bool isFootOnGround = true;
    public bool useGravity = false;
    [SerializeField]
    float raycastDist;
    List<bool> gravityBuffer;
    [SerializeField]
    int bufferSize;

    bool spawnButtonIsDown = false;
    bool spawnButtonChanged = false;
    SteamVR_Input_Sources spawnSource;

    Vector3 lastLeftFootPos = new Vector3();
    Vector3 lastRightFootPos = new Vector3();
    public SteamVR_Action_Boolean walkButton;
    public SteamVR_Action_Boolean menuButton;
    public SteamVR_Action_Boolean spawnButton;
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
        leftFootGroundChecker = rightFoot.GetComponent<GroundChecker>();
        rightFootCalibrationData = rightFoot.GetComponent<Calibration>();
        rightFootGroundChecker = rightFoot.GetComponent<GroundChecker>();
        gravityBuffer = new List<bool>();
        for (int i = 0; i < bufferSize; i++)
        {
            gravityBuffer.Add(true);
        }
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

            if(spawnButtonIsDown && spawnButtonChanged){
                Debug.Log("Spawn cube");
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                
                cube.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                if(spawnSource == SteamVR_Input_Sources.LeftHand){
                    cube.transform.position = leftHand.transform.position;
                }
                else if(spawnSource == SteamVR_Input_Sources.RightHand){
                    cube.transform.position = rightHand.transform.position;
                }
                
                cube.AddComponent<Rigidbody>();
                
                spawnButtonChanged = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (useGravity)
        {
            gravityBuffer.RemoveAt(gravityBuffer.Count - 1);
            if (!(rightFootGroundChecker.grounded || leftFootGroundChecker.grounded))
                Debug.LogWarning("right: " + rightFootGroundChecker.grounded + "\nleft: " + leftFootGroundChecker.grounded);

            gravityBuffer.Insert(0, !(rightFootGroundChecker.grounded || leftFootGroundChecker.grounded));
            int i = 0;
            foreach (bool b in gravityBuffer)
            {
                Debug.Log("buffer val " + i++ + ": " + b);
            }
            bool a = gravityBuffer.TrueForAll(x => { return x; });
            Debug.Log("gravity should be: " + a);
            rigid.useGravity = a;
            if (!rigid.useGravity)
            {
                rigid.velocity = Vector3.zero;
            }
        }

        rightFootGroundChecker.colliding = leftFootGroundChecker.colliding = false;
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

    void SpawnButtonChange(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState){
        Debug.Log("spawn button change");
        spawnButtonIsDown = newState;
        spawnButtonChanged = true;
        spawnSource = fromSource;
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
