using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUtil : MonoBehaviour
{
    public Canvas targetMenu;
    public void changeMenu()
    {
        transform.root.GetComponent<MenuManager>().ChangeMenu(targetMenu);
    }
}