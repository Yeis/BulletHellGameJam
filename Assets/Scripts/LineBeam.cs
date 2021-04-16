using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBeam : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Vector3 direction = Vector3.right;

    private Vector3 position; 
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody2D;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
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
        position += direction * Time.deltaTime * moveSpeed;
        rigidbody2D.MovePosition(position);
    }
}
