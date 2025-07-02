using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace UIEffect
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class FadeOnLayerOut : MonoBehaviour
    {
        [SerializeField] private LayerMask layer;
        [SerializeField, Min(0.1f)] private float fadeDuration = 1f;
        [SerializeField] private Ease fadeEase = Ease.Linear;

        private Collider2D col;
        private SpriteRenderer spriteRenderer;

        void Awake()
        {
            col = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if ((layer & (1 << other.gameObject.layer)) != 0)
            {
                StopAllCoroutines();
                spriteRenderer.DOFade(0f, fadeDuration).SetEase(fadeEase);
            }
        }
    }
}