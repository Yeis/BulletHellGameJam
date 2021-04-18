using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public List<GameObject> projectiles; 
    public GameObject spawner;
    public GameObject radiusSpawner;

    public GameObject lineBeamProjectile;
    public GameObject sinusoidalProjectile;
    public GameObject shurikenProjectile;
    public GameObject greenProjectile;

    public float spawningTime = 0.1f;
    private bool isPaused = false , isStopped = false;
    private Vector3 mainCameraLimits;
    int cameraLimitXRound, cameraLimitYRound;

    public int difficulty = 1;
    private System.Random random;
    // Start is called before the first frame update
    private string[] projectileTypes = new string[4] {"Sinusoidal","LineBeam", "FireWork", "Flamethrower"};
    private string[] projectileDirections = new string[4] {"Left","Right", "Down", "Up"};
    private float timeSinceLastSpawned = 0f;

    void Awake()
    {
        projectiles =  new List<GameObject>();
    }
    void Start()
    {
        random = new System.Random();
        Camera mainCamera = Camera.main;
        mainCameraLimits = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight, mainCamera.nearClipPlane));
        cameraLimitXRound = Mathf.RoundToInt(mainCameraLimits.x);
        cameraLimitYRound = Mathf.RoundToInt(mainCameraLimits.y);

        timeSinceLastSpawned = spawningTime;
    }

    // Update is called once per frame
    void Update()
    {
        //Spawn objects randomly
        timeSinceLastSpawned += Time.deltaTime;
        if (timeSinceLastSpawned >= spawningTime) {
            timeSinceLastSpawned = 0.0f;
            string projectileType = projectileTypes[random.Next(projectileTypes.Length)];

            if(projectileType == "Sinusoidal") {
                string projectileDirection = projectileDirections[random.Next(projectileDirections.Length)];
                Spawner spawnerScript = spawner.GetComponent<Spawner>();
                spawnerScript.projectile = sinusoidalProjectile;
                spawnerScript.delayBetweenProjectiles = 0.5f;
                spawnerScript.totalProjectiles = 20;
                switch(projectileDirection) {
                    case "Left":
                        spawnerScript.projectileDirection = Vector3.left;
                        Instantiate(spawner, new Vector3(cameraLimitXRound, random.Next(-cameraLimitYRound, cameraLimitYRound),0), Quaternion.identity);
                        break;
                    case "Right":
                        spawnerScript.projectileDirection = Vector3.right;
                        Instantiate(spawner, new Vector3(-cameraLimitXRound, random.Next(-cameraLimitYRound, cameraLimitYRound), 0), Quaternion.identity);
                        break;
                    case "Up":
                        spawnerScript.projectileDirection = Vector3.up;
                        Instantiate(spawner, new Vector3(random.Next(-cameraLimitXRound, cameraLimitXRound), -cameraLimitYRound, 0), Quaternion.identity);
                        break;
                    case "Down":
                        spawnerScript.projectileDirection = Vector3.down;
                        Instantiate(spawner, new Vector3(random.Next(-cameraLimitXRound, cameraLimitXRound), cameraLimitYRound, 0), Quaternion.identity);
                        break;
                }
            } else if (projectileType == "LineBeam") {
                string projectileDirection = projectileDirections[random.Next(projectileDirections.Length)];
                Spawner spawnerScript = spawner.GetComponent<Spawner>();
                spawnerScript.projectile = lineBeamProjectile;
                spawnerScript.delayBetweenProjectiles = 2.0f;
                spawnerScript.totalProjectiles = 3;
                switch (projectileDirection)
                {
                    case "Left":
                        spawnerScript.projectileDirection = Vector3.left;
                        Instantiate(spawner, new Vector3(cameraLimitXRound, random.Next(-cameraLimitYRound, cameraLimitYRound), 0),  Quaternion.identity);
                        break;
                    case "Right":
                        spawnerScript.projectileDirection = Vector3.right;
                        Instantiate(spawner, new Vector3(-cameraLimitXRound, random.Next(-cameraLimitYRound, cameraLimitYRound), 0), Quaternion.identity);
                        break;
                    case "Up":
                        spawnerScript.projectileDirection = Vector3.up;
                        Instantiate(spawner, new Vector3(random.Next(-cameraLimitXRound, cameraLimitXRound), -cameraLimitYRound, 0), Quaternion.identity);
                        break;
                    case "Down":
                        spawnerScript.projectileDirection = Vector3.down;
                        Instantiate(spawner, new Vector3(random.Next(-cameraLimitXRound, cameraLimitXRound), cameraLimitYRound, 0), Quaternion.identity);
                        break;
                }
            } else if(projectileType == "FireWork") {
                RadiusSpawner spawnerScript = radiusSpawner.GetComponent<RadiusSpawner>();
                spawnerScript.projectile = shurikenProjectile;
                spawnerScript.minRotation = 1;
                spawnerScript.maxRotation = 360;
                spawnerScript.totalProjectiles = 8;
                spawnerScript.totalProjectileWaves = 3;

                Instantiate(radiusSpawner, new Vector3(random.Next(-cameraLimitXRound + (cameraLimitXRound /4), cameraLimitXRound - (cameraLimitXRound / 4)), 
                    random.Next(-cameraLimitYRound + (cameraLimitYRound / 4), cameraLimitYRound - (cameraLimitYRound / 4)),0), Quaternion.identity);
            } else if(projectileType == "Flamethrower") {
                RadiusSpawner spawnerScript = radiusSpawner.GetComponent<RadiusSpawner>();
                string projectileDirection = projectileDirections[random.Next(projectileDirections.Length)];
                spawnerScript.projectile = greenProjectile;
                spawnerScript.totalProjectiles = 3;
                spawnerScript.totalProjectileWaves = 8;
                switch (projectileDirection)
                {
                    case "Left":
                        spawnerScript.minRotation = 135;
                        spawnerScript.maxRotation = 225;
                        Instantiate(radiusSpawner, new Vector3(cameraLimitXRound, random.Next(-cameraLimitYRound, cameraLimitYRound), 0), Quaternion.identity);
                        break;
                    case "Right":
                        spawnerScript.minRotation = 315;
                        spawnerScript.maxRotation = 405;
                        Instantiate(radiusSpawner, new Vector3(-cameraLimitXRound, random.Next(-cameraLimitYRound, cameraLimitYRound), 0), Quaternion.identity);
                        break;
                    case "Up":
                        spawnerScript.minRotation = 45;
                        spawnerScript.maxRotation = 135;
                        Instantiate(radiusSpawner, new Vector3(random.Next(-cameraLimitXRound, cameraLimitXRound), -cameraLimitYRound, 0), Quaternion.identity);
                        break;
                    case "Down":
                        spawnerScript.minRotation = 225;
                        spawnerScript.maxRotation = 315;
                        Instantiate(radiusSpawner, new Vector3(random.Next(-cameraLimitXRound, cameraLimitXRound), cameraLimitYRound, 0), Quaternion.identity);
                        break;
                }
            }
        }

        //Input values
        if(Input.GetKeyDown(KeyCode.S) && !isStopped) {
            //Stop
            Time.timeScale = 0f;
            isStopped = true;
            isPaused = false;
            // foreach (GameObject projectile in projectiles)
            // {
            //     SinusoidalMove movementScript = projectile.GetComponent<SinusoidalMove>();
            //     movementScript.Stop();
            // }

        } else if(Input.GetKeyDown(KeyCode.D) && !isPaused) {
            //Pause
            Time.timeScale = 0.5f;

            isStopped = false;
            isPaused = true;
            // foreach (GameObject projectile in projectiles)
            // {
            //     SinusoidalMove movementScript = projectile.GetComponent<SinusoidalMove>();
            //     movementScript.Pause();
            // }
        } else if(Input.GetKeyDown(KeyCode.A) && (isPaused || isStopped)) {
            //Resume
            Time.timeScale = 1;

            isStopped = false;
            isPaused = false;
            // foreach (GameObject projectile in projectiles)
            // {
            //     SinusoidalMove movementScript = projectile.GetComponent<SinusoidalMove>();
            //     movementScript.Resume();
            // }
        }
    }
}
