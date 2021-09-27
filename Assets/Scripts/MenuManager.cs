using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public Stack<Canvas> menuHistory;
    // Start is called before the first frame update
    void Start()
    {
        menuHistory = new Stack<Canvas>();
    }
}
