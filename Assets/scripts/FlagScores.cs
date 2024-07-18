using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagScores : MonoBehaviour
{
    private int levelScore;

    public void OnTriggerEnter(Collider other)
    {
        levelScore = int.Parse(GetComponent<Text>().text.Replace("m",""));

        var gameState = FindObjectOfType<GameState>();
        if (gameState == null)
            return;

        //gameState.OnArraiveToEnd?.Invoke();
        gameState.SetWinnerScore(levelScore);
    }
}
