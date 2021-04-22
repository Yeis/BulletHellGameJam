using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public float distance = 1.0f, recoveryFromHeart = 10.0f;
    public bool useInitalCameraDistance = false;
    public GameLogic gameLogicReference;
    Animator animator;
    public bool isKill = false;
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

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Bomb")
        {
            col.gameObject.GetComponent<Bomb>().Explode();
            Destroy(col.gameObject);
        }
        else if (col.tag == "Heart")
        {
            gameLogicReference.timeLeftToLose += recoveryFromHeart;
            Destroy(col.gameObject);
        }
        else if (col.tag != "BombBullet")
        {
            DestroySpaceship();
        }

    }

    public void DestroySpaceship()
    {
        isKill = true;
        animator.SetTrigger("Destroy");
    }

    public void Goodbye()
    {
        Destroy(gameObject);
    }
}
