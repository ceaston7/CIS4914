using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandControls : MonoBehaviour
{
    public Transform grabPoint;
    GameObject colliding;
    GameObject holding;
    [SerializeField]
    SteamVR_Input_Sources controller;
    [SerializeField]
    SteamVR_Action_Boolean grabButton;
    bool isGrabButtonPressed = false;
    bool isHolding = false;

    // Start is called before the first frame update
    void Start()
    {
        grabButton.AddOnChangeListener(GrabButtonChange, controller);
    }

    void FixedUpdate()
    {
        if(isGrabButtonPressed && !isHolding && colliding != null)
        {
            Pickup pickup = colliding.GetComponent<Pickup>();
            if (pickup != null)
            {
                isHolding = pickup.Grab(grabPoint.gameObject);
                holding = colliding;
            }
        }

        if(isHolding && !isGrabButtonPressed){
            Pickup pickup = holding.GetComponent<Pickup>();
            if (pickup != null)
            {
                isHolding = pickup.Drop();
                holding = null;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        colliding = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        colliding = null;
    }

    void GrabButtonChange(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        isGrabButtonPressed = newState;
    }
}
