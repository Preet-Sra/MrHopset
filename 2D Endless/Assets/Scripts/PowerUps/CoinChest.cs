using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CoinChest : MonoBehaviour
{
    bool alreadyCollided;
    AudioSource _audio;

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
    void OnEnable()
    {
        _audio = GetComponent<AudioSource>();
        alreadyCollided = false;
    }

    void OnDisable()
    {
        alreadyCollided = false;
        transform.localScale = Vector3.one;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (alreadyCollided) return;
        if (!collision.CompareTag("Player")) return;
        transform.DOKill();
        SoundManager.instance.PlayAudio(_audio);
        alreadyCollided = true;
        transform.SetParent(null);
        // Target world position of coin UI holder
        Vector3 uiWorldPos = UIManager.instance.coinsHolder.position;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(uiWorldPos);
        screenPos.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 worldTarget = Camera.main.ScreenToWorldPoint(screenPos);
        worldTarget.z = transform.position.z;

        Vector3 screenCenter = new Vector3(8, 0, transform.position.z);
      
        Sequence seq = DOTween.Sequence();

        
        seq.Append(transform.DOMove(screenCenter, 0.4f).SetEase(Ease.OutCubic));
        seq.Join(transform.DOScale(Vector3.one * 3f, 0.4f).SetEase(Ease.OutBack));

        
        seq.AppendInterval(0.5f);

        // Step 3: Move to UI + rotate + scale down
        seq.AppendCallback(() => UIManager.instance.AddCoins(20));
        seq.Append(transform.DOMove(worldTarget, 0.3f).SetEase(Ease.InCubic));
        seq.Join(transform.DORotate(new Vector3(0, 0, 360), 0.3f, RotateMode.FastBeyond360));
        seq.Join(transform.DOScale(Vector3.zero, 0.3f));

        // Step 4: Disable on complete
        seq.OnComplete(() => ObjectPooler.instance.ReturnObject(this.gameObject));
    }


    

}

