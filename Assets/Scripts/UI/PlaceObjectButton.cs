using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceObjectButton : MonoBehaviour, IPointerDownHandler
{
    public delegate void PlacedButtonClicked();
    public static event PlacedButtonClicked OnPlacedButtonClicked;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPlacedButtonClicked?.Invoke();
    }
}
