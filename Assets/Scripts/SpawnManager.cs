using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public GameObject enemyPrefab;

	public bool playerAlive;

	private void Start()
	{
		playerAlive = true;

		Instantiate(enemyPrefab, Vector3.forward * 2.27f, enemyPrefab.transform.rotation);
	}
}
