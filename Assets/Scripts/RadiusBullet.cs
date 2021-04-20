using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusBullet : MonoBehaviour
{
    public Vector2 velocity;
    public float moveSpeed;
    public float rotation;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody2D;
    private Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0,0, rotation);
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!spriteRenderer.isVisible)
        {
            Destroy(gameObject);
        }   
    }

    void FixedUpdate()
    {
        Move();
    }
    

    void Move() {
        // rigidbody2D.AddForce(velocity * moveSpeed * Time.deltaTime);
        transform.Translate(velocity * moveSpeed * Time.deltaTime);
    }
}
