using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public Stack<Canvas> menuHistory;
    public Canvas currentMenu;
    // Start is called before the first frame update
    void Awake()
    {
        menuHistory = new Stack<Canvas>();
        currentMenu = GetComponent<Canvas>();
    }

    public void changeMenu(Canvas nextMenu){
        menuHistory.Push(currentMenu);
        currentMenu.enabled = false;
        currentMenu = nextMenu;
    }

    public void goBack()
    {
        currentMenu.enabled = false;
        currentMenu = menuHistory.Pop();
        currentMenu.enabled = true;
    }
}
