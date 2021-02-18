using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public class SwitchButton : MonoBehaviour, IPointerDownHandler
{
    public delegate void ButtonClicked();
    [SerializeField] private GameObject switchButton;
    
    public static event ButtonClicked OnButtonClicked;

    public void OnPointerDown(PointerEventData eventData)
    {
        switchButton.transform.DOLocalMoveX(-switchButton.transform.localPosition.x, .2f);
        OnButtonClicked?.Invoke();
    }
}
