using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PlayerHealth : MonoBehaviour
{
    [Header("Reference")]
    GameManager gameManager;
    Rigidbody2D rb;
    Collider2D col;
    float startGravityScale;
    [HideInInspector]public bool dead=false;
    bool revived;
    [SerializeField] int totalHealth = 3;
    int currentHealth;
    Vector2 startPos;
    CameraShake cameraShake;
    PlayerController playerController;

    [SerializeField] GameObject[] healthUI;
    [SerializeField] Transform coinTargetUI;
    int coinCount;
    SoundManager soundManager;
    [SerializeField] AudioSource coinPickSound,hurtSound,deathSound;

    [Header("DeathVariables")]
    [SerializeField] float upwardForce;
    [SerializeField] float rotationAngle, rotationSpeed;
    ObjectPooler objectPooler;
    [SerializeField] AudioSource ShieldCollideSound;

    [Header("HitEffects")]
    PlayerSkin playerSkin;
    [SerializeField] float invinsibleTime;
    [SerializeField] float flashRate;
    bool invinsible;
    

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerSkin = GetComponent<PlayerSkin>();
        startPos = transform.position;
        cameraShake = Camera.main.GetComponent<CameraShake>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        startGravityScale = rb.gravityScale;
        gameManager = GameManager.instance;
        soundManager = SoundManager.instance;
        currentHealth = totalHealth;
        objectPooler = ObjectPooler.instance;
        DisplayHealth();
    }

    public void ReduceHealth()
    {
        if (playerController.IsInvisibleFromBoost()) return;
        if (invinsible) return;
        if (dead) return;
        soundManager.PlayAudio(hurtSound);

        cameraShake.Shake();
        currentHealth--;
        if (currentHealth <= 0 )
        {
            dead = true;
            soundManager.PlayAudio(deathSound);
         
            StartCoroutine(DeathAnimation());
            gameManager.GameOver();
            currentHealth = 0;
        }
        else
        {

            StartCoroutine(StartInvinsibleAndFlash());
        }

        DisplayHealth();
    }


    IEnumerator StartInvinsibleAndFlash()
    {
        invinsible = true;

        SpriteRenderer sprite = playerSkin.GetActiveSkin().GetComponent<SpriteRenderer>();
        float elapsed = 0f;
        bool visible = true;

        while (elapsed < invinsibleTime)
        {
            visible = !visible;
            sprite.enabled = visible;

            yield return new WaitForSeconds(flashRate);
            elapsed += flashRate;
        }

        sprite.enabled = true; // make sure it's visible at the end
        invinsible = false;
    }

    public void AddHealth()
    {
        
        currentHealth++;
        Debug.Log(currentHealth);
        if (currentHealth > totalHealth)
            currentHealth = totalHealth;

        DisplayHealth();
    }

    public void DisplayHealth()
    {
        for (int i = 0; i < healthUI.Length; i++)
        {
            healthUI[i].SetActive(i < currentHealth);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            AnimateCoinToUI(collision.gameObject);
        }
        if (collision.CompareTag("Enemy"))
        {

            if (!ShieldActivated())
            {
                ReduceHealth();
            }
            else
            {
                soundManager.PlayAudio(ShieldCollideSound);
                collision.gameObject.SetActive(false);
            }
        }
    }

    void AnimateCoinToUI(GameObject coin)
    {
        Coin coinScript = coin.GetComponent<Coin>();
        Collider2D col = coin.GetComponent<Collider2D>();
        if (col) col.enabled = false;
        soundManager.PlayAudio(coinPickSound);
        // First pop-up effect
        Vector3 popUpPos = coin.transform.position + new Vector3(0, 3f, 0); // move up
        Sequence sequence = DOTween.Sequence();

        sequence.Append(coin.transform.DOMove(popUpPos, 0.2f).SetEase(Ease.OutQuad));
        sequence.Join(coin.transform.DOScale(1.5f, 0.2f).SetEase(Ease.OutBack));

        // Then move to UI while shrinking
        Vector3 targetPos = coinTargetUI.position;

        sequence.Append(coin.transform.DOMove(targetPos, 0.4f).SetEase(Ease.InQuad));
        sequence.Join(coin.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.InQuad));

        // On complete, update coin count and destroy coin
        sequence.OnComplete(() =>
        {
            coinCount++;
            UIManager.instance.DisplayCoins(coinCount);

            // Optional: animate the UI coin icon
            coinTargetUI.DOPunchScale(Vector3.one * 0.2f, 0.2f, 5, 0.5f);

            coin.SetActive(false);
            objectPooler.ReturnObject(coin);
        });
    }


    IEnumerator DeathAnimation()
    {
        Vector2 velocity = rb.velocity;
        velocity.y = Mathf.Min(0, velocity.y); // If going up, zero it; if falling, keep falling
        rb.velocity = velocity;
        Vector2 _force = new Vector2(0, upwardForce);
        rb.AddForce(_force,ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        col.isTrigger = true;
        StartCoroutine(RotateObject( rotationAngle, rotationSpeed));
        yield return new WaitForSeconds(5f);
        if(!revived)
        rb.gravityScale = 0;
    }

    IEnumerator RotateObject(float targetAngle, float speed)
    {
        float initialAngle = rb.rotation;
        float elapsedTime = 0f;
        float duration = 0.5f; // Adjust duration for smooth rotation

        while (elapsedTime < duration)
        {
            float angle = Mathf.Lerp(initialAngle, initialAngle + targetAngle, elapsedTime / duration);
            rb.rotation = angle;
            elapsedTime += Time.deltaTime * speed;
            yield return null;
        }

        rb.rotation = initialAngle + targetAngle;
    }

    public void Revived()
    {
        revived = true;
        rb.velocity = Vector2.zero;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        col.isTrigger = false;
        transform.position = new Vector3(startPos.x, startPos.y, transform.position.z);
        dead = false;
        rb.gravityScale = startGravityScale;
        rb.isKinematic = false;
       
    }

    bool ShieldActivated()
    {
        return playerController.ShieldActivated;
    }    

}
