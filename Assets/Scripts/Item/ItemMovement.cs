using UnityEngine;

public abstract class ItemMovement : MonoBehaviour
{
    public float amplitude = 0.5f;  // Độ lớn của chuyển động
    public float speed = 1.0f;      // Tốc độ chuyển động

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        // Tính toán chuyển động lên xuống
        float verticalMovement = Mathf.Sin(Time.time * speed) * amplitude;
        float horizontalMovement = Mathf.Cos(Time.time * speed) * amplitude; 

        // Áp dụng chuyển động vào vị trí của đối tượng
        transform.position = initialPosition + new Vector3(horizontalMovement, verticalMovement, 0);
    }
}
