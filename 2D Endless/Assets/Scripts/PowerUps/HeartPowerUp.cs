
using UnityEngine;
using DG.Tweening;

public class HeartPowerUp : MonoBehaviour
{
    [SerializeField] AudioSource pickupSound;
    bool alreadyCollected;

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
        if (collision.CompareTag("Player") && !alreadyCollected)
        {
            alreadyCollected = true;
            GameObject _vfx= ObjectPooler.instance.GetObject("HeartPickup");
            _vfx.transform.position = transform.position;
            _vfx.SetActive(true);

            Animation();
            collision.GetComponent<PlayerHealth>().AddHealth();
            SoundManager.instance.PlayAudio(pickupSound);
           
        }
    }

    void Animation()
    {

        transform.DOKill();

        Vector3 screenCenter = new Vector3(8, 0, transform.position.z);
        transform.SetParent(null);
        Sequence seq = DOTween.Sequence();


        seq.Append(transform.DOMove(screenCenter, 0.4f).SetEase(Ease.OutCubic));
        seq.Join(transform.DOScale(Vector3.one * 3f, 0.4f).SetEase(Ease.OutBack));


        seq.AppendInterval(0.5f);

        // Step 3: Move to UI + rotate + scale down
        seq.AppendCallback(() => UIManager.instance.AddCoins(20));
        
       

        // Step 4: Disable on complete
        seq.OnComplete(() =>
        {
            gameObject.SetActive(false);
            ObjectPooler.instance.ReturnObject(this.gameObject);

        });
    }

    private void OnDisable()
    {
        alreadyCollected = false;
        transform.localScale = Vector3.one;
    }
}
