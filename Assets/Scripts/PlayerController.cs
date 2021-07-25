using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed = 40;
	private Rigidbody rigidbody;

	private Camera camera;

	//Deceleration and damping variables, for SmoothDamp
	private Vector3 dampingHz;
	private Vector3 dampingVt;
	private float deceleration = 0.1f; //time in seconds to come to a stop

    // Start is called before the first frame update
    void Start()
    {
		rigidbody = GetComponent<Rigidbody>();
		camera = Camera.main;
    }

	private void FixedUpdate()
	{
		//Get input, and the total movement vector
		float horizontalInput = Input.GetAxisRaw("Horizontal");
		float verticalInput = Input.GetAxisRaw("Vertical");
		Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);

		//Rotate to face the direction of the view, and make movement vector relative to view
		Vector3 camPos = new Vector3(camera.transform.position.x, transform.position.y, camera.transform.position.z);
		Vector3 diff = camPos - transform.position;
		transform.forward = -diff;
		Vector3 move = transform.TransformDirection(movement);

		//Add final force to movement
		rigidbody.AddForce(move * speed * 100);

		//Arrest movement in direction the player isn't trying to move
		if (horizontalInput == 0)
		{
			Vector3 velocity = rigidbody.velocity;
			rigidbody.velocity = Vector3.SmoothDamp(velocity, Vector3.Project(velocity, transform.forward), ref dampingHz, deceleration);
		}
		if (verticalInput == 0)
		{
			Vector3 velocity = rigidbody.velocity;
			rigidbody.velocity = Vector3.SmoothDamp(velocity, Vector3.Project(velocity, transform.right), ref dampingVt, deceleration);
		}
	}

	private void LateUpdate()
	{
		//Rotate camera with mouse
		//TODO: Clamp mouse, hide mouse
		float mouseX = Input.GetAxis("Mouse X");
		camera.transform.position = transform.position;
		camera.transform.Translate(new Vector3(0, 1, -3.75f));
		camera.transform.RotateAround(transform.position, transform.up, mouseX * 4f);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Monster"))
		{
			Debug.Log("Death");
			Destroy(gameObject);
		}
	}
}
