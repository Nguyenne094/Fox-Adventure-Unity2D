using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    [SerializeField] private List<Collider2D> hitCollider;
    private Collider2D checkGround;
    private ContactFilter2D contactFilter;
    private RaycastHit2D[] groundedHits = new RaycastHit2D[10];
    [SerializeField] private float groundDistance = 1f;

    [SerializeField] private bool _haveGround;
    public bool HaveGround { get => _haveGround; private set => _haveGround = value; }

    private void Awake() {
        hitCollider =  new List<Collider2D>();
        checkGround = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        hitCollider.Add(other);
    }

    private void OnTriggerExit2D(Collider2D other) {
        hitCollider.Remove(other);
    }

    private void FixedUpdate()
    {
        HaveGround = checkGround.Cast(Vector2.down, contactFilter, groundedHits, groundDistance) > 0;
    }
}
