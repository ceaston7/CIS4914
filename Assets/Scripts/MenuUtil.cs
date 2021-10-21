using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUtil : MonoBehaviour
{
    [SerializeField]
    List<GameObject> activate = new List<GameObject>();
    [SerializeField]
    List<GameObject> deactivate = new List<GameObject>();

    public void OpenMenu(){
        foreach (GameObject a in activate)
        {
            a.SetActive(true);
        }

        foreach (GameObject d in deactivate)
        {
            d.SetActive(false);
        }

        gameObject.SetActive(true);
    }

    public void CloseMenu(){
        gameObject.SetActive(false);
    }
}
