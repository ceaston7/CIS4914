using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalibrationButton : MonoBehaviour
{
    public Image targetImage;
    public List<GameObject> activate = new List<GameObject>();
    public List<GameObject> deactivate = new List<GameObject>();

    public void CalibrationTimer()
    {   
        foreach(GameObject d in deactivate){
            d.SetActive(false);
        }
        StartCoroutine("CalibrationProgress", 2.0f);
    }

    IEnumerator CalibrationProgress(float timer)
    {
        targetImage.fillAmount = 0.0f;
        targetImage.gameObject.SetActive(true);

        float time = 0.0f;
        while (time < timer)
        {
            yield return new WaitForSecondsRealtime(0.03f);
            time += 0.03f;
            targetImage.fillAmount = time / timer;
        }
        targetImage.fillAmount = 1.0f;
        yield return new WaitForSecondsRealtime(1.0f);
        targetImage.gameObject.SetActive(false);
        foreach (GameObject a in activate)
        {
            a.SetActive(true);
        }
    }
}
