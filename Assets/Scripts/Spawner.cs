using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    GameObject projectile;

    [SerializeField]
    int totalProjectiles = 10;

    [SerializeField]
    float delayBetweenProjectiles = 1f;

    private float timeSinceLastSpawned = 0.0f;
    private int projectilesSpawned = 0;

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawned += Time.deltaTime;

        if(timeSinceLastSpawned >= delayBetweenProjectiles &&
            totalProjectiles > 0) {
            totalProjectiles--;
            timeSinceLastSpawned = 0.0f;

            projectile.GetComponent<SinusoidalMove>().offset += 1; 
            Instantiate(projectile);
            }
    }
}
