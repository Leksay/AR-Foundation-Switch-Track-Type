using UnityEngine;
using UnityEngine.XR.ARFoundation;
public class TrackTypeSwitcher : MonoBehaviour
{
    public delegate void PlaneTrackingTypeChanged(bool enabled);
    public static event PlaneTrackingTypeChanged OnPlaneTrackingTypeChanged;

    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private ARTrackedImageManager imageManager;

    private void Start()
    {
        if(planeManager == null)
            planeManager = FindObjectOfType<ARPlaneManager>();

        if (imageManager == null)
            imageManager = FindObjectOfType<ARTrackedImageManager>();

        OnPlaneTrackingTypeChanged?.Invoke(planeManager.enabled);
    }

    private void OnEnable()
    {
        SwitchButton.OnButtonClicked+= SwitchDetectionType;
    }

    private void OnDisable()
    {
        SwitchButton.OnButtonClicked -= SwitchDetectionType;
    }

    public void SwitchDetectionType()
    {
        bool planeTypeEnabled = planeManager.enabled;
        SetPlaneTracking(!planeTypeEnabled);
        SetImageTracking(planeTypeEnabled);
    }

    private void SetImageTracking(bool imageTypeEnabled)
    {
        foreach(var image in imageManager.trackables)
        {
            image.gameObject.SetActive(imageTypeEnabled);
        }
        imageManager.enabled = imageTypeEnabled;
    }

    private void SetPlaneTracking(bool planeTypeEnabled)
    {
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(planeTypeEnabled);
        }
        planeManager.enabled = planeTypeEnabled;
        OnPlaneTrackingTypeChanged?.Invoke(planeTypeEnabled);
    }
}
