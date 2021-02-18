using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
[RequireComponent(typeof(ARRaycastManager))]

public class PlanePoseTracker : MonoBehaviour
{
    // Рейкастит из центра экрана позицию на плейне 
    // Двигает индикатор в эту позицию

    public static bool poseAviable { get; private set; }
    public static PlanePoseTracker instance;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [SerializeField] private Transform indicator;
    [SerializeField] private GameObject placeObjectButton;

    private GameObject indicatorGameObject;
    private ARRaycastManager raycastManager;
    private ARSessionOrigin origin;
    private PlacedObjectManager objectManager;

    private Pose placementPose;
   
    private Vector3 centerCoords = new Vector3(.5f, .5f);
    private void Awake()
    {
        indicatorGameObject = indicator.gameObject;
        objectManager = FindObjectOfType<PlacedObjectManager>();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        origin = GetComponent<ARSessionOrigin>();
        raycastManager = GetComponent<ARRaycastManager>();
    }

    private void Update()
    {
        UpdatePose();
        UpdateIdicator();
    }

    private void UpdateIdicator()
    {
        if(poseAviable && PlacedObjectManager.planeObjectPlaced == false)
        {
            indicatorGameObject.SetActive(true);
            indicator.position = placementPose.position;
            indicator.rotation = placementPose.rotation;
            SetButton(true);
        }
        else
        {
            indicatorGameObject.SetActive(false);
            SetButton(false);
        }
    }

    private void UpdatePose()
    {
        if (PlacedObjectManager.planeObjectPlaced) return;
        Ray fromCenterRay = origin.camera.ScreenPointToRay(origin.camera.ViewportToScreenPoint(centerCoords));
        raycastManager.Raycast(fromCenterRay, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
        if (hits.Count > 0)
        {
            poseAviable = true;
            placementPose = hits[0].pose;
        }
        else
        {
            poseAviable = false;
        }
    }

    public bool TryGetPose(out Pose pose)
    {
        pose = placementPose;
        return poseAviable;
    }

    private void OnEnable()
    {
        TrackTypeSwitcher.OnPlaneTrackingTypeChanged += SetEnable;
    }

    private void OnDestroy()
    {
        TrackTypeSwitcher.OnPlaneTrackingTypeChanged -= SetEnable;
    }

    private void SetEnable(bool enabled)
    {
        if (objectManager.IsPlaneObjectPlaced() == false)
        {
            this.enabled = enabled;
            indicatorGameObject.SetActive(enabled);
            SetButton(enabled);
        }
    }

    private void SetButton(bool enabled)
    {
        placeObjectButton.SetActive(enabled);
    }
}
