using UnityEngine;
using UnityEngine.Events;

public class TimeStopper : MonoBehaviour {
    public UnityEvent OnTriggerEnterEvent;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Time Stopper Triggered");
            OnTriggerEnterEvent?.Invoke();
        }
    }
}