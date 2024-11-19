using System;
using UnityEngine;

public class Beetle : MonoBehaviour
{
    EnemyHealth enemyHealth;

    [Header("Setting")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float amplitude = 1f;
    
    private float horizontalMovement;
    private Vector2 initialPos;

    private void Awake() {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Start() {
        initialPos = transform.position;
        if(enemyHealth.IsAlive) Movement();
    }

    private void FixedUpdate() {
    }

    private void Movement()
    {
        horizontalMovement = Mathf.Cos(Time.time * speed) * amplitude;

        transform.position = initialPos + new Vector2(horizontalMovement, 0);
    }

}
