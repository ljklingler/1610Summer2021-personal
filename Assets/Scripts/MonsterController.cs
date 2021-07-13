using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
	NavMeshAgent agent;
	public Transform target;
	public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
		agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
		{
			agent.destination = player.transform.position;
		}
    }

	//This is all temporary and gross but it's late and I don't want to move it right now
	//TODO or whatever
	private void OnGUI()
	{
		float f = 0.01f;
		float halfAngle = 40f;

		//Set the 'head' position, and draw the forward
		Vector3 origin = transform.position + Vector3.up * 2;
		Debug.DrawRay(origin, transform.forward * 4f, Color.red, f);

		//Get closest point on player's collider
		Collider collider = player.GetComponent<Collider>();
		Vector3 closest = collider.ClosestPoint(origin);
		RaycastHit hit;
		
		//Check if the sight line to the player is clear
		if (Physics.Linecast(origin, closest, out hit, LayerMask.NameToLayer("Player"), QueryTriggerInteraction.Ignore))
		{
			//Show where the obstacle is
			Debug.DrawLine(origin, hit.point, Color.blue, f);
			Debug.DrawLine(hit.point, closest, Color.red, f);
		}
		else
		{
			//Get direction to the player, and the angle of difference
			Vector3 dirToPlayer = (closest - origin).normalized;
			float diff = Vector3.Angle(transform.forward, dirToPlayer);

			//Check if angle is in view or not
			if (diff < halfAngle)
			{
				//It's in view! Move to that position
				Debug.DrawLine(origin, closest, Color.green, f);
				agent.destination = player.transform.position;

				//increase sight cone while traveling
				//further increase it if arrive at position and they haven't hit the player
				//also increase speed
			}
			else
			{
				//It's not in view, so the player is safe
				Debug.DrawLine(origin, closest, Color.yellow, f);

				//Continue along 'patrol'
			}
		}
	}
}
