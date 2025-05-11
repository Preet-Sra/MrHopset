using DG.Tweening;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float duration = 0.5f;
    [SerializeField] float strength = 0.3f;
    [SerializeField] int vibrato = 10;
    [SerializeField] float randomness = 90;

    private Vector3 originalPosition;

    private Tween shakeTween;
    [Header("Boost")]
    [SerializeField] float Boostduration = 0.5f;
    [SerializeField] float Booststrength = 0.3f;
    [SerializeField] int Boostvibrato = 10;
    [SerializeField] float Boostrandomness = 90;

    private void Awake()
    {
        originalPosition = transform.localPosition;
    }

    [ContextMenu("TestShake")]
    public void Shake()
    {
        // Kill any existing tweens to prevent overlap
        transform.DOKill();

        // Start shake and return to original position after
        transform.DOShakePosition(duration, strength, vibrato, randomness)
                 .OnComplete(() =>
                 {
                     transform.localPosition = originalPosition;
                 });
    }

    public void StartContinuousShake()
    {
        transform.DOKill(); // Stop any previous tween
        shakeTween = transform.DOShakePosition(Boostduration, Booststrength, Boostvibrato, Boostrandomness)
                              .SetLoops(-1, LoopType.Restart)
                              .SetEase(Ease.Linear);
    }

    public void StopContinuousShake()
    {
        if (shakeTween != null && shakeTween.IsActive())
        {
            shakeTween.Kill();
            transform.localPosition = originalPosition;
        }
    }
}
