using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    SoundManager soundManager;
    [SerializeField] GameObject ShopPanel;
    [SerializeField] GameObject PlayerSkinsPanel, MapPanel;
    [SerializeField] TMP_Text PlayerPanelSwitchText, MapPanelSwitchText;
    [SerializeField] Color selectedColor, unSelectedColor;

    [Header("Player Skins")]
    [SerializeField] Image PlayerIcon;
    [SerializeField] Button playerBuyButton;
    [SerializeField] TMP_Text playername;
    [SerializeField] TMP_Text buttonStatus;
    [SerializeField] List<PlayerData> allPlayers;

    int playerCurrentIndex;

    const string playerSelectedKey = "SelectedPlayer";
    const string playerBoughtKey = "PlayerBought_";

    [Header("Map Skins")]
    [SerializeField] Image MapIcon;
    [SerializeField] Button mapBuyButton;
    [SerializeField] TMP_Text mapButtonStatus;
    [SerializeField] List<MapData> allMaps;

    int mapCurrentIndex;

    const string mapSelectedKey = "SelectedMap";
    const string mapBoughtKey = "MapBought_";

    private void Start()
    {
        soundManager = SoundManager.instance;

        // Initialize player data
        if (!PlayerPrefs.HasKey(playerSelectedKey))
            PlayerPrefs.SetInt(playerSelectedKey, 0);

        for (int i = 0; i < allPlayers.Count; i++)
        {
            if (!PlayerPrefs.HasKey(playerBoughtKey + i))
                PlayerPrefs.SetInt(playerBoughtKey + i, i == 0 ? 1 : 0);
        }

        playerCurrentIndex = PlayerPrefs.GetInt(playerSelectedKey);
        UpdatePlayerUI();

        // Initialize map data
        if (!PlayerPrefs.HasKey(mapSelectedKey))
            PlayerPrefs.SetInt(mapSelectedKey, 0);

        for (int i = 0; i < allMaps.Count; i++)
        {
            if (!PlayerPrefs.HasKey(mapBoughtKey + i))
                PlayerPrefs.SetInt(mapBoughtKey + i, i == 0 ? 1 : 0);
        }

        mapCurrentIndex = PlayerPrefs.GetInt(mapSelectedKey);
        UpdateMapUI();
    }

    public void OpenShopMenu(bool _state)
    {
        soundManager.PlayUiButtonSound();
        ShopPanel.SetActive(_state);
        if (_state)
        {
            playerCurrentIndex = PlayerPrefs.GetInt(playerSelectedKey);
            mapCurrentIndex = PlayerPrefs.GetInt(mapSelectedKey);
            UpdatePlayerUI();
            UpdateMapUI();
        }
    }

    public void ShowPlayerSkinPanel()
    {
        soundManager.PlayUiButtonSound();
        PlayerSkinsPanel.SetActive(true);
        MapPanel.SetActive(false);
        PlayerPanelSwitchText.color = selectedColor;
        MapPanelSwitchText.color = unSelectedColor;
    }

    public void ShowMapSkinPanel()
    {
        soundManager.PlayUiButtonSound();
        PlayerSkinsPanel.SetActive(false);
        MapPanel.SetActive(true);
        PlayerPanelSwitchText.color = unSelectedColor;
        MapPanelSwitchText.color = selectedColor;
    }

    public void Back()
    {
        OpenShopMenu(false);
        ShowPlayerSkinPanel();
    }

    // ---------- PLAYER SKIN LOGIC ----------
    public void ShowNextPlayer()
    {
        soundManager.PlayUiButtonSound();
        playerCurrentIndex = (playerCurrentIndex + 1) % allPlayers.Count;
        UpdatePlayerUI();
    }

    public void ShowPreviousPlayer()
    {
        soundManager.PlayUiButtonSound();
        playerCurrentIndex = (playerCurrentIndex - 1 + allPlayers.Count) % allPlayers.Count;
        UpdatePlayerUI();
    }

    public void BuyPlayer()
    {
        soundManager.PlayUiButtonSound();
        int coins = PlayerPrefs.GetInt("Coins");
        PlayerData current = allPlayers[playerCurrentIndex];

        if (PlayerPrefs.GetInt(playerBoughtKey + playerCurrentIndex) == 1)
        {
            PlayerPrefs.SetInt(playerSelectedKey, playerCurrentIndex);
        }
        else
        {
            if (coins >= current._price)
            {
                coins -= current._price;
                PlayerPrefs.SetInt("Coins", coins);
                PlayerPrefs.SetInt(playerBoughtKey + playerCurrentIndex, 1);
                PlayerPrefs.SetInt(playerSelectedKey, playerCurrentIndex);
                IAPManager.instance.ShowCoins();
            }
            else
            {
                //Debug.Log("Not enough coins!");
                IAPManager.instance.ShowInAppWindow(true);

            }
        }

        UpdatePlayerUI();
    }

    void UpdatePlayerUI()
    {
        PlayerData current = allPlayers[playerCurrentIndex];
        PlayerIcon.sprite = current._icon;
        playername.text = current._name;

        bool isBought = PlayerPrefs.GetInt(playerBoughtKey + playerCurrentIndex) == 1;
        bool isSelected = PlayerPrefs.GetInt(playerSelectedKey) == playerCurrentIndex;

        if (isSelected)
        {
            buttonStatus.text = "Using...";
            playerBuyButton.interactable = false;
        }
        else if (isBought)
        {
            buttonStatus.text = "Use";
            playerBuyButton.interactable = true;
        }
        else
        {
            buttonStatus.text = current._price + " <sprite=0>";
            playerBuyButton.interactable = true;
        }
    }

    // ---------- MAP SKIN LOGIC ----------
    public void ShowNextMap()
    {
        soundManager.PlayUiButtonSound();
        mapCurrentIndex = (mapCurrentIndex + 1) % allMaps.Count;
        UpdateMapUI();
    }

    public void ShowPreviousMap()
    {
        soundManager.PlayUiButtonSound();
        mapCurrentIndex = (mapCurrentIndex - 1 + allMaps.Count) % allMaps.Count;
        UpdateMapUI();
    }

    public void BuyMap()
    {
        soundManager.PlayUiButtonSound();
        int coins = PlayerPrefs.GetInt("Coins");
        MapData current = allMaps[mapCurrentIndex];

        if (PlayerPrefs.GetInt(mapBoughtKey + mapCurrentIndex) == 1)
        {
            PlayerPrefs.SetInt(mapSelectedKey, mapCurrentIndex);
        }
        else
        {
            if (coins >= current._price)
            {
                coins -= current._price;
                PlayerPrefs.SetInt("Coins", coins);
                PlayerPrefs.SetInt(mapBoughtKey + mapCurrentIndex, 1);
                PlayerPrefs.SetInt(mapSelectedKey, mapCurrentIndex);
                IAPManager.instance.ShowCoins();
            }
            else
            {
                // Debug.Log("Not enough coins!");
                IAPManager.instance.ShowInAppWindow(true);
            }
        }

        UpdateMapUI();
    }

    void UpdateMapUI()
    {
        MapData current = allMaps[mapCurrentIndex];
        MapIcon.sprite = current._icon;

        bool isBought = PlayerPrefs.GetInt(mapBoughtKey + mapCurrentIndex) == 1;
        bool isSelected = PlayerPrefs.GetInt(mapSelectedKey) == mapCurrentIndex;

        if (isSelected)
        {
            mapButtonStatus.text = "Using...";
            mapBuyButton.interactable = false;
        }
        else if (isBought)
        {
            mapButtonStatus.text = "Use";
            mapBuyButton.interactable = true;
        }
        else
        {
            mapButtonStatus.text = current._price + " <sprite=0>";
            mapBuyButton.interactable = true;
        }
    }
}

[System.Serializable]
public class PlayerData
{
    public string _name;
    public int _price;
    public Sprite _icon;
}

[System.Serializable]
public class MapData
{
    public int _price;
    public Sprite _icon;
}
