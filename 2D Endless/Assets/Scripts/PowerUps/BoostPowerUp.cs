using DG.Tweening;
using UnityEngine;

public class BoostPowerUp : MonoBehaviour
{

    [SerializeField] AudioSource pickupSound;

    private void Start()
    {
        transform
       .DOMoveY(transform.position.y + 0.3f, 0.5f)
       .SetEase(Ease.InOutSine)
       .SetLoops(-1, LoopType.Yoyo);

        transform
            .DOScale(1.2f, 0.5f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.instance.PlayAudio(pickupSound);
        }
    }
}
