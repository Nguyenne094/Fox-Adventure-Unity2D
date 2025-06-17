using System;
using Enemy.Flying_Enemy;
using UnityEditor;
using UnityEngine;

public class Beetle : MonoBehaviour
{
    BeetleHealth enemyHealth;

    [Header("Setting")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float amplitude = 1f;

    [Header("Debug")] public bool debug;
    
    private float horizontalMovement;
    private Vector2 initialPos;

    private void Awake() {
        enemyHealth = GetComponent<BeetleHealth>();
    }

    private void Start() {
        initialPos = transform.position;
    }

    private void Update()
    {
        if(enemyHealth.IsAlive) Movement();
    }

    private void Movement()
    {
        horizontalMovement = Mathf.Cos(Time.time * speed) * amplitude;
        
        // Update facing direction
        // if (horizontalMovement < 0)
        //     transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
        // else
        //     transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        
        transform.position = initialPos + new Vector2(horizontalMovement, 0);
    }

    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.white;
        // if (debug && EditorApplication.isPlaying)
        // {
        //     var start = new Vector3(initialPos.x + amplitude, initialPos.y, transform.position.z);
        //     var end = new Vector3(initialPos.x - amplitude, initialPos.y, transform.position.z);
        //     Gizmos.DrawLine(start, end);
        // }
    }
}
