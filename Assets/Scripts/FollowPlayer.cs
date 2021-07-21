using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
	public GameObject player;
	private float rotation;
	private Vector3 offset => new Vector3(0, 1, -3.75f);

    // Start is called before the first frame update
    void Start()
    {
		rotation = transform.rotation.eulerAngles.y;
    }

	private void LateUpdate()
	{
		Vector3 playerPos = player.transform.position;
		float mouseX = Input.GetAxis("Mouse X");
		rotation += mouseX * 1.5f;
		//transform.rotation = Quaternion.Euler(30, rotation, 0);
		transform.position = playerPos;
		transform.Translate(offset);
		transform.RotateAround(playerPos, player.transform.up, mouseX * 4f);
		//transform.position = new Vector3(playerPos.x, playerPos.y + 3, playerPos.z - 5.5f);


		//transform.RotateAround(playerPos, Vector3.up, mouseX);
	}
}
