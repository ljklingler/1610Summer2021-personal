using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed = 40;
	private Rigidbody rigidbody;

	private float walkX;
	private float walkZ;
	public float deceleration = 5f;

    // Start is called before the first frame update
    void Start()
    {
		rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
		
    }

	private void FixedUpdate()
	{
		float horizontalInput = Input.GetAxisRaw("Horizontal");
		float verticalInput = Input.GetAxisRaw("Vertical");

		rigidbody.AddForce(new Vector3(horizontalInput, 0, verticalInput) * speed * 100, ForceMode.Force);

		//Arrest movement in direction the player isn't trying to move
		if (horizontalInput == 0)
		{
			rigidbody.velocity = new Vector3(Mathf.SmoothDamp(rigidbody.velocity.x, 0, ref walkX, deceleration), rigidbody.velocity.y, rigidbody.velocity.z);
		}
		if (verticalInput == 0)
		{
			rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, Mathf.SmoothDamp(rigidbody.velocity.z, 0, ref walkZ, deceleration));
		}
	}
}
