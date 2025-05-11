using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSkin : MonoBehaviour
{
    SpriteRenderer sr;
   [SerializeField] Sprite[] AllSprites;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ShowActiveSkin();
    }

    void ShowActiveSkin()
    {
         int selectedSkinIndex = PlayerPrefs.GetInt("SelectedMap", 0);
        sr.sprite = AllSprites[selectedSkinIndex];

    }

}
