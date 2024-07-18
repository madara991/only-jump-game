using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class powerJump : MonoBehaviour
{
	[Range(0f, 50f)]
	public int ThrowingPower = 8;
	[Range(5f, 40f)]
	public float JumpPower = 5f;

	public ParticleSystem jumpSmokeEffect;
	private void Start()
	{
		GetComponentInChildren<Text>().text = ThrowingPower.ToString() + "x" + "\n" + "Jump";
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			other.GetComponent<Player>().JumpFromPlatform(ThrowingPower, JumpPower);

			Instantiate(jumpSmokeEffect,transform.position,Quaternion.identity);
			
		}
	}

}
