using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibration : MonoBehaviour
{
    public float baseHeight = 0;
    private List<float> measurements;
    private float calibrateTime = 2.0f;
    public bool recordingHeight = false;

    void Awake(){
        measurements = new List<float>();
    }

    public void CalibrateHeight()
    {
        if (!recordingHeight) { 
            measurements = new List<float>();
            recordingHeight = true;
            RecordHeight();
        }
    }

    public IEnumerator RecordHeight() {
        for (float timer = 0.0f; timer < calibrateTime; timer += Time.deltaTime)
        { 
            Debug.Log("recording height");
            //Difference between tracker and floor height
            measurements.Add(transform.position.y - transform.root.transform.position.y);

            yield return new WaitForFixedUpdate();
        }

        float sum = 0;

        foreach(var measurement in measurements){
            sum += measurement;
        }

        baseHeight = sum / measurements.Count;
        recordingHeight = false;
    }
}
