using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

	public enum BossState {Idle, Move, MeleeAttack, RangedAttack, Tired}
	public GameObject projectile;

	int bossHealth;
	private PlayerController playerScript;
	private GameObject player;
	Vector2 playerPos;
	int projectiles;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		MeleeAttack ();
	}

	void OnCollisionEnter2D(Collision2D col)
	{

	}


	void MeleeAttack()
	{
		playerPos = player.transform.position;
	}

	void RangedAttack()
	{

	}
}
