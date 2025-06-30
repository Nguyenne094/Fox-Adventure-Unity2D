using UnityEngine;
using System.Collections;

namespace UIEffect{
    public class ScaleUpIn : MonoBehaviour
    {
        [SerializeField] private float duration = 0.5f;

        private Vector3 originalScale;

        void OnEnable()
        {
            originalScale = transform.localScale;
            transform.localScale = Vector3.zero;
            TriggerScale();
        }

        private void TriggerScale()
        {
            StopAllCoroutines();
            StartCoroutine(ScaleCoroutine());
        }

        private IEnumerator ScaleCoroutine()
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}