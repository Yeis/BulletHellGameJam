using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusoidalMove : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5f;

    [SerializeField]
    float frequency = 20f;

    [SerializeField]
    float magnitude = 0.5f;

    public float offset = 0.0f;

    private Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveRight();
    }

    void MoveRight() {
        position += transform.right * Time.deltaTime * moveSpeed;
        transform.position = position + transform.up * Mathf.Sin(Time.time * frequency + offset) * magnitude;
    }
}
