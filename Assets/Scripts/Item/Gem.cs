using System;
using Manager;
using UnityEngine;
using UnityEngine.Events;

public class Gem : MonoBehaviour
{
    [SerializeField] private float degreePerSecond = 5f;
    public UnityEvent onCollected;

    private Animator animator;
    private AudioSource audioSource;

    private const string playerTag = "Player";
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        RotateAround();
    }

    private void RotateAround()
    {
        transform.Rotate(Vector3.up, degreePerSecond);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(playerTag)){
            
            //Invoke Events
            onCollected?.Invoke();
            SaveTime();
            GameManager.Instance.playerWinEventChannel.RaiseEvent();
            
            //Other logic
            animator.SetTrigger(AnimationString.isClaimed);
            other.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            if (audioSource != null)
            {
                audioSource.Stop();
                audioSource.Play();
            }

            // Destroy the gem after a delay
            Destroy(this.gameObject, 0.4f);
        }
    }

    private void SaveTime()
    {
        FirebaseManager.Instance.DbRef.Child("Time").SetValueAsync(100);
    }
}