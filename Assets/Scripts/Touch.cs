using UnityEngine;
using UnityEngine.UI;

public class Touch : MonoBehaviour
{
    public Text touchCount;

    public void Update()
    {
        touchCount.text = Input.touchCount.ToString();
    }
}