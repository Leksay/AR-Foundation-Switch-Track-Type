using UnityEngine;
using UnityEngine.XR.ARFoundation;
public class PlacedObjectManager : MonoBehaviour
{
    // Ставит объекты на плейн
    // Устанавливает анимацию объектов
    public static bool planeObjectPlaced { get; private set; }

    [SerializeField] private GameObject objectPrefab;

    private ARTrackedImageManager imageManager;
    private PlanePoseTracker planePoseTracker;
    private GameObject planeObject;
    private GameObject imageObject;
    private bool currentModeIsPlane;

    private void Awake()
    {
        imageManager = FindObjectOfType<ARTrackedImageManager>();
        if(planePoseTracker == null)
            planePoseTracker = FindObjectOfType<PlanePoseTracker>();

        imageObject = GameObject.Instantiate(objectPrefab, Vector3.zero, Quaternion.identity);
        imageObject.SetActive(false);
        imageManager.trackedImagesChanged += HandleImageTrackable;
    }

    private void Update()
    {
        if (imageManager.trackables.count < 0)
            imageObject.SetActive(false);
    }

    // Проверяет что видит Image Tracker. Включает/выключает и перемещает создавнный объект
    private void HandleImageTrackable(ARTrackedImagesChangedEventArgs args)
    {
        if (currentModeIsPlane)
        {
            imageObject.SetActive(false);
            return;
        }
        if(imageManager.trackables.count > 0)
        {
            if (args.added.Count > 0) imageObject.SetActive(true);
            if (args.removed.Count > 0) imageObject.SetActive(false);
            foreach (var trackedImage in args.updated)
            {
                UpdateImageObject(trackedImage);
            }
        }
        else
        {
            imageObject.SetActive(false);
        }
    }

    private void UpdateImageObject(ARTrackedImage image)
    {
        imageObject.transform.position = image.transform.position;
        imageObject.transform.up = image.transform.up;
    }

    public void PlacePlaneObject()
    {
        if(planeObject == null && planePoseTracker.TryGetPose(out Pose pose))
        {
            planeObject = GameObject.Instantiate(objectPrefab, pose.position, pose.rotation);
            planeObject.transform.localScale *= 2.5f;
            planeObjectPlaced = true;
        }
    }

    public bool IsPlaneObjectPlaced() => planeObject != null;

    private void OnEnable()
    {
        PlaceObjectButton.OnPlacedButtonClicked += PlacePlaneObject;
        TrackTypeSwitcher.OnPlaneTrackingTypeChanged += SetTrackMode;
        TapInput.OnScreenTap += AnimateCurrentObject;
    }

    private void OnDisable()
    {
        PlaceObjectButton.OnPlacedButtonClicked -= PlacePlaneObject;
        TapInput.OnScreenTap -= AnimateCurrentObject;
    }

    private void OnDestroy()
    {
        TrackTypeSwitcher.OnPlaneTrackingTypeChanged -= SetTrackMode;
    }
    private void SetTrackMode(bool isPlaneMode)
   {
        currentModeIsPlane = isPlaneMode;
        if (isPlaneMode)
        {
            planeObject?.SetActive(true);
            imageObject.SetActive(false);
        }
        else
        {
            planeObject?.SetActive(false);
        }
   }

   private void AnimateCurrentObject()
   {
        print("Play animation");
        if(currentModeIsPlane)
        {
            planeObject.GetComponent<Animator>().SetTrigger("animate");
            imageObject.SetActive(false);
        }
        else
        {
            imageObject.GetComponent<Animator>().SetTrigger("animate");
        }
   }
}
