using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonCompress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Image img;
    [SerializeField] private Sprite _default, _pressed;

    private void Awake() {
        img = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        img.sprite = _pressed;    
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        img.sprite = _default;
    }
}
