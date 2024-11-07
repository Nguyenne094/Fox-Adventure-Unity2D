using UnityEngine;
using UnityEngine.UI;

public class ImgScrolling : MonoBehaviour
{
    RawImage img;

    [SerializeField] float speed = 2f;

    private void Awake() {
        img = GetComponent<RawImage>();
    }

    private void Update() {
        img.uvRect = new Rect(img.uvRect.x + speed * Time.deltaTime, img.uvRect.y + speed * Time.deltaTime, img.uvRect.width, img.uvRect.height);
    }
}
