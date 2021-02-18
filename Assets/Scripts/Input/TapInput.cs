using UnityEngine;
using UnityEngine.EventSystems;

public class TapInput : MonoBehaviour
{
    public delegate void ScreenTap();
    public static event ScreenTap OnScreenTap;

    void Update()
    {
        bool pointerDown = Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began;
        bool underUI;
#if UNITY_EDITOR
        pointerDown = Input.GetMouseButtonDown(0);
        underUI = EventSystem.current.IsPointerOverGameObject();
#else
        underUI = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        if(pointerDown && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false)
        {
            OnScreenTap?.Invoke();
        }
#endif


    }
}
