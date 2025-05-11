
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    [SerializeField] float rotationDuration = 2f;
    private Tween rotationTween;
    Collider2D col;
    void OnEnable()
    {
        col = GetComponent<Collider2D>();
        col.enabled = true;
        rotationTween = transform.DORotate(new Vector3(0, 360, 0), rotationDuration, RotateMode.FastBeyond360)
                                 .SetEase(Ease.Linear)
                                 .SetLoops(-1, LoopType.Restart);
    }

   
    public void StopRotation()
    {
        if (rotationTween != null && rotationTween.IsActive())
        {
            rotationTween.Kill();
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnDisable()
    {
        StopRotation();
    }
}
