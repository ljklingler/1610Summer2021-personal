using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
	public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		
    }

	private void LateUpdate()
	{
		Vector3 playerPos = player.transform.position;
		transform.position = new Vector3(playerPos.x, playerPos.y + 3, playerPos.z - 5.5f);
	}
}
