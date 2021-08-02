using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class MonsterController : MonoBehaviour
{
	NavMeshAgent agent;
	private float speed;
	public GameObject player;
	private GameManager manager;

	public float normalSightArc;
	private float sightArc;
	public Vector3 headPosition;

	private bool aggressive = false;
	private float timeStopped;
	private float timeAggro;


    // Start is called before the first frame update
    void Start()
    {
		agent = GetComponent<NavMeshAgent>();
		player = GameObject.Find("Player");
		manager = GameObject.Find("GameManager").GetComponent<GameManager>();
		sightArc = AdjustedSightArc();
		timeStopped = 0f;
		timeAggro = 0f;

		StartCoroutine(SpotPlayer());
		StartCoroutine(Move());
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
		{
			for (int i = 0; i < 15; i++)
			{
				RandomWalkPoint(out Vector3 point);
			}
		}
	}

	float AdjustedSightArc()
	{
		float s = aggressive ? normalSightArc + 40 : normalSightArc; //wider while aggro
		s *= aggressive && timeStopped > 0 ? 2f : 1f; //slightly wider if standing still
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
			manager.isAnyMonsterAggressive = aggressive;
			agent.speed = AdjustedSpeed();
			Debug.DrawLine(transform.position, agent.destination, Color.red, 0.01f);

			if (!aggressive)
			{
				
				//patrol
				if (timeStopped > 3f && RandomWalkPoint(out Vector3 point))
				{
					agent.destination = point;
				}
			}
			else
			{
				//suspend patrol

				//Get faster over time to a cap
				timeAggro += Time.deltaTime;
				agent.speed += Mathf.Min(Mathf.Max(timeAggro - 3f, 0f), 6f);
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
			Vector3 closest = collider != null ? collider.ClosestPoint(origin) : Vector3.zero;

			//Adjust sight arc (based on aggression, movement)
			sightArc = AdjustedSightArc();
			
			//Debug draw sight cone
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
				timeAggro = 0;
			}

			yield return null;
		}
	}

	bool RandomWalkPoint(out Vector3 result)
	{
		for (int i = 0; i < 10; i++)
		{
			var randDir = Random.insideUnitSphere;
			var pointWithinRing = randDir.normalized * 3f + randDir * (40f - 3f);
			Vector3 pos = new Vector3(transform.position.x + pointWithinRing.x, transform.position.y, transform.position.z + pointWithinRing.y);

			NavMeshHit hit;
			if (NavMesh.SamplePosition(pos, out hit, pos.magnitude, NavMesh.AllAreas))
			{
				Debug.DrawLine(transform.position, pos, Color.magenta, 2f);
				Debug.DrawLine(transform.position, hit.position, Color.cyan, 2f);
				result = hit.position;
				return true;
			}
		}
		result = Vector3.zero;
		return false;
	}
}
