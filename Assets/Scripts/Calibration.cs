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
    [SerializeField] private Transform playerRoot;
    Quaternion rotation;

    void Awake(){
        heightMeasurements = new List<float>();

        if (hmd == null)
            hmd = GameObject.Find("[CameraRig]").transform;
    }

    private void Start()
    {
        baseHeight = playerRoot.InverseTransformPoint(transform.position).y;
    }

    public void Calibrate(){
        CalibrateHeight();
        CalibrateOrientation();
    }

    public void CalibrateHeight()
    {
        baseHeight = 0;
        if (!recordingHeight) { 
            heightMeasurements = new List<float>();
            recordingHeight = true;
            StartCoroutine("RecordHeight");
        }
    }

    public void CalibrateOrientation() {
        rotation = Quaternion.LookRotation(hmd.forward, Vector3.up);
    }

    public IEnumerator RecordHeight() {
        for (float timer = 0.0f; timer < calibrateTime; timer += 0.03f)
        {
            //Difference between tracker and floor height
            heightMeasurements.Add(playerRoot.InverseTransformPoint(transform.position).y);

            yield return new WaitForSecondsRealtime(0.03f);
        }

        float sum = 0;

        foreach(var measurement in heightMeasurements){
            sum += measurement;
        }

        baseHeight = sum / heightMeasurements.Count;
        Debug.Log(name + " height diff: " + Vector3.up * -baseHeight);
        transform.GetChild(0).position += Vector3.up *-baseHeight*0.05f;
        model.transform.rotation = rotation;
        recordingHeight = false;
    }
}
