using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUtil : MonoBehaviour
{
    public Canvas targetMenu;
    public Canvas superMenu;
    public void changeMenu()
    {
        superMenu.GetComponent<MenuManager>().ChangeMenu(targetMenu);
    }
}