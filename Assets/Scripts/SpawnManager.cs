using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public GameObject spawnPoint;
	public GameObject enemyPrefab;

	public bool playerAlive;

	private void Start()
	{
		playerAlive = true;

		Instantiate(enemyPrefab, spawnPoint.transform.position, enemyPrefab.transform.rotation);
	}
}
