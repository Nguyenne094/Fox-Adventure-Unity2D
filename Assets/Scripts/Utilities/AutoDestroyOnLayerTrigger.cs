using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AutoDestroyOnLayerTrigger : MonoBehaviour
{
    public LayerMask triggerLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & triggerLayer) != 0)
        {
            Destroy(gameObject);
        }
    }
}
