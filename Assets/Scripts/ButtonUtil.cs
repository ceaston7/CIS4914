using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUtil : MonoBehaviour
{
    public Canvas nextMenu;

    public void changeMenu(){
        transform.root.GetComponent<MenuManager>().changeMenu(nextMenu);
    }
}