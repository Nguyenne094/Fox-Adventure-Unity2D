using System;
using UnityEngine;

public class DetectedZone : MonoBehaviour
{
    public LayerMask playerLayer;
    [HideInInspector] public Collider2D playerCollider;

    public bool wasDetected;
    public float detectedRadius;


    private void Update() {
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        playerCollider = Physics2D.OverlapCircle(transform.position, detectedRadius, playerLayer);

        if(playerCollider != null) 
            wasDetected = true;
        else 
            wasDetected = false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectedRadius);
    }
}
