
using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    [SerializeField] GameObject[] AllSkins;

    private void Start()
    {
        ShowActiveSkin();
    }

    public GameObject GetActiveSkin()
    {
        foreach(GameObject _obj in AllSkins)
        {
            if (_obj.activeInHierarchy)
                return _obj;
            
        }
        return null;
    }

    void ShowActiveSkin()
    {
        foreach(GameObject _obj in AllSkins)
        {
            _obj.SetActive(false);
        }
        int selectedSkinIndex = PlayerPrefs.GetInt("SelectedPlayer", 0);
        AllSkins[selectedSkinIndex].SetActive(true);
    }
}
