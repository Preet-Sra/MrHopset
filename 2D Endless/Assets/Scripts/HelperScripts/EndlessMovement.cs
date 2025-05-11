using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessMovement : MonoBehaviour
{
    [SerializeField] float platformLength = 40;
    GameManager gameManager;
    float currentSpeed;
    float startXPos;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        startXPos = transform.position.x;
        
    }

    public void SetSpeed(float _speed)
    {
        currentSpeed = _speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Reset"))
        {
            float farthestX = transform.position.x;
            EndlessMovement[] allPlatforms = gameManager.GetAllEndlessMovers();
            foreach (EndlessMovement platform in allPlatforms)
            {
                if (platform != this && platform.transform.position.x > farthestX)
                {
                    farthestX = platform.transform.position.x;
                }
            }

            // Position this platform right after the farthest one
            transform.position = new Vector3(farthestX + platformLength, transform.position.y, transform.position.z);
        }
    }

    public void ResetAtStartPos()
    {
        transform.position = new Vector3(startXPos, transform.position.y,  transform.position.z);
    }
}
