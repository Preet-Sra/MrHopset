using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderBoard : MonoBehaviour
{
    [System.Serializable]
    public class PlayerData
    {
        public string playerName;
        public int score;
    }

    [Header("Leaderboard UI")]
    [SerializeField] GameObject nameItemPrefab;
    [SerializeField] Transform contentParent;
    [SerializeField] int numberOfItemsToShow = 5;

    [Header("Fake Players (Editable in Inspector)")]
    [SerializeField] List<PlayerData> fakeData = new List<PlayerData>();

    void OnEnable()
    {
        GenerateLeaderboard();
    }

    void GenerateLeaderboard()
    {
        string currentName = PlayerPrefs.GetString("UserName") + " (You)";
        int currentScore = (int)PlayerPrefs.GetFloat("HighScore", 0);

        // Clone the list so editor data is not modified
        List<PlayerData> tempList = new List<PlayerData>(fakeData);

        // Remove old entries of current player to avoid duplicates
        tempList.RemoveAll(p => p.playerName == currentName);

        // Add current player
        tempList.Add(new PlayerData { playerName = currentName, score = currentScore });

        // Sort by score descending
        tempList.Sort((a, b) => b.score.CompareTo(a.score));

        int playerRank = tempList.FindIndex(p => p.playerName == currentName && p.score == currentScore) + 1;

        List<PlayerData> displayList = new List<PlayerData>();

        if (playerRank <= numberOfItemsToShow)
        {
            displayList = tempList.GetRange(0, Mathf.Min(numberOfItemsToShow, tempList.Count));
        }
        else
        {
            displayList = tempList.GetRange(0, numberOfItemsToShow - 1);
            displayList.Add(new PlayerData { playerName = currentName, score = currentScore });
        }

        // Clear existing UI items
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Spawn leaderboard entries
        for (int i = 0; i < displayList.Count; i++)
        {
            GameObject go = Instantiate(nameItemPrefab, contentParent);
            IndividualItemList itemlist = go.GetComponent<IndividualItemList>();
            int actualRank = (displayList[i].playerName == currentName && displayList[i].score == currentScore) ? playerRank : i + 1;

            itemlist.SetName(actualRank.ToString(), displayList[i].playerName, displayList[i].score.ToString());
        }
    }

}
