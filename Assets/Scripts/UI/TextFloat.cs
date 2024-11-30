using TMPro;
using UnityEngine;
using UnityEditor;

public class TextFloat : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 80f;
    private Vector3 moveUp = Vector3.up;

    [SerializeField] private float fadeTime = 0.75f;
    private float timeElapsed = 0f;

    private RectTransform textTransform;
    private TextMeshProUGUI textMeshPro;
    private Color startColor;

    private void Awake()
    {
        startColor = GetComponent<TextMeshProUGUI>().color;
        textTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        textTransform.position += moveUp * floatSpeed * Time.deltaTime;

        timeElapsed += Time.deltaTime;

        if (timeElapsed < fadeTime)
        {
            float fadeAlpha = startColor.a * (1 - (timeElapsed / fadeTime));

            textMeshPro.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);
        }
        else
            Destroy(gameObject);
    }
}
