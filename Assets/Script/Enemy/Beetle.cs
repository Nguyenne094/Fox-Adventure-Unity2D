using System;
using System.Collections;
using UnityEngine;

public class Beetle : MonoBehaviour
{
    Damageable damageable;

    [Header("Setting")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float amplitude = 1f;
    
    private float horizontalMovement;
    private Vector2 initialPos;

    private void Awake() {
        damageable = GetComponent<Damageable>();
    }

    private void Start() {
        initialPos = transform.position;
    }

    private void FixedUpdate() {
        if(damageable.IsAlive) Movement();
    }

    private void Movement()
    {
        horizontalMovement = Mathf.Cos(Time.time * speed) * amplitude;

        transform.position = initialPos + new Vector2(horizontalMovement, 0);
    }

}
