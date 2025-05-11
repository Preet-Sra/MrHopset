
using UnityEngine;
using DG.Tweening;

public class Bomb : MonoBehaviour
{
    [SerializeField] float rotationDuration = 2f;

    void Start()
    {
        transform.DORotate(new Vector3(0, 0, 360), rotationDuration, RotateMode.FastBeyond360)
                 .SetEase(Ease.Linear)
                 .SetLoops(-1, LoopType.Restart);
    }
}
