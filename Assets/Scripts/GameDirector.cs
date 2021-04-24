using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [SerializeField] TMP_Text coinText;
    int coinAmount = 0;

    private void Awake()
    {
        AddCoin(10);
    }

    internal void AddCoin(int amount)
    {
        coinAmount += amount;
        coinText.text = $"x{coinAmount}";
    }
}
