using UnityEngine;
using System.Collections.Generic;

public class PlatformContentSpawner : MonoBehaviour
{
    [SerializeField] Transform[] obstacleSpawnPoints;
    [SerializeField] Transform[] coinsSpawnPoints;
    [SerializeField] Transform[] powerUpSpawnPoints;

    GameManager gameManager;

    [SerializeField] float yminPos = 4.8f, ymaxPos = 8.6f;
    ObjectPooler objectPooler;

    private void Start()
    {
        objectPooler = ObjectPooler.instance;
        gameManager = GameManager.instance;
    }



    public void SpawnContent()
    {

        List<Transform> shuffledObstacles = new List<Transform>(obstacleSpawnPoints);
        ShuffleList(shuffledObstacles);

        int obstacleCount = Random.Range(2, 4); // 2 to 3 inclusive
        for (int i = 0; i < obstacleCount && i < shuffledObstacles.Count; i++)
        {
            GameObject obstacle = objectPooler.GetRandomFromCategory("Obstacle");
            if (obstacle != null)
            {
                Transform point = shuffledObstacles[i];
                obstacle.transform.parent = point;
                obstacle.transform.localPosition = new Vector2(0, Random.Range(yminPos, ymaxPos));
                obstacle.transform.rotation = Quaternion.identity;
            }
        }

        // === Spawn Coins ===
        List<Transform> shuffledCoins = new List<Transform>(coinsSpawnPoints);
        ShuffleList(shuffledCoins);

        int coinCount = Random.Range(3, 6); // 2 to 3 inclusive
        for (int i = 0; i < coinCount && i < shuffledCoins.Count; i++)
        {
            GameObject coin = objectPooler.GetRandomFromCategory("Coin");
            if (coin != null)
            {
                Transform point = shuffledCoins[i];
                coin.transform.parent = point;
                coin.transform.localPosition = new Vector2(0, Random.Range(yminPos, ymaxPos));
                coin.transform.rotation = Quaternion.identity;
            }
        }


    }

    void ShuffleList(List<Transform> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randIndex = Random.Range(i, list.Count);
            (list[i], list[randIndex]) = (list[randIndex], list[i]);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Reset"))
        {
            ClearChildren();
            SpawnContent();
            CheckAndSpawnPowerUp();
        }
    }


    void CheckAndSpawnPowerUp()
    {
        if (gameManager.CanSpawnPowerUp())
        {
            gameManager.PowerUpSpawned();
            Transform point = powerUpSpawnPoints[Random.Range(0, powerUpSpawnPoints.Length)];
            GameObject powerup = objectPooler.GetRandomFromCategory("PowerUp");
         
            if (powerup != null)
            {
                powerup.transform.parent = point;
                powerup.transform.localPosition = new Vector2(0, Random.Range(yminPos, ymaxPos));
                powerup.transform.rotation = Quaternion.identity;
            }

        }
    }

    public void ClearChildren()
    {
        foreach (Transform point in obstacleSpawnPoints)
        {
            if (point.childCount > 0)
            {
                for (int i = point.childCount - 1; i >= 0; i--)
                {
                    GameObject obj = point.GetChild(i).gameObject;
                    obj.SetActive(false);
                    objectPooler.ReturnObject(obj);
                }
            }
        }

        foreach (Transform point in coinsSpawnPoints)
        {
            if (point.childCount > 0)
            {
                for (int i = point.childCount - 1; i >= 0; i--)
                {
                    GameObject obj = point.GetChild(i).gameObject;
                    obj.SetActive(false);
                    objectPooler.ReturnObject(obj);
                }
            }
        }

        foreach (Transform point in powerUpSpawnPoints)
        {
            if (point.childCount > 0)
            {
                for (int i = point.childCount - 1; i >= 0; i--)
                {
                    
                    GameObject obj = point.GetChild(i).gameObject;
                    obj.SetActive(false);
                    objectPooler.ReturnObject(obj);
                }
            }
        }
    }
}
