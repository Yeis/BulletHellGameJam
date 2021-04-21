using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject bombProjectile;
    public GameObject radiusSpawner;
    // Start is called before the first frame update

    public void Explode() {

        RadiusSpawner spawnerScript = radiusSpawner.GetComponent<RadiusSpawner>();
        spawnerScript.projectile = bombProjectile;
        spawnerScript.minRotation = 1;
        spawnerScript.maxRotation = 360;
        spawnerScript.bulletSpeed = 3.0f;
        spawnerScript.totalProjectiles = 30;
        spawnerScript.totalProjectileWaves = 1;
        spawnerScript.isRandom = false;
        spawnerScript.isBombBullet = true;

        Instantiate(spawnerScript, transform.position, Quaternion.identity);
    }
}
