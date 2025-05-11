using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    SoundManager soundManager;
    float startXPos;
    float startPosY;
    [SerializeField] AudioSource jumpSound;
    [SerializeField] AudioSource doubleJumpSound;
    PlayerHealth playerHealth;
    [SerializeField] AudioSource landSound;
    UIManager uIManager;

    [Header("Jump Settings")]
    [SerializeField] float singleJumpForce = 12f, doubleJumpForce = 12f;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform playerSkinParent;
    [SerializeField] ParticleSystem landParticles;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool wasGrounded;
    private bool canDoubleJump;


    [Header("Powerups")]
    [HideInInspector]public bool ShieldActivated;
    [SerializeField] float ShieldTime;
    
    [SerializeField] Sprite ShieldIcon;
    [SerializeField] GameObject shieldVisual;

    [SerializeField] bool boostActivated;
    [SerializeField] bool invinsibleFromBoost;
    [SerializeField] float boostTime;
    [SerializeField] Sprite BoostIcon;
    [SerializeField] GameObject BoostVisual;
    

    void Start()
    {
        uIManager = UIManager.instance;
        playerHealth = GetComponent<PlayerHealth>();
        startXPos = transform.position.x;
        startPosY = transform.position.y;
        rb = GetComponent<Rigidbody2D>();
        soundManager = SoundManager.instance;
    }

    void Update()
    {
        FixXPosition();
        // Ground check
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Landing detection
        if (!wasGrounded && isGrounded)
        {
            soundManager.PlayAudio(landSound);
            landParticles.Clear();
            landParticles.Play();
        }

        if (isGrounded)
        {
            canDoubleJump = true;
        }

        if (Input.GetMouseButtonDown(0) && !IsTouchOverUI())
        {
            if (isGrounded)
            {
                Jump(false);
            }
            else if (canDoubleJump)
            {
                Jump(true);
                canDoubleJump = false;
            }
        }
    }

    bool IsTouchOverUI()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        }
#endif
        return EventSystem.current.IsPointerOverGameObject();
    }

    void FixXPosition()
    {
        transform.position = new Vector3(startXPos, transform.position.y, transform.position.z);
    }

    void Jump(bool isDoubleJump)
    {
        if (IsDead()) return;

        if (isDoubleJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);

            soundManager.PlayAudio(doubleJumpSound);
            playerSkinParent.DOKill();
            playerSkinParent.localRotation = Quaternion.identity;
            playerSkinParent.DORotate(new Vector3(0, 0, -360), 0.24f, RotateMode.FastBeyond360).SetEase(Ease.OutCubic);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, singleJumpForce);
            soundManager.PlayAudio(jumpSound);
          
        }
    }

    bool IsDead()
    {
        return playerHealth.dead;
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shield"))
        {
            ShieldActivated = true;
            shieldVisual.SetActive(true);
            collision.gameObject.SetActive(false);
            uIManager.ActivatePowerUp(true);
            StartCoroutine(DeactiveShield());
        }

        if (collision.CompareTag("Boost"))
        {
            boostActivated = true;
            BoostVisual.SetActive(true);
            invinsibleFromBoost = true;
            collision.gameObject.SetActive(false);
            uIManager.ActivatePowerUp(true);
            StartCoroutine(DeactiveBoost());

        }
    }


    IEnumerator DeactiveShield()
    {
        float timer = ShieldTime;
        
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            uIManager.ShowPowerUpTimer(ShieldIcon, ShieldTime, timer);
            yield return null;
        }
        ShieldActivated = false;
        shieldVisual.SetActive(false);
        uIManager.ActivatePowerUp(false);
    }

    IEnumerator DeactiveBoost()
    {
        float timer =boostTime;
        GameManager.instance.StartBoost();
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            uIManager.ShowPowerUpTimer(BoostIcon, boostTime, timer);
            yield return null;
        }
        boostActivated = false;
        BoostVisual.SetActive(false);
        uIManager.ActivatePowerUp(false);
        GameManager.instance.StopBoost();
        yield return new WaitForSeconds(0.5f);
        invinsibleFromBoost = false;
    }

    public bool IsInvisibleFromBoost()
    {
        return invinsibleFromBoost;
    }
}
