using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DetectionZone : MonoBehaviour
{
    [SerializeField] private ContactFilter2D contactFilter;
    private Collider2D collider;
    private RaycastHit2D[] groundedHits = new RaycastHit2D[1];
    [SerializeField] private float groundDistance = 1f;

    [SerializeField] private bool _haveGround;
    public bool HaveGround { get => _haveGround; private set => _haveGround = value; }

    private void Awake() { 
        collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        HaveGround = collider.Cast(Vector2.down, contactFilter, groundedHits, groundDistance) > 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundDistance));
    }
}
