using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    public GameObject projectile;

    private GameLogic gameLogicReference;

    [SerializeField]
    public int totalProjectiles = 10;

    [SerializeField]
    public float delayBetweenProjectiles = 1f;

    public AudioController audioController;

    public Vector3 projectileDirection = Vector3.right;
    public float bulletSpeed = 1.0f;
    private float timeSinceLastSpawned = 0.0f;
    private int projectilesSpawned = 0;

    void Start()
    {
        gameLogicReference = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        timeSinceLastSpawned = delayBetweenProjectiles;
    }
    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawned += Time.deltaTime;

        if (timeSinceLastSpawned >= delayBetweenProjectiles &&
            totalProjectiles > 0)
        {
            totalProjectiles--;
            timeSinceLastSpawned = 0.0f;
            //Decide how to instantiate depending on projectile type
            if (projectile.tag == "SinusoidalBullet")
            {
                projectile.GetComponent<SinusoidalMove>().offset += 1;
                projectile.GetComponent<SinusoidalMove>().direction = projectileDirection;
                projectile.GetComponent<SinusoidalMove>().moveSpeed = bulletSpeed;
                audioController.PlayClip("projectile");
                GameObject newProjectile = Instantiate(projectile, gameObject.transform.position, Quaternion.identity);
            }
            else if (projectile.tag == "LineBeam")
            {
                projectile.GetComponent<LineBeam>().direction = projectileDirection;
                projectile.GetComponent<LineBeam>().moveSpeed = bulletSpeed;
                audioController.PlayClip("projectile");
                GameObject newProjectile = Instantiate(projectile, gameObject.transform.position, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, projectileDirection)));
            } else if (projectile.tag == "Bomb" || projectile.tag == "Heart") {
                projectile.GetComponent<LineBeam>().direction = projectileDirection;
                projectile.GetComponent<LineBeam>().moveSpeed = bulletSpeed;
                GameObject newProjectile = Instantiate(projectile, gameObject.transform.position, Quaternion.identity);
            }
        }
        else if (totalProjectiles == 0)
        {
            Destroy(gameObject);
        }
    }
}
