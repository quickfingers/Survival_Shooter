using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6f;

	private Vector3 movement;
	private Animator animator;
	private Rigidbody playerRigidBody;
	private int floorMask;
	private float camRayLength=100f;

	void Awake()
	{
		floorMask = LayerMask.GetMask ("Floor");
		animator = GetComponent<Animator> ();
		playerRigidBody = GetComponent<Rigidbody> ();
	}

	// run every physics update
	void FixedUpdate()
	{
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		Move (h, v);
		Turning ();
		Animating (h, v);
	}

	void Move(float h, float v)
	{
		movement.Set (h, 0f, v);
		// this is for diagonal movement it needs to be 1.
		movement = movement.normalized * speed * Time.deltaTime;
		playerRigidBody.MovePosition (transform.position + movement);

	}

	void Turning()
	{
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit floorHit;
		if(Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
		{
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;

			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
			playerRigidBody.MoveRotation(newRotation);
		}
	}

	void Animating(float h, float v)
	{
		// are we pressing any key?
		bool walking = h != 0f || v != 0f;
		animator.SetBool ("IsWalking", walking);
	}

}
