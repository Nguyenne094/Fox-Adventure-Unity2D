using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UIEffect
{
    [RequireComponent(typeof(Collider2D))]
    public class FadeOnLayerOut : MonoBehaviour
    {
        [SerializeField] private LayerMask layer;
        [SerializeField, Min(0.1f)] private float fadeDuration = 1f;
        [SerializeField] private Ease fadeEase = Ease.Linear;

        private Collider2D col;
        private SpriteRenderer spriteRenderer;
        private Tilemap tilemap;

        void Awake()
        {
            col = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            tilemap = GetComponent<Tilemap>();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if ((layer & (1 << other.gameObject.layer)) != 0)
            {
                StopAllCoroutines();
                spriteRenderer.DOFade(0f, fadeDuration).SetEase(fadeEase);
                if(tilemap == null) return;
                DOVirtual.Float(0, 1, fadeDuration, value =>
                {
                    tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, value);
                }).SetEase(fadeEase);
            }
        }
    }
}