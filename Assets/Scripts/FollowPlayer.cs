using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
	private SpawnManager spawnManager;

	public GameObject player;
	private Vector3 offset => new Vector3(0, 1, -3.75f);
	private Vector3 last;

    void Start()
    {
		spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

	private void LateUpdate()
	{
		if (spawnManager.playerAlive == true)
		{
			//Rotate camera with mouse
			//TODO: Clamp mouse, hide mouse
			last = player.transform.position;
			float mouseX = Input.GetAxis("Mouse X");
			transform.position = last;
			transform.Translate(offset);
			transform.RotateAround(last, player.transform.up, mouseX * 4f);
		}
		else
		{
			transform.RotateAround(last, Vector3.up, 4f * Time.deltaTime);
		}
	}
}
