using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public Canvas nextMenu;
    public MenuManager MenuManager;

    void changeMenu(){
        var parentCanvas = GetComponentInParent<Canvas>();
        MenuManager.menuHistory.Push(parentCanvas);
        parentCanvas.enabled = false;
        nextMenu.enabled = true;
    }
}