using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public static GameManager instance;
    PlayerController playerController;
    UIManager uIManager;
    [SerializeField]EndlessMovement[] endlessMovements;
    bool isGameOver = false;
    bool gameStarted;
    float distanceTraveled;
    [SerializeField]float distanceIncreaseRate=0.5f;

    [Header("Speed")]
    [SerializeField] float startingSpeed;
    float currentSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float increaserate;
    [SerializeField] float increaseWaitTime;
    [SerializeField] float boostSpeed;


    [Header("PowerUps")]
    [SerializeField] float powerUpWait;
    bool spawnPowerUp;
    bool isBoosting;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        currentSpeed = startingSpeed;
        
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        Time.timeScale = 1;
        Application.runInBackground = false;
        uIManager = UIManager.instance;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
       // StartGame();
        endlessMovements[3].GetComponent<PlatformContentSpawner>().SpawnContent();
        endlessMovements[4].GetComponent<PlatformContentSpawner>().SpawnContent();

    }

    public void StartGame()
    {
        foreach (EndlessMovement en in endlessMovements)
        {
            en.SetSpeed(startingSpeed);
        }
        StartCoroutine(IncreaseSpeedOverTime());
        spawnPowerUp = true;
        gameStarted = true;
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public EndlessMovement[] GetAllEndlessMovers()
    {
        return endlessMovements;
    }

    IEnumerator IncreaseSpeedOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(increaseWaitTime);

            if (isGameOver) yield break;

            // Skip speed increase if currently boosting
            if (isBoosting) continue;

            if (currentSpeed < maxSpeed)
            {
                currentSpeed += increaserate;

                foreach (EndlessMovement en in GetAllEndlessMovers())
                {
                    en.SetSpeed(currentSpeed);
                }
            }
        }
    }

    public IEnumerator EnablePowerUps()
    {
        
        yield return new WaitForSeconds(powerUpWait);
        spawnPowerUp = true;
    }

    public bool CanSpawnPowerUp()
    {
        return spawnPowerUp;
    }
    public void PowerUpSpawned()
    {
        spawnPowerUp = false;
        StartCoroutine(EnablePowerUps());
    }
  
    void Update()
    {
        if (!isGameOver && gameStarted)
        {
            distanceTraveled += (Time.deltaTime * distanceIncreaseRate);
            uIManager.DisplayDistance(distanceTraveled);
        }
    }

    // Call this function when game over is triggered
    public void GameOver()
    {
        isGameOver = true;
        foreach(EndlessMovement em in endlessMovements)
        {
            
            em.SetSpeed(0);
        }

        StartCoroutine(GameOverSequence());
    }

    IEnumerator GameOverSequence()
    {
        yield return new WaitForSeconds(2f);
        uIManager.GameOver();
    }

    public void Revived()
    {
        isGameOver = false;
        
        foreach(EndlessMovement em in endlessMovements)
        {
            em.SetSpeed(currentSpeed);
            em.GetComponent<PlatformContentSpawner>().ClearChildren();
            em.ResetAtStartPos();

        }

        endlessMovements[3].GetComponent<PlatformContentSpawner>().SpawnContent();
        endlessMovements[4].GetComponent<PlatformContentSpawner>().SpawnContent();
        StartCoroutine(IncreaseSpeedOverTime());

    }

    public void StartBoost()
    {
        if (isBoosting) return;
        Camera.main.GetComponent<CameraShake>().StartContinuousShake();

        isBoosting = true;
    
        foreach (EndlessMovement em in endlessMovements)
        {
            em.SetSpeed(boostSpeed);
        }
    }

    public void StopBoost()
    {
        if (!isBoosting) return;
        Camera.main.GetComponent<CameraShake>().StopContinuousShake();

        foreach (EndlessMovement em in endlessMovements)
        {
            em.SetSpeed(currentSpeed);
        }
        StartCoroutine(IncreaseSpeedOverTime());
        isBoosting = false;
    }
}
