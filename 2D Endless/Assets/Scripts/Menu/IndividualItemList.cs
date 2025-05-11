
using UnityEngine;
using TMPro;

public class IndividualItemList : MonoBehaviour
{
    [SerializeField] TMP_Text rank, Playername, Playerscore;
    public void SetName(string _rank, string _name, string _score)
    {
        rank.text = _rank + ".";
        Playername.text = _name;
        Playerscore.text = _score;
    }
}
