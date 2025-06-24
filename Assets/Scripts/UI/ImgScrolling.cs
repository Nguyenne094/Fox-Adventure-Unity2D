using UnityEngine;
using UnityEngine.UI;

public class ImgScrolling : MonoBehaviour
{
    [SerializeField] private bool _applyX = true;
    [SerializeField] private bool _applyY = true;
    
    private RawImage img;

    [SerializeField] float speed = 2f;

    private void Awake() {
        img = GetComponent<RawImage>();
    }

    private void Update() {
        img.uvRect = new Rect(
            _applyX ? (img.uvRect.x + speed * Time.deltaTime) : img.uvRect.x,
            _applyY ? (img.uvRect.y + speed * Time.deltaTime) : img.uvRect.y,
            img.uvRect.width, 
            img.uvRect.height);
    }
}
