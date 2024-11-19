using System;
using Manager;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [Tooltip("This window will pop up whenever you win"), SerializeField] private GameObject winWindow;
    [SerializeField] private float degreePerSecond = 5f;

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
            
            // Trigger claimed animation
            animator.SetTrigger(AnimationString.isClaimed);

            if (audioSource != null)
            {
                audioSource.Stop();
                audioSource.Play();
            }

            // Display win window
            winWindow.SetActive(true);

            // Invoke the OnPlayerWin event
            GameManager.Instance.playerWinEventChannel.RaiseEvent();

            // Destroy the gem after a delay
            Destroy(this.gameObject, 0.4f);
        }
    }
}