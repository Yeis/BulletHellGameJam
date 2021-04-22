using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipController : MonoBehaviour
{
    public float distance = 1.0f;
    public bool useInitalCameraDistance = false;
    public GameLogic gameLogicReference;
    Animator animator;
    public bool isKill = false;
    private float actualDistance;
    private Vector3 lastMousePosition;
    public Text timeLeftLabel;
    public Text gameOverLabel;



    // Start is called before the first frame update
    void Start()
    {
        gameOverLabel.enabled = false;
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
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (isKill)
        {
            timeLeftLabel.enabled = false;
            gameOverLabel.enabled = true;
        }
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
            gameLogicReference.timeLeftToLose = 10; // que no pase de 10! 
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
