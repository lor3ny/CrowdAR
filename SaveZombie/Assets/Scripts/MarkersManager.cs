using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MarkersManager : MonoBehaviour
{
    private ARTrackedImageManager trackedImages;
    public GameObject[] _arPrefabs;
    public GameObject _arEnvironmentPrefab;

    private Dictionary<string, GameObject> ARObjects = new Dictionary<string, GameObject>();
    private GameObject AREnv;
    private bool envSpawned = false;
    void Awake()
    {
        trackedImages = GetComponent<ARTrackedImageManager>();
        foreach (GameObject prefab in _arPrefabs)
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            newPrefab.SetActive(false);
            ARObjects.Add(prefab.name, newPrefab);
        }

        AREnv = Instantiate(_arEnvironmentPrefab, Vector3.zero, Quaternion.identity);
        AREnv.name = _arEnvironmentPrefab.name;
        GameObject fixedBridge = GameObject.Find("FixedBridge");
        AREnv.SetActive(false);

        ARObjects["BridgeRock"].GetComponent<InteractableObject>().SetFixedBridge(fixedBridge);
        fixedBridge.SetActive(false);
    }

    void OnEnable()
    {
        trackedImages.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImages.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    public void ResetEnvSpawned()
    {
        envSpawned = false;
        //AREnv.SetActive(false);
    }

    public void FixEnvSpawned()
    {
        if(AREnv.activeSelf) 
            envSpawned = true;
    }

    public bool envIsActive()
    {
        return AREnv.activeSelf;
    }


    // Event Handler
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        //Create object based on image tracked
        foreach (var trackedImage in eventArgs.added)
        {
            EnableImage(trackedImage);
        }

        //Update tracking position
        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }

        //Removed tracking position
        foreach (var trackedImage in eventArgs.removed)
        {
            if (trackedImage.referenceImage.name == AREnv.name)
            {
                AREnv.SetActive(false);
            }
            else
            {
                ARObjects[trackedImage.referenceImage.name].SetActive(false);
            }
        }

    }

    private void EnableImage(ARTrackedImage trackedImage)
    {

        if(trackedImage.referenceImage.name == AREnv.name && !envSpawned)
        {
            AREnv.transform.position = trackedImage.transform.position;
            AREnv.transform.rotation = trackedImage.transform.rotation;
            AREnv.SetActive(true);
        } else if(trackedImage.referenceImage.name != AREnv.name)
        {
            if (!ARObjects.ContainsKey(trackedImage.referenceImage.name))
            {
                return;
            }

            ARObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
            ARObjects[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;
            ARObjects[trackedImage.referenceImage.name].SetActive(true);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {

        if (trackedImage.referenceImage.name == AREnv.name && !envSpawned)
        {
            AREnv.transform.position = trackedImage.transform.position;
            AREnv.transform.rotation = trackedImage.transform.rotation;
            AREnv.SetActive(true);
        }
        else if (trackedImage.referenceImage.name != AREnv.name)
        {
            if (!ARObjects.ContainsKey(trackedImage.referenceImage.name))
            {
                return;
            }

            ARObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
            ARObjects[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;
            ARObjects[trackedImage.referenceImage.name].SetActive(true);
        }
        /*
        foreach(GameObject arObj in ARObjects.Values)
        {
            if(arObj.name != trackedImage.referenceImage.name)
                arObj.SetActive(false);
        }
        */
    }
}
