using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibration : MonoBehaviour
{
    public float baseHeight = 0;
    public GameObject model;
    private List<float> heightMeasurements;
    private float calibrateTime = 2.0f;
    public bool recordingHeight = false;
    [SerializeField] private Transform hmd;

    void Awake(){
        heightMeasurements = new List<float>();

        if (hmd == null)
            hmd = GameObject.Find("[CameraRig]").transform;
    }

    public void Calibrate(){
        CalibrateHeight();
        CalibrateOrientation();
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
        model.transform.rotation = Quaternion.LookRotation(hmd.forward, Vector3.up);
    }

    public IEnumerator RecordHeight() {
        for (float timer = 0.0f; timer < calibrateTime; timer += 0.03f)
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
