using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

	public Text resultScoreText;


	public float speedCalculating = 0.4f;
	private int scoreCalculating;
	private float eclapsedTime;
	private GameState gameState;


	private void Start()
	{
		gameState = FindObjectOfType<GameState>();
	}
	private void Update()
	{
		if (gameState.isWin)
		{
			ShowResultScore();
		}
		
	}

	
	void ShowResultScore()
	{
		
		int maximum = gameState.score;

		eclapsedTime +=  Time.deltaTime * speedCalculating;
		scoreCalculating = (int)Mathf.Lerp(0, maximum, eclapsedTime);
		resultScoreText.text = scoreCalculating.ToString() + "m";

		

	}
}
