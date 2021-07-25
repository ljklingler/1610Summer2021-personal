using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
	NavMeshAgent agent;
	private float speed;
	public Transform target;
	public GameObject player;

	public float normalSightArc;
	private float sightArc;
	public Vector3 headPosition;

	private bool aggressive = false;
	private float timeStopped;

    // Start is called before the first frame update
    void Start()
    {
		agent = GetComponent<NavMeshAgent>();
		sightArc = AdjustedSightArc();
		timeStopped = 0f;

		StartCoroutine(SpotPlayer());
		StartCoroutine(Move());
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
		{
			agent.destination = player.transform.position;
		}
	}

	float AdjustedSightArc()
	{
		float s = aggressive ? normalSightArc + 40 : normalSightArc; //wider while aggro
		s *= aggressive && timeStopped > 0 ? 1.5f : 1f; //slightly wider if standing still
		return s;
	}

	float AdjustedSpeed()
	{
		return aggressive ? 5f : 3.5f;
	}

	IEnumerator Move()
	{
		while (true)
		{
			agent.speed = AdjustedSpeed();

			if (!aggressive)
			{
				//patrol
			}
			else
			{
				//suspend patrol
			}

			yield return null;
		}
	}

	IEnumerator SpotPlayer()
	{
		Collider collider = player.GetComponent<Collider>();
		float debugDrawTime = 0.01f;

		while (true)
		{
			//Get important positions
			Vector3 origin = transform.position + headPosition;
			Vector3 closest = collider.ClosestPoint(origin);

			//Adjust sight arc (based on aggression, movement)
			sightArc = AdjustedSightArc();
			
			//Debug draw sight cone
			Debug.DrawRay(origin, transform.forward * 4f, Color.red, debugDrawTime);
			Vector3 left = Quaternion.Euler(0, -sightArc / 2, 0) * transform.forward;
			Debug.DrawRay(origin, left * 40f, Color.grey, debugDrawTime);
			Vector3 right = Quaternion.Euler(0, sightArc / 2, 0) * transform.forward;
			Debug.DrawRay(origin, right * 40f, Color.grey, debugDrawTime);

			//Is there a clear line between the head and the target?
			if (Physics.Linecast(origin, closest, out RaycastHit hit, LayerMask.NameToLayer("Player"), QueryTriggerInteraction.Ignore))
			{
				//Nope, blocked.
				Debug.DrawLine(origin, hit.point, Color.blue, debugDrawTime);
				Debug.DrawLine(hit.point, closest, Color.red, debugDrawTime);
			}
			else
			{
				//Clear line.
				//Get direction to target
				Vector3 directionToTarget = (closest - origin).normalized;
				float diff = Vector3.Angle(transform.forward, directionToTarget);

				//Is the target within sight cone?
				if (diff < sightArc / 2)
				{
					//Target is visible. Become aggressive, move to target.
					Debug.DrawLine(origin, closest, Color.green, debugDrawTime);
					aggressive = true;
					agent.destination = player.transform.position;
				}
				else
				{
					//Target isn't visible.
					Debug.DrawLine(origin, closest, Color.yellow, debugDrawTime);
				}
			}

			//Increment timeStopped while stationary, else reset it.
			if (agent.remainingDistance < 0.05f)
			{
				timeStopped += Time.deltaTime;
			}
			else
			{
				timeStopped = 0;
			}

			//If aggro and doesn't see anyone for a while, stop being aggro.
			if (aggressive && timeStopped > 3f)
			{
				aggressive = false;
			}

			yield return null;
		}
	}
}
