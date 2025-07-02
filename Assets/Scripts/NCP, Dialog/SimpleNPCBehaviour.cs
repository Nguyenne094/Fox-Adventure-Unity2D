using UnityEngine;

public class SimpleNPCBehaviour : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;

    private bool isPlayerInRange;
    private GameObject player;

    void Update()
    {
        if(!isPlayerInRange || player == null)
            return;

        transform.localScale = new Vector3(
            Mathf.Abs(transform.localScale.x) * Mathf.Sign(player.transform.position.x - transform.position.x),
            transform.localScale.y,
            transform.localScale.z);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((playerLayer & (1 << collision.gameObject.layer)) != 0)
        {
            isPlayerInRange = true;
            player = collision.gameObject;
        }
    }
    
    void OnTriggerExit2D(Collider2D collision)
    {
        if ((playerLayer & (1 << collision.gameObject.layer)) != 0)
        {
            isPlayerInRange = false;
            player = null;
        }
    }
}