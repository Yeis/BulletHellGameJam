using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusSpawner : MonoBehaviour
{
    public GameObject projectile;
    public float minRotation, maxRotation;
    public int totalProjectiles = 10;
    public float delayBetweenProjectiles = 1f;
    public int totalProjectileWaves = 4;
    public bool isRandom;
    float[] rotations;
    private float timeSinceLastSpawned = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastSpawned = delayBetweenProjectiles;
        rotations = new float[totalProjectiles];
        if (!isRandom) {
            DistributedRotations();
        } 
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawned += Time.deltaTime;
        if(timeSinceLastSpawned >= delayBetweenProjectiles && totalProjectileWaves > 0) {
            timeSinceLastSpawned = 0.0f;
            totalProjectileWaves--;
            SpawnBullets();


        }
        else if (totalProjectileWaves == 0)
        {
            Destroy(gameObject);
        }
    }


    // Select a random rotation from min to max for each bullet
    public float[] RandomRotations()
    {
        for (int i = 0; i < totalProjectiles; i++)
        {
            rotations[i] = Random.Range(minRotation, maxRotation);
        }
        return rotations;
    }

    // This will set random rotations evenly distributed between the min and max Rotation.
    public float[] DistributedRotations()
    {
        for (int i = 0; i < totalProjectiles; i++)
        {
            var fraction = (float)i / ((float)totalProjectiles - 1);
            var difference = maxRotation - minRotation;
            var fractionOfDifference = fraction * difference;
            rotations[i] = fractionOfDifference + minRotation; // We add minRotation to undo Difference
        }
        return rotations;
    }

    public GameObject[] SpawnBullets()
    {
        if (isRandom)
        {
            // This is in Update because we want a random rotation for each bullet each time
            RandomRotations();
        }

        // Spawn Bullets
        GameObject[] spawnedBullets = new GameObject[totalProjectiles];
        for (int i = 0; i < totalProjectiles; i++)
        {
            projectile.GetComponent<RadiusBullet>().rotation = rotations[i];
            spawnedBullets[i] = Instantiate(projectile, transform);

        }
        return spawnedBullets;
    }

}
