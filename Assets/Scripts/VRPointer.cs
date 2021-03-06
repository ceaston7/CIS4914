using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VRPointer : MonoBehaviour
{
    public EventSystem eventSystem;
    public StandaloneInputModule inputModule;
    public LineRenderer line;
    private List<RaycastResult> results;
    private RaycastResult canvasRaycast;

    public float defaultLength = 5.0f;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        results = new List<RaycastResult>();
    }

    void Update()
    {
        UpdateLength();
    }

    private void UpdateLength(){
        line.SetPositions(new Vector3[]{ transform.position, FindEnd()});
    }

    private Vector3 FindEnd(){
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.position = inputModule.inputOverride.mousePosition;
        
        results.Clear();
        eventSystem.RaycastAll(eventData, results);
        canvasRaycast = FindCanvasRaycast();
        float distance = Mathf.Clamp(canvasRaycast.distance, 0.0f, defaultLength);

        if(distance != 0.0f){
            return CalculateEnd(distance);
        }
        else{
            return CalculateEnd(defaultLength);
        }
    }

    private RaycastResult FindCanvasRaycast()
    {
        foreach(RaycastResult result in results){
            if(!result.gameObject)
                continue;

            return result;
        }

        return new RaycastResult();
    }

    private Vector3 CalculateEnd(float distance){
        return transform.position + (transform.forward * distance);
    }
}
