using TMPro;
using UnityEngine;

public class TextEffectAnimator : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private float waveSpeed = 2f; 
    [SerializeField] private float waveFrequency = 0.5f; 
    [SerializeField] private float waveAmplitude = 5f; 

    [Header("Shake Settings")]
    [SerializeField] private float shakeSpeed = 10f; 
    [SerializeField] private float shakeFrequency = 0.5f; 
    [SerializeField] private float shakeAmplitude = 2f; 

    [Header("ScaleUp Settings")]
    [SerializeField] private float scaleUpSpeed = 3f; 
    [SerializeField] private float scaleUpAmplitude = 0.1f; 

    [Header("FadeIn Settings")]
    [SerializeField] private float fadeInDelay = 0.05f;

    public TextMeshProUGUI textMesh;
    public TextEffectType effectType;

    private float time;

    void OnEnable() => time = 0;

    void Update()
    {
        if (effectType == TextEffectType.None || string.IsNullOrEmpty(textMesh.text)) return;

        textMesh.ForceMeshUpdate();
        var textInfo = textMesh.textInfo;

        time += Time.deltaTime;
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int vertexIndex = charInfo.vertexIndex;
            int matIndex = charInfo.materialReferenceIndex;
            Vector3[] verts = textInfo.meshInfo[matIndex].vertices;

            Vector3 offset = Vector3.zero;

            switch (effectType)
            {
                case TextEffectType.Wave:
                    offset.y = Mathf.Sin(time * waveSpeed + i * waveFrequency) * waveAmplitude;
                    break;
                case TextEffectType.Shake:
                    offset = new Vector3(Mathf.Sin(time * shakeSpeed + i * shakeFrequency) * shakeAmplitude, Mathf.Cos(time * shakeSpeed + i * shakeFrequency) * shakeAmplitude, 0);
                    break;
                case TextEffectType.ScaleUp:
                    float scale = 1f + Mathf.Sin(time * scaleUpSpeed + i) * scaleUpAmplitude;
                    for (int j = 0; j < 4; j++)
                        verts[vertexIndex + j] = charInfo.bottomLeft + (verts[vertexIndex + j] - charInfo.bottomLeft) * scale;
                    continue;
                case TextEffectType.FadeIn:
                    Color32[] colors = textInfo.meshInfo[matIndex].colors32;
                    byte alpha = (byte)(Mathf.Clamp01(time - i * fadeInDelay) * 255);
                    for (int j = 0; j < 4; j++)
                        colors[vertexIndex + j].a = alpha;
                    continue;
            }

            for (int j = 0; j < 4; j++)
                verts[vertexIndex + j] += offset;
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textMesh.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}
