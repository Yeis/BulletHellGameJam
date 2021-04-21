using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public GameObject player;
    public GameObject spawner;
    public GameObject radiusSpawner;
    public GameObject bomb;
    public GameObject heart;

    public GameObject lineBeamProjectile;
    public GameObject sinusoidalProjectile;
    public GameObject shurikenProjectile;
    public GameObject greenProjectile;

    public Text difficultyLabel;
    public Text timeLeftLabel;
    public Text timeFuelLeftLabel;

    public float spawningTime = 0.1f , stillnessTimePenalty = 3.0f, stillnessRadio = 1f, difficultyIncrease = 10f;
    public float timeLeftToLose = 10.0f, powerUpTime, heartTime = 5.0f;
    public Vector3 playerPosition;
    private bool isPaused = false , isStopped = false, isStill = false;
    private Vector3 mainCameraLimits;
    int cameraLimitXRound, cameraLimitYRound;

    public int difficulty = 1;
    private System.Random random;
    // Start is called before the first frame update
    private string[] projectileTypes = new string[5] {"Sinusoidal","LineBeam", "FireWork", "Flamethrower", "Bomb"};
    // private string[] projectileTypes = new string[2] { "Flamethrower", "Bomb" };

    private string[] projectileDirections = new string[4] {"Left","Right", "Down", "Up"};
    private float timeSinceLastSpawned = 0f, timeSinceLastMoved = 0f, timeSinceLastDifficultyIncrease = 0f;
    private float timeSinceLastHeart = 0f, timeSinceLastPowerUp = 0f;
    void Start()
    {
        random = new System.Random();
        Camera mainCamera = Camera.main;
        mainCameraLimits = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight, mainCamera.nearClipPlane));
        cameraLimitXRound = Mathf.RoundToInt(mainCameraLimits.x);
        cameraLimitYRound = Mathf.RoundToInt(mainCameraLimits.y);
        playerPosition = player.transform.position;
        timeSinceLastSpawned = spawningTime;
        powerUpTime = UnityEngine.Random.Range(5f, 10f + (difficulty / 2));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = isStill? Color.red : Color.green;
        Gizmos.DrawWireSphere(playerPosition, stillnessRadio);
    }
    // Update is called once per frame
    void Update()
    {
        //Update Timers
        timeSinceLastSpawned += Time.deltaTime;
        timeSinceLastDifficultyIncrease += Time.deltaTime;
        timeSinceLastPowerUp += Time.deltaTime;
        timeSinceLastHeart += Time.deltaTime;
        timeLeftToLose -= Time.deltaTime;

        //Fuel Logic
        if(timeLeftToLose <= 0.0f) {
            player.GetComponent<SpaceshipController>().DestroySpaceship();
        }

        //Spawning Fuel logic
        if(timeSinceLastHeart >= heartTime) {
            timeSinceLastHeart = 0.0f;
            SpawnPowerUp(heart);
        }

        //PowerUp Logic
        if(timeSinceLastPowerUp >= powerUpTime) {
            timeSinceLastPowerUp = 0.0f;
            powerUpTime = UnityEngine.Random.Range(5f, 10f + (difficulty / 2));
            SpawnPowerUp(bomb);
        }

        //Difficulty logic
        if(timeSinceLastDifficultyIncrease >= difficultyIncrease) {
            timeSinceLastDifficultyIncrease = 0.0f;
            difficulty++;
            //First argument is max difficulty
            spawningTime= Mathf.Max(1f, spawningTime - 0.3f);
        }

        //Stillness logic
        if (player != null && IsPlayerStill())
        {
            timeSinceLastMoved += Time.deltaTime;
            if(timeSinceLastMoved >= stillnessTimePenalty) {
                isStill = true;
            }
        } else {
            isStill = false;
            timeSinceLastMoved = 0.0f;
            playerPosition = player.transform.position;

        }

        //Spawn objects randomly
        if (timeSinceLastSpawned >= spawningTime)
        {
            timeSinceLastSpawned = 0.0f;
            SpawnSpawner();
        }

        //Input values
        if (Input.GetKeyDown(KeyCode.S) && !isStopped)
        {
            //Stop
            Time.timeScale = 0f;
            isStopped = true;
            isPaused = false;
        }
        else if (Input.GetKeyDown(KeyCode.D) && !isPaused)
        {
            //Pause
            Time.timeScale = 0.5f;
            isStopped = false;
            isPaused = true;
        }
        else if (Input.GetKeyDown(KeyCode.A) && (isPaused || isStopped))
        {
            //Resume
            Time.timeScale = 1;
            isStopped = false;
            isPaused = false;
        }

        //UI
        UpdateUI();
        
    }

    private void UpdateUI() {
        difficultyLabel.text = "Difficulty: " + difficulty;
        timeFuelLeftLabel.text = "Time Fuel Left: " + timeLeftToLose;
    }

    private bool IsPlayerStill() {
        return Vector2.Distance(player.transform.position, playerPosition) < stillnessRadio;
    }

    private void SpawnPowerUp(GameObject powerUp) {
        string projectileDirection = projectileDirections[random.Next(projectileDirections.Length)];
        Spawner spawnerScript = spawner.GetComponent<Spawner>();
        spawnerScript.projectile = powerUp;
        spawnerScript.totalProjectiles = 1;
        spawnerScript.delayBetweenProjectiles = 2.0f;
        spawnerScript.bulletSpeed = 2.0f;
        switch (projectileDirection)
        {
            case "Left":
                spawnerScript.projectileDirection = Vector3.left;
                Instantiate(spawner, new Vector3(cameraLimitXRound, random.Next(-cameraLimitYRound, cameraLimitYRound), 0), Quaternion.identity);
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
    }

    
    private void SpawnSpawner() {
        string projectileType = projectileTypes[random.Next(projectileTypes.Length)];
        // projectileType = "Bomb";
        if (projectileType == "Sinusoidal")
        {
            string projectileDirection = projectileDirections[random.Next(projectileDirections.Length)];
            Spawner spawnerScript = spawner.GetComponent<Spawner>();
            spawnerScript.projectile = sinusoidalProjectile;
            spawnerScript.delayBetweenProjectiles = 0.3f;
            spawnerScript.bulletSpeed = UnityEngine.Random.Range(1f, 1f + (difficulty/2));
            spawnerScript.totalProjectiles = random.Next(6, 6 + (difficulty/2));
            switch (projectileDirection)
            {
                case "Left":
                    spawnerScript.projectileDirection = Vector3.left;
                    if(isStill) Instantiate(spawner, new Vector3(cameraLimitXRound, random.Next((int) (playerPosition.y - stillnessRadio), (int)(playerPosition.y + stillnessRadio)), 0), Quaternion.identity);
                    else Instantiate(spawner, new Vector3(cameraLimitXRound, random.Next(-cameraLimitYRound, cameraLimitYRound), 0), Quaternion.identity);
                    break;
                case "Right":
                    spawnerScript.projectileDirection = Vector3.right;
                    if (isStill) Instantiate(spawner, new Vector3(-cameraLimitXRound, random.Next((int)(playerPosition.y - stillnessRadio), (int)(playerPosition.y + stillnessRadio)), 0), Quaternion.identity);
                    else Instantiate(spawner, new Vector3(-cameraLimitXRound, random.Next(-cameraLimitYRound, cameraLimitYRound), 0), Quaternion.identity);
                    break;
                case "Up":
                    spawnerScript.projectileDirection = Vector3.up;
                    if(isStill) Instantiate(spawner, new Vector3(random.Next((int)(playerPosition.x - stillnessRadio), (int)(playerPosition.x - stillnessRadio)), -cameraLimitYRound, 0), Quaternion.identity);
                    else Instantiate(spawner, new Vector3(random.Next(-cameraLimitXRound, cameraLimitXRound), -cameraLimitYRound, 0), Quaternion.identity);
                    break;
                case "Down":
                    spawnerScript.projectileDirection = Vector3.down;
                    if (isStill) Instantiate(spawner, new Vector3(random.Next((int)(playerPosition.x - stillnessRadio), (int)(playerPosition.x - stillnessRadio)), cameraLimitYRound, 0), Quaternion.identity);
                    else Instantiate(spawner, new Vector3(random.Next(-cameraLimitXRound, cameraLimitXRound), cameraLimitYRound, 0), Quaternion.identity);
                    break;
            }
        }
        else if (projectileType == "LineBeam")
        {
            string projectileDirection = projectileDirections[random.Next(projectileDirections.Length)];
            Spawner spawnerScript = spawner.GetComponent<Spawner>();
            spawnerScript.projectile = lineBeamProjectile;
            spawnerScript.delayBetweenProjectiles = 2.0f;
            spawnerScript.totalProjectiles = random.Next(3, 3 + (difficulty/2));
            spawnerScript.bulletSpeed = UnityEngine.Random.Range(1f, 1f + (difficulty / 2));
            switch (projectileDirection)
            {
                case "Left":
                    spawnerScript.projectileDirection = Vector3.left;
                    if (isStill) Instantiate(spawner, new Vector3(cameraLimitXRound, random.Next((int)(playerPosition.y - stillnessRadio), (int)(playerPosition.y + stillnessRadio)), 0), Quaternion.identity);
                    else Instantiate(spawner, new Vector3(cameraLimitXRound, random.Next(-cameraLimitYRound, cameraLimitYRound), 0), Quaternion.identity);
                    break;
                case "Right":
                    spawnerScript.projectileDirection = Vector3.right;
                    if (isStill) Instantiate(spawner, new Vector3(-cameraLimitXRound, random.Next((int)(playerPosition.y - stillnessRadio), (int)(playerPosition.y + stillnessRadio)), 0), Quaternion.identity);
                    else Instantiate(spawner, new Vector3(-cameraLimitXRound, random.Next(-cameraLimitYRound, cameraLimitYRound), 0), Quaternion.identity);
                    break;
                case "Up":
                    spawnerScript.projectileDirection = Vector3.up;
                    if (isStill) Instantiate(spawner, new Vector3(random.Next((int)(playerPosition.x - stillnessRadio), (int)(playerPosition.x - stillnessRadio)), -cameraLimitYRound, 0), Quaternion.identity);
                    else Instantiate(spawner, new Vector3(random.Next(-cameraLimitXRound, cameraLimitXRound), -cameraLimitYRound, 0), Quaternion.identity);
                    break;
                case "Down":
                    spawnerScript.projectileDirection = Vector3.down;
                    if (isStill) Instantiate(spawner, new Vector3(random.Next((int)(playerPosition.x - stillnessRadio), (int)(playerPosition.x - stillnessRadio)), cameraLimitYRound, 0), Quaternion.identity);
                    else Instantiate(spawner, new Vector3(random.Next(-cameraLimitXRound, cameraLimitXRound), cameraLimitYRound, 0), Quaternion.identity);
                    break;
            }
        }
        else if (projectileType == "FireWork")
        {
            RadiusSpawner spawnerScript = radiusSpawner.GetComponent<RadiusSpawner>();
            spawnerScript.projectile = shurikenProjectile;
            spawnerScript.minRotation = 1;
            spawnerScript.maxRotation = 360;
            spawnerScript.bulletSpeed = UnityEngine.Random.Range(3f, 3f + (difficulty / 2));
            spawnerScript.totalProjectiles = random.Next(6, 6 + (difficulty / 2)); 
            spawnerScript.totalProjectileWaves = random.Next(3, 3 + (difficulty / 2));
            spawnerScript.isRandom = random.Next(1, difficulty) > 1;
            spawnerScript.isBombBullet = false;


            if(isStill) {
                Instantiate(radiusSpawner, new Vector3(random.Next((int)(playerPosition.x - stillnessRadio), (int)(playerPosition.x + stillnessRadio)),
                    random.Next((int)(playerPosition.y - stillnessRadio),(int)(playerPosition.y + stillnessRadio) ), 0), Quaternion.identity);
            } else {
                Instantiate(radiusSpawner, new Vector3(random.Next(-cameraLimitXRound, cameraLimitXRound), random.Next(-cameraLimitYRound, cameraLimitYRound), 0), Quaternion.identity);
            }
        }
        else if (projectileType == "Flamethrower")
        {
            RadiusSpawner spawnerScript = radiusSpawner.GetComponent<RadiusSpawner>();
            string projectileDirection = projectileDirections[random.Next(projectileDirections.Length)];
            spawnerScript.projectile = greenProjectile;
            spawnerScript.totalProjectiles = random.Next(3, 3 + (difficulty / 2));
            spawnerScript.totalProjectileWaves = random.Next(6, 6 + (difficulty / 2));
            spawnerScript.bulletSpeed = UnityEngine.Random.Range(3f, 3f + (difficulty / 2));
            spawnerScript.isRandom = random.Next(1, difficulty) > 30;
            spawnerScript.isBombBullet = false;

            switch (projectileDirection)
            {
                case "Left":
                    spawnerScript.minRotation = 135;
                    spawnerScript.maxRotation = 225;
                    if (isStill) Instantiate(radiusSpawner, new Vector3(cameraLimitXRound, random.Next((int)(playerPosition.y - stillnessRadio), (int)(playerPosition.y + stillnessRadio)), 0), Quaternion.identity);
                    else Instantiate(radiusSpawner, new Vector3(cameraLimitXRound, random.Next(-cameraLimitYRound, cameraLimitYRound), 0), Quaternion.identity);
                    break;
                case "Right":
                    spawnerScript.minRotation = 315;
                    spawnerScript.maxRotation = 405;
                    if (isStill) Instantiate(radiusSpawner, new Vector3(-cameraLimitXRound, random.Next((int)(playerPosition.y - stillnessRadio), (int)(playerPosition.y + stillnessRadio)), 0), Quaternion.identity);
                    else Instantiate(radiusSpawner, new Vector3(-cameraLimitXRound, random.Next(-cameraLimitYRound, cameraLimitYRound), 0), Quaternion.identity);
                    break;
                case "Up":
                    spawnerScript.minRotation = 45;
                    spawnerScript.maxRotation = 135;
                    if (isStill) Instantiate(radiusSpawner, new Vector3(random.Next((int)(playerPosition.x - stillnessRadio), (int)(playerPosition.x - stillnessRadio)), -cameraLimitYRound, 0), Quaternion.identity);
                    else Instantiate(radiusSpawner, new Vector3(random.Next(-cameraLimitXRound, cameraLimitXRound), -cameraLimitYRound, 0), Quaternion.identity);             
                    break;
                case "Down":
                    spawnerScript.minRotation = 225;
                    spawnerScript.maxRotation = 315;
                    if (isStill) Instantiate(radiusSpawner, new Vector3(random.Next((int)(playerPosition.x - stillnessRadio), (int)(playerPosition.x - stillnessRadio)), cameraLimitYRound, 0), Quaternion.identity);
                    else Instantiate(radiusSpawner, new Vector3(random.Next(-cameraLimitXRound, cameraLimitXRound), cameraLimitYRound, 0), Quaternion.identity);
                    break;
            }
        }
    }


}
