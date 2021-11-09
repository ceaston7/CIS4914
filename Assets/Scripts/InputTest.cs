using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class InputTest : MonoBehaviour
{
    public SteamVR_Action_Boolean grip;
    public SteamVR_Action_Vector2 touch;
    public SteamVR_Action_Boolean interact;
    // Update is called once per frame
    void Update()
    {
        if (grip.GetStateDown(SteamVR_Input_Sources.Any))
            print("GRIP");

        Vector2 asdf = touch.GetAxis(SteamVR_Input_Sources.Any);
        if (asdf.magnitude != 0.0f)
            print("touch: " + asdf);

        if (interact.GetStateDown(SteamVR_Input_Sources.LeftHand))
            print("INTERACT");
    }
}
