using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class VRMenuInput : BaseInput
{
    public Camera eventCamera;
    public SteamVR_Action_Boolean inputButton;
    public SteamVR_Input_Sources controller;

    protected override void Awake()
    {
        GetComponent<BaseInputModule>().inputOverride = this;
    }

    public override bool GetMouseButton(int button){
        return inputButton.GetState(controller);
    }

    public override bool GetMouseButtonUp(int button){
        return inputButton.GetStateUp(controller);
    }

    public override bool GetMouseButtonDown(int button)
    {
        return inputButton.GetStateDown(controller);
    }

    public override Vector2 mousePosition
    {
        get
        {
            return new Vector2(eventCamera.pixelWidth / 2, eventCamera.pixelHeight / 2);
        }
    }
}
