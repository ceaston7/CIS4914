using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibration : MonoBehaviour
{
    public float baseHeight = 0;
    private List<float> heightMeasurements;
    private float calibrateTime = 2.0f;
    public bool recordingHeight = false;
    [SerializeField] private Transform hmd;

    void Awake(){
        heightMeasurements = new List<float>();
        hmd = GameObject.Find("[Camera]").transform;
    }

    public void CalibrateHeight()
    {
        if (!recordingHeight) { 
            heightMeasurements = new List<float>();
            recordingHeight = true;
            RecordHeight();
        }
    }

    public void CalibrateOrientation(){
        Quaternion.LookRotation(hmd.forward, Vector3.up);
    }

    public IEnumerator RecordHeight() {
        for (float timer = 0.0f; timer < calibrateTime; timer += Time.deltaTime)
        { 
            Debug.Log("recording height");
            //Difference between tracker and floor height
            heightMeasurements.Add(transform.position.y - transform.root.transform.position.y);

            yield return new WaitForFixedUpdate();
        }

        float sum = 0;

        foreach(var measurement in heightMeasurements){
            sum += measurement;
        }

        baseHeight = sum / heightMeasurements.Count;
        recordingHeight = false;
    }
}
