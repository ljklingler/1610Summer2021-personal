using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
	private GameManager manager;
	private int pointValue = 1;
	private Transform rotatePivot;

	private void Start()
	{
		manager = GameObject.Find("GameManager").GetComponent<GameManager>();

		rotatePivot = transform.Find("Rotate Pivot");
	}

	private void Update()
	{
		rotatePivot.Rotate(Vector3.up, 180 * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			manager.UpdateScore(pointValue);
			Destroy(gameObject);
		}
	}
}
