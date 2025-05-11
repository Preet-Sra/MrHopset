using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleToCameraHeight : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateScale();
    }

    void UpdateScale()
    {
        float screenHeight = Camera.main.orthographicSize * 2f;
        float height = spriteRenderer.sprite.bounds.size.y;
        float scaleFactor = screenHeight / height;

        transform.localScale = new Vector3(1f, scaleFactor, 1f);
    }
}
