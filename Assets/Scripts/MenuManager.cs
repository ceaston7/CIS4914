﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public Stack<Canvas> menuHistory;
    public Canvas currentMenu;
    public Canvas startMenu;
    
    void Awake()
    {
        menuHistory = new Stack<Canvas>();
    }

    public void ChangeMenu(Canvas nextMenu){
        menuHistory.Push(currentMenu);
        currentMenu.GetComponent<MenuUtil>().CloseMenu();
        currentMenu = nextMenu;
        currentMenu.GetComponent<MenuUtil>().OpenMenu();
    }

    public void GoBack()
    {
        currentMenu.GetComponent<MenuUtil>().CloseMenu();
        currentMenu = menuHistory.Pop();
        currentMenu.GetComponent<MenuUtil>().OpenMenu();
    }

    public void CloseMenu(){
        Debug.Log("Closing menu");
        menuHistory.Clear();
        gameObject.SetActive(false);
        currentMenu.GetComponent<MenuUtil>().CloseMenu();
        Time.timeScale = 1.0f;
    }

    public void OpenMenu(){
        gameObject.GetComponent<MenuUtil>().OpenMenu();
        currentMenu = startMenu;
        menuHistory.Push(startMenu);
    }
}
