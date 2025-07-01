using UnityEngine;
using System.Collections;

namespace UIEffect{
    public class ScaleDownIn : MonoBehaviour
    {
        [SerializeField] private float duration = 0.2f;

        private Vector3 originalScale;

        void Start()
        {   
            originalScale = transform.localScale;
        }

        void OnEnable()
        {
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
                transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}