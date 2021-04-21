using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public float distance = 1.0f;
    public bool useInitalCameraDistance = false;
    Animator animator;
    bool isKill = false;
    private float actualDistance;
    private Vector3 lastMousePosition;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Cursor.visible = false;
        lastMousePosition = Input.mousePosition;

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

    void FixedUpdate()
    {

        if (Input.GetAxis("Mouse X") == 0 || Input.GetAxis("Mouse Y") == 0)
        {
            lastMousePosition = Input.mousePosition;
        }

        Vector3 mouseDelta = Input.mousePosition - lastMousePosition;

        if (mouseDelta.sqrMagnitude < 0.1f)
        {
            return; // don't do tiny rotations.
        }

        float angle = Mathf.Atan2(mouseDelta.y, mouseDelta.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        angle -= 90;
        Debug.Log(angle);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
                                                  transform.localEulerAngles.y,
                                                  angle);
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
