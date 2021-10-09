using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibration : MonoBehaviour
{
    public float baseHeight;
    private List<float> measurements;
    private float calibrateTime = 5.0f;

    void Awake(){
        measurements = new List<float>();
    }

    public void CalibrateHeight(){
        measurements = new List<float>();
        RecordHeight();
    }

    public IEnumerator RecordHeight() {
        for (float timer = 0.0f; timer < calibrateTime; timer += Time.deltaTime)
        { 
            Debug.Log("recording height");
            measurements.Add(transform.position.y);

            yield return new WaitForFixedUpdate();
        }
    }
}
