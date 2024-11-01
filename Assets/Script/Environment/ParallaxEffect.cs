using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform subject;
    public Camera cam;
    private Vector2 startPos;
    private float startPosZ;

    private Vector2 travel => (Vector2)cam.transform.position - startPos;

    private float distanceToSubject => transform.position.z - subject.position.z;
    private float clippingPlane => (cam.transform.position.z + (distanceToSubject > 0 ? cam.farClipPlane : cam.nearClipPlane));
    private float parallaxFactor => Mathf.Abs(distanceToSubject) / clippingPlane;


    private void Awake() {
        startPos = transform.position;
        startPosZ = transform.position.z;
    }

    private void FixedUpdate() {
        if(subject == null) return;
        Vector2 bgPos = startPos + (travel * parallaxFactor);
        transform.position = new Vector3(bgPos.x, transform.position.y, startPosZ);
    }
}
