﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public float distance = 1.0f;
    public bool useInitalCameraDistance = false;
    Animator animator;
    bool isKill = false;
    private float actualDistance;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Cursor.visible = false;
        if (useInitalCameraDistance)
        {
            Vector3 toObjectVector = transform.position - Camera.main.transform.position;
            Vector3 linearDistanceVector = Vector3.Project(toObjectVector, Camera.main.transform.forward);
            actualDistance = linearDistanceVector.magnitude;
        }
        else
        {
            actualDistance = distance;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!isKill)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = actualDistance;
            transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        }
        //player rotation is driven by the Horizontal 
        animator.SetFloat("MouseX", Input.GetAxis("Mouse X") * 2);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Bomb") {
            col.gameObject.GetComponent<Bomb>().Explode();
            Destroy(col.gameObject);
        }else if(col.tag != "BombBullet") {
            isKill = true;
            animator.SetTrigger("Destroy");
        }

    }

    public void Goodbye()
    {
        Destroy(gameObject);
    }
}
