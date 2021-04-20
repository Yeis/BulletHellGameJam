using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    public float speed = 0.0f;

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.material.mainTextureOffset = new Vector2(0, Time.time * speed);
        // spriteRenderer.sharedMaterial.SetTextureOffset("_MainTex", offset); 
    }
}
