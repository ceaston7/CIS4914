using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using MyUserSettings;

public class PlayerControl : MonoBehaviour
{
    //For debugging
    bool firstPress = false;
    List<float> leftHeights = new List<float>();
    List<float> rightHeights = new List<float>();
    public int walkDebugFrames;
    int walkDebugCounter;

    //Debug setters
    public bool debugGravity;
    public bool debugWalk;
    public bool debugSlide;

    public Transform camera;
    Rigidbody rigid;
    Vector3 initialPosition;

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

    public float walkSpeed;
    public float slideSpeed;
    public float liftThreshold;

    bool walkButtonIsDown = false;
    bool isSlideButtonDown = false;
    public bool useGravity = false;
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
    public SteamVR_Action_Vector2 slideDirection;
    public SteamVR_Action_Boolean slideButton;
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
                slideButton.AddOnChangeListener(SlideButtonChange, controller);
                break;
            case Locomotion.walk:
                //walking button
                walkButton.AddOnChangeListener(WalkButtonChange, controller);
                break;
        }

        spawnButton.AddOnChangeListener(SpawnButtonChange, controller);
        menuButton.AddOnStateDownListener(OpenMenu, controller);

        if(leftFootCalibrationData == null)
            leftFootCalibrationData = leftFoot.GetComponent<Calibration>();
        if(leftFootGroundChecker == null)
            leftFootGroundChecker = leftFoot.GetComponent<GroundChecker>();
        if(rightFootCalibrationData == null)
            rightFootCalibrationData = rightFoot.GetComponent<Calibration>();
        if(rightFootGroundChecker == null)
            rightFootGroundChecker = rightFoot.GetComponent<GroundChecker>();

        gravityBuffer = new List<bool>();
        for (int i = 0; i < bufferSize; i++)
        {
            gravityBuffer.Add(true);
        }
        initialPosition = transform.position;
    }

    void Update()
    {
        if (!menuIsOpen)
        {
            if (walkButtonIsDown)
            {
                Walk();
            }

            if(spawnButtonIsDown && spawnButtonChanged){
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
            gravityBuffer.Insert(0, !(rightFootGroundChecker.grounded || leftFootGroundChecker.grounded));
            bool a = gravityBuffer.TrueForAll(x => { return x; });
            rigid.useGravity = a;
            if (!rigid.useGravity)
            {
                rigid.velocity = Vector3.zero;
            }
            if(debugGravity){
                Debug.Log("Use gravity: " + a);
                Debug.Log("Right FoG: " + rightFootGroundChecker.footOnGround +
                            "\nright colliding: " + rightFootGroundChecker.colliding +
                            "\nright sphere: " + rightFootGroundChecker.spherecastGround);
                Debug.Log("Left FoG: " + leftFootGroundChecker.footOnGround +
                            "\nleft colliding: " + leftFootGroundChecker.colliding +
                            "\nleft sphere: " + leftFootGroundChecker.spherecastGround);
            }
        }

        if(!rigid.useGravity){
            if(isSlideButtonDown){
                Slide();
            }
        }

        rightFootGroundChecker.colliding = leftFootGroundChecker.colliding = false;
    }

    //TODO: Walking backward?
    private void Walk()
    {
        Vector3 walkVector = new Vector3();
        //Walk in the average direction feet are pointing
        var walkDirection = leftFoot.forward + rightFoot.forward;
        walkDirection.y = 0f;
        walkDirection.Normalize();

        float leftDiff = Mathf.Abs(leftFoot.position.y - lastLeftFootPos.y);
        float rightDiff = Mathf.Abs(rightFoot.position.y - lastRightFootPos.y);

        rightHeights.Add(rightDiff);
        leftHeights.Add(leftDiff);

        //if (leftDiff + rightDiff > liftThreshold)
        walkVector = walkDirection * (leftDiff + rightDiff) * walkSpeed;
        transform.position += walkVector;

        //update last foot positions after walk
        lastLeftFootPos.Set(leftFoot.position.x, leftFoot.position.y, leftFoot.position.z);
        lastRightFootPos.Set(rightFoot.position.x, rightFoot.position.y, rightFoot.position.z);

        if (debugWalk && walkDebugCounter++ < walkDebugFrames)
        {
            Debug.Log(walkDebugCounter + " walkDirection: " + walkDirection.ToString("F4"));
            Debug.Log(walkDebugCounter + " walkVector: " + walkVector.ToString("F4"));
            Debug.Log(walkDebugCounter + " leftDiff: " + Mathf.Abs(leftFoot.position.y - lastLeftFootPos.y) + 
                        "\nlastLeft: " + lastLeftFootPos.y + 
                        "\ncurrent left: " + leftFoot.position.y);
            Debug.Log(walkDebugCounter + " rightDiff: " + Mathf.Abs(rightFoot.position.y - lastRightFootPos.y) + 
                        "\nlastRight: " + lastRightFootPos.y + 
                        "\ncurrent right: " + rightFoot.position.y);
        }
        firstPress = false;
    }

    private void Slide()
    {
        Quaternion relativeDirection = GetHMDRelativeDirection(slideDirection.GetAxis(controller));
        transform.position += relativeDirection * Vector3.forward * slideSpeed * Time.deltaTime;
    }

    private Quaternion GetHMDRelativeDirection(Vector2 direction){
        float directionAngle = Mathf.Atan2(direction.x, direction.y);
        directionAngle *= Mathf.Rad2Deg;

        Vector3 relativeDirection = new Vector3(0, camera.rotation.eulerAngles.y + directionAngle, 0);
        return Quaternion.Euler(relativeDirection);
    } 

    void WalkButtonChange(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState){
        walkDebugCounter = 0;
        firstPress = newState;
        lastLeftFootPos = leftFoot.position;
        lastRightFootPos = rightFoot.position;
        walkButtonIsDown = newState;
    }

    void SpawnButtonChange(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState){
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

    void SlideButtonChange(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newstate){
        isSlideButtonDown = newstate;
    }

    public void ChangeMovementOption(int option)
    {
        if (option != (int)MyUserSettings.MyUserSettings.LocomotionMode)
        {
            switch ((Locomotion)option)
            {
                case Locomotion.walk:
                    Debug.Log("Setting walk");
                    walkButton.AddOnChangeListener(WalkButtonChange, controller);
                    slideButton.RemoveOnChangeListener(SlideButtonChange, controller);
                    break;
                case Locomotion.slide:
                    Debug.Log("Setting slide");
                    slideButton.AddOnChangeListener(SlideButtonChange, controller);
                    walkButton.RemoveOnChangeListener(WalkButtonChange, controller);
                    break;
            }

            MyUserSettings.MyUserSettings.LocomotionMode = (Locomotion)option;
        }
    }

    public void ResetPosition(){
        transform.position = initialPosition;
    }

    void OnDestroy(){
        if (debugWalk)
        {
            float rightAvg = 0f;
            float leftAvg = 0f;
            float rightMode = 0f;
            int rightModeCount = 0;
            float leftMode = 0f;
            int leftModeCount = 0;

            List<Vector2> leftCounter = new List<Vector2>();
            List<Vector2> rightCounter = new List<Vector2>();

            for (int i = 0; i < rightHeights.Count; i++)
            {
                rightHeights[i] = Mathf.Round(rightHeights[i] * 100f) / 100f;
                leftHeights[i] = Mathf.Round(leftHeights[i] * 100f) / 100f;
            }

            int index = -1;

            foreach (var r in rightHeights)
            {
                rightAvg += r;
                index = rightCounter.FindIndex(x => { return x.x == r; });
                if (index != -1)
                {
                    var a = rightCounter[index];
                    a.y++;
                    rightCounter[index] = a;
                }
                index = -1;
            }

            foreach (var l in leftHeights)
            {
                leftAvg += l;
                index = leftCounter.FindIndex(x => { return x.x == l; });
                if (index != -1)
                {
                    var a = leftCounter[index];
                    a.y++;
                    leftCounter[index] = a;
                }
                index = -1;
            }

            foreach (var v in rightCounter)
            {
                if (rightModeCount < v.y)
                {
                    rightMode = v.x;
                    rightModeCount = (int)v.y;
                }
            }

            foreach (var v in leftCounter)
            {
                if (leftModeCount < v.y)
                {
                    leftMode = v.x;
                    leftModeCount = (int)v.y;
                }
            }

            rightAvg = rightAvg / rightHeights.Count;
            leftAvg = leftAvg / leftHeights.Count;

            Debug.Log("Leftavgdiff: " + leftAvg + "\nleft mode: " + leftMode + ", with: " + leftModeCount);
            Debug.Log("Rightavgdiff: " + rightAvg + "\nright mode: " + rightMode + ", with: " + rightModeCount);
        }
    }
}
