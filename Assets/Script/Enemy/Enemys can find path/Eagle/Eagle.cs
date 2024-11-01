using System;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    DetectedZone detectedZone;
    Damageable damageable;
    CircleCollider2D col;

    [SerializeField] private bool isFacingLeft = true;
    public bool IsFacingLeft
    {
        get { return isFacingLeft; }
        set
        {
            if (isFacingLeft != value)
            {
                transform.localScale *= new Vector2(-1, 1); // Flip along X axis
            }
            isFacingLeft = value;
        }
    }

    [Header("Setting")]
    [SerializeField] private float _speed = 0.1f;

    private void Awake()
    {
        detectedZone = GetComponentInChildren<DetectedZone>();
        damageable = GetComponent<Damageable>();
        col = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if (detectedZone && detectedZone.wasDetected)
        {
            IsFacingLeft = FacingDirection();

            if(damageable.IsAlive) Movement();
        }
        else return;
    }

    private void Movement()
    {
        transform.position = Vector3.Lerp(transform.position, detectedZone.playerCollider.gameObject.transform.position, _speed);
    }

    private bool FacingDirection()
    {
        return transform.position.magnitude > detectedZone.playerCollider.gameObject.transform.position.magnitude;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (detectedZone && detectedZone.wasDetected)
        {
            Gizmos.DrawLine(transform.position, detectedZone.playerCollider.gameObject.transform.position);
        } 
        else return;
    }
}
