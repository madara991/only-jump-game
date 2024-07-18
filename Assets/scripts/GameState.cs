using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GameState : MonoBehaviour
{

	public int coins;
	public int score;
    public bool isWin;
	private bool isArrive;

	public GameObject CoinDestroyedEffect;

	public CinemachineVirtualCamera riseEndCamera;

	public Transform endPoint;
	private Player _player;

	public UnityEvent OnArraiveToEnd;
	




	

	//  Added as listener to UnityEvent 
	public void Winner()
	{
		isWin = true;	    
	}

	public void SetWinnerScore(int _winnerScore)
	{
		score = _winnerScore;
		Debug.Log("score: " + score);
	}
	public  void InstantiateObject(Vector3 position)
    {
        Instantiate(CoinDestroyedEffect, position, Quaternion.identity);
    }



	void Start()
	{
		_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		riseEndCamera.Priority = 2; // origin value
	}

	private void Update()
	{
		float distance = Mathf.Abs(_player.transform.position.z - endPoint.position.z);
		if (distance <= 20f )
		{
			OnArraiveToEnd?.Invoke();
		}
	}
	public void SwitchToEndCamera()
	{
		riseEndCamera.Priority += 1;
	}

	public void StopControlerState()
	{
		_player.isStopControler = true;
	}
	public void ActiveDance()
	{

		_player.ActiveWinnerDance();
	}
	
}
