using UnityEngine;

public class Gem : MonoBehaviour
{
    Animator animator;
    public AudioSource sound;

    [Tooltip("This window will pop up whenever you win")]
    public GameObject winWindow;

    [SerializeField] private float degreePerSecons = 5f;

    private void Awake() {
        animator = GetComponent<Animator>();
        sound = GameObject.Find("Win Sound").GetComponent<AudioSource>();
    }

    private void FixedUpdate() {
        RotateAround();
    }

    private void RotateAround()
    {
        transform.Rotate(Vector3.up, degreePerSecons);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        animator.SetTrigger(AnimationString.isClaimed);
        sound.gameObject.transform.position = transform.position;
        sound?.Stop();
        sound?.Play();
        winWindow.SetActive(true);
        Destroy(gameObject, 0.4f);
    }
}
