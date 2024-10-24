using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    private ARTrackedImageManager trackedImages;
    public GameObject[] _arPrefabs;

    Dictionary<string, GameObject> ARObjects = new Dictionary<string, GameObject>();


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
    }

    void OnEnable()
    {
        trackedImages.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImages.trackedImagesChanged -= OnTrackedImagesChanged;
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
            ARObjects[trackedImage.referenceImage.name].SetActive(false);
        }

    }

    private void EnableImage(ARTrackedImage trackedImage)
    {
        ARObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
        ARObjects[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;
        ARObjects[trackedImage.referenceImage.name].SetActive(true);
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        ARObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
        ARObjects[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;

        /*
        foreach(GameObject arObj in ARObjects.Values)
        {
            if(arObj.name != trackedImage.referenceImage.name)
                arObj.SetActive(false);
        }
        */
    }
}
