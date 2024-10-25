using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.VisualScripting;


[RequireComponent(typeof(ARPlaneManager), typeof(ARRaycastManager))]
public class PlaceEnvironment : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    private GameObject instantiatedPrefab;


    private ARRaycastManager _arRaycastManager;
    private ARPlaneManager _arPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private bool OneTime = false;

    private void Awake()
    {
        _arPlaneManager = GetComponent<ARPlaneManager>();
        _arRaycastManager = GetComponent<ARRaycastManager>();

    }

    private void Start()
    {
        instantiatedPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        instantiatedPrefab.SetActive(false);
    }

    private void OnEnable()
    {
        TouchSimulation.Enable();
        EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable()
    {
        TouchSimulation.Enable();
        EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }
    public void SetOneTimeFalse()
    {
        if (!OneTime) return;
        
        OneTime = false;
        Destroy(instantiatedPrefab);
        instantiatedPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        instantiatedPrefab.SetActive(false);
    }


    // TO DEBUG POURPOSES COULD BE USEFUL TO ALLOW THE SPAWN ALSO WITH MOUSE CLICK

    private void FingerDown(Finger finger)
    {
        if (OneTime) return;
        if (finger.index != 0) return;

        if(_arRaycastManager.Raycast(finger.currentTouch.screenPosition, 
            hits, TrackableType.PlaneWithinPolygon)){

            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        foreach (ARRaycastHit hit in hits)
        {
            Pose pose = hit.pose;
            instantiatedPrefab.transform.position = pose.position;
            instantiatedPrefab.transform.rotation = pose.rotation;
            instantiatedPrefab.SetActive(true);
        }

        OneTime = true;
    }
}
