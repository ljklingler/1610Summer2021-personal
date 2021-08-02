using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class GameManager : MonoBehaviour
{
	private GameObject player;
	private TextMeshProUGUI scoreText;
	private TextMeshProUGUI seenText;
	public GameObject spawnPoint;
	public GameObject enemyPrefab;
	public GameObject pickupPrefab;

	private GameObject activePickup;

	public bool isPlayerAlive;
	public bool isAnyMonsterAggressive;
	public bool isEscapePossible => score >= scoreToEndRound;

	private int score = 0;
	private int scoreToEndRound;
	private float timeElapsed = 0f;

	private void Start()
	{
		player = GameObject.Find("Player").gameObject;
		scoreText = GameObject.Find("Score Text").GetComponent<TextMeshProUGUI>();
		seenText = GameObject.Find("Seen Text").GetComponent<TextMeshProUGUI>();
		StartGame();

	}

	private void Update()
	{
		if (isPlayerAlive)
		{
			timeElapsed += Time.deltaTime;
			if (timeElapsed % 5 == 0)
			{
				SpawnMonster();
			}

			if (!isEscapePossible)
			{
				if (seenText.gameObject.activeInHierarchy != isAnyMonsterAggressive)
				{
					seenText.gameObject.SetActive(isAnyMonsterAggressive);
				}
			} else
			{
				seenText.gameObject.SetActive(true);
				seenText.text = "Get to the exit!";
			}
		}


	}

	private void StartGame()
	{
		isPlayerAlive = true;
		scoreToEndRound = Random.Range(4, 10);
		UpdateScore(0);
		SpawnMonster();
	}

	private void SpawnMonster()
	{
		Instantiate(enemyPrefab, spawnPoint.transform.position, enemyPrefab.transform.rotation);
	}

	public void UpdateScore(int add)
	{
		score += add;
		float scoreToWrite = (float)System.Math.Round(score * (7.25f / scoreToEndRound), 2);
		Debug.Log("Score: " + score);
		scoreText.text = "$" + scoreToWrite.ToString("0.00");
		if (!isEscapePossible)
		{
			activePickup = Instantiate(pickupPrefab, RandomPosition(), pickupPrefab.transform.rotation);
		}
		
	}

	private Vector3 RandomPosition()
	{
		for (int i = 0; i < 10; i++)
		{
			var randDir = Random.insideUnitSphere;
			var randPos = randDir.normalized * Random.Range(0f, 40f);
			Vector3 pos = new Vector3(randPos.x, transform.position.y, randPos.y);

			if (NavMesh.SamplePosition(pos, out NavMeshHit hit, pos.magnitude, NavMesh.AllAreas))
			{
				if (Vector3.Distance(hit.position, player.transform.position) < 3f)
					continue;
				return hit.position;
			}
		}
		return Vector3.zero;
	}
}
