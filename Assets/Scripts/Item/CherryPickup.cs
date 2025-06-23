using System;
using Manager;
using UI;
using UnityEngine;

public class CherryPickup : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [Range(0,1), SerializeField] private float volume;
    
    private const string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag(playerTag)){
            other.GetComponent<PlayerPresenter>().CollectCherry();
            SoundManager.Instance.PlaySFX(clip, volume);
            Destroy(gameObject);
        }
    }
}
