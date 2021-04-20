using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusoidalMove : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 1f;

    [SerializeField]
    float frequency = 3f;

    [SerializeField]
    float magnitude = 2f;

    [SerializeField]
    public Vector3 direction = Vector3.right;
    private Vector3 sineFrequencyDirection;
    public float offset = 0.0f;

    private Vector3 position;
    private Rigidbody2D rigidbody2D;
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        rigidbody2D = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (direction == Vector3.right || direction == Vector3.left)
        {
            sineFrequencyDirection = Vector3.up;
        }
        else
        {
            sineFrequencyDirection = Vector3.right;
        }

    }



    void FixedUpdate()
    {
        Move();
        if (!spriteRenderer.isVisible)
        {
            Destroy(gameObject);
        }
    }

    void Move()
    {
        position += direction * Time.deltaTime * moveSpeed;

        rigidbody2D.MovePosition(position + sineFrequencyDirection * Mathf.Sin(Time.time * frequency + offset) * magnitude);
    }

    //Might end up using them depending on the final implementation
    public void Pause()
    {
        // moveSpeed /=2;
        // frequency /= 2;

    }

    public void Stop()
    {
        // moveSpeed = 0;
        // frequency = 0;
    }

    public void Resume()
    {
        // moveSpeed = 1f;
        // frequency = 3f;

    }
}
