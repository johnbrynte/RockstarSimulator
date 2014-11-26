using UnityEngine;
using System.Collections;

public class CharacterControllerLogicOld : MonoBehaviour {
	
	[SerializeField]
	private Animator animator;
	[SerializeField]
	private float directionDampTime = 0.25f;
	[SerializeField]
	private ThirdPersonCamera gamecam;
	[SerializeField]
	private float directionSpeed = 3f;
	[SerializeField]
	private float rotationDegreePerSecond = 120f;
	[SerializeField]
	private float turnSmoothing = 3f;
	[SerializeField]
	private float jumpSpeed = 25000f;
	[SerializeField]
	private float climbSpeed = 5f;
	[SerializeField]
	private float groundDist = 0.05f;
	[SerializeField]
	private bool canControlInAir = true;



	
	private float speed = 0.0f;
	private float direction = 0.0f;
	private float horizontal = 0.0f;
	private float vertical = 0.0f;
	
	private Vector3 currentDirection = Vector3.zero;
	private Vector3 targetDirection = Vector3.zero;
	
	private AnimatorStateInfo stateInfo;
	
	private int m_LocomotionId = 0;

	public const int STATE_GROUND = 0;
	public const int STATE_AIR = 1;
	public const int STATE_CLIMB = 2;

	private int current_state;


	private bool grounded = true;
	private bool is_in_climb = false;

	
	public float Speed {
		get {
			return this.speed;
		}
	}
	
	public float LocomotionThreshold {
		get {
			return 0.2f;
		}
	}
	
	// Use this for initialization
	void Start () {
		// somehow initializes the animator...
		animator = GetComponent<Animator>();
		if (animator.layerCount >= 2) {
			animator.SetLayerWeight (1, 1);
		}
		
		m_LocomotionId = Animator.StringToHash("Base Layer.Locomotion");
		current_state = STATE_GROUND;

	}
	
	// FixedUpdate can be called multiple times per frame
	void FixedUpdate () {
		/*if (IsInLocomotion() && ((direction >= 0 && horizontal >= 0) || (direction < 0 && horizontal < 0))) {
            Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, rotationDegreePerSecond * (horizontal < 0f ? -1f : 1f), 0f), Mathf.Abs(horizontal));
            Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
            this.transform.rotation = this.transform.rotation * deltaRotation;
        }*/
		//Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, rotationDegreePerSecond * (horizontal < 0f ? -1f : 1f), 0f), Mathf.Abs(horizontal));
		//Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
		//this.transform.rotation = this.transform.rotation * deltaRotation;
		
		// Create a rotation that is an increment closer to the target rotation from the player's rotation.
		//Quaternion newRotation = Quaternion.Lerp(rigidbody.rotation, targetRotation, turnSmoothing * Time.deltaTime);
		
		// Change the players rotation to this new rotation.
		//rigidbody.MoveRotation(newRotation);
		
		if (speed > 0.5) {
			// Create a rotation based on this new vector assuming that up is the global y axis.
			Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
			Quaternion currentRotation = Quaternion.Lerp(this.transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
			this.transform.rotation = currentRotation;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (animator) {
			stateInfo = animator.GetCurrentAnimatorStateInfo(0);

			//check if we need to change state:
			if(IsClimbing()) {
				EnterState (STATE_CLIMB);
			} else {
				if(IsGrounded()) {
					EnterState (STATE_GROUND);
				} else {
					EnterState (STATE_AIR);
				}
			}

			if(current_state == STATE_GROUND) {
				animator.SetBool("IsInAir",false);

				GroundMoveControl();
				if(Input.GetKeyDown(KeyCode.JoystickButton0)) {
					//Debug.Log("Jumping");
					Jump();
				}

			} else if(current_state == STATE_AIR) {
				//If the line below is commented, jump animations are not used.
				//animator.SetBool("IsInAir",true);
				if(canControlInAir)
					GroundMoveControl();
			}
				
			else if(current_state == STATE_CLIMB) {
				animator.SetBool("IsInAir",false);
				vertical = Input.GetAxis("Vertical");
				if(vertical < 0 && IsGrounded()) {

					setClibmMode(false);
				} else {
					Vector3 trans = new Vector3(0,vertical*climbSpeed*Time.deltaTime,0);
					transform.Translate(trans);
				}
				


				//ClimbMoveControl();
			}



			//Debug.Log ("speed: " + speed);
			//Debug.Log ("Is grounded: " + grounded);
			
			//animator.SetFloat("direction", direction, directionDampTime, Time.deltaTime);
			/*if(Input.GetKeyDown(KeyCode.JoystickButton0)) {
				if(IsGrounded()) {
					Debug.Log("Jumping");
					Jump ();
				}

			}*/
		}
	}
	
	public void Jump() { 
		//animation.Play("jump_pose"); 
		this.rigidbody.AddForce(Vector3.up *jumpSpeed);
	}

	public void EnterState(int newState) {
		if (current_state == newState)
			return;

		Debug.Log("ENTERING new state " + newState);
		

		if (current_state == STATE_CLIMB)
			rigidbody.useGravity = true;


		//current_state = newState;
		switch (newState) {
		case STATE_GROUND:
			current_state = STATE_GROUND;
			break;
		case STATE_AIR:
			current_state = STATE_AIR;
			break;
		case STATE_CLIMB:
			current_state = STATE_CLIMB;
			speed = 0;
			animator.SetFloat("HorizontalSpeed",speed);
			rigidbody.useGravity = false;
			break;
		default:
			break;
		
		
		}
	}

	public void GroundMoveControl() {
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
		
		
		// basically h*h+v*v (but faster?)
		speed = new Vector2(horizontal, vertical).sqrMagnitude;
		
		//StickToWorldspace(this.transform, gamecam.transform, ref direction, ref speed);
		StickToWorldspace(this.transform, gamecam.transform, ref targetDirection);
		
		//animator.SetFloat("speed", speed);
		animator.SetFloat("HorizontalSpeed",speed);
	}

	public void ClimbMoveControl() {
		//horizontal = Input.GetAxis("Horizontal");



		
		// basically h*h+v*v (but faster?)
		//speed = new Vector2(horizontal, vertical).sqrMagnitude;




		
		//StickToWorldspace(this.transform, gamecam.transform, ref direction, ref speed);
		StickToWorldspace(this.transform, gamecam.transform, ref targetDirection);
		
		//animator.SetFloat("speed", speed);
		animator.SetFloat("HorizontalSpeed",speed);
	}
	
	public void StickToWorldspace (Transform root, Transform camera, ref Vector3 directionOut) {//ref float directionOut, ref float speedOut) {
		Vector3 rootDirection = root.forward;
		
		Vector3 stickDirection = new Vector3(horizontal, 0, vertical);
		
		//speedOut = stickDirection.sqrMagnitude;
		
		// get camera rotation (global)
		Vector3 cameraDirection = camera.forward;
		cameraDirection.y = 0.0f;
		Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, cameraDirection);
		
		// convert joystick input in Worldspace coordinates
		directionOut = referentialShift * stickDirection;
		/*
        Vector3 moveDirection = referentialShift * stickDirection;
		Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);

		float angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);

		angleRootToMove = angleRootToMove / 180f;

		directionOut = angleRootToMove * directionSpeed;
        */
	}
	
	public bool IsInLocomotion () {
		return stateInfo.nameHash == m_LocomotionId;
	}

	public void setClibmMode(bool isClimbing) {
		is_in_climb = isClimbing;
	}
	
	private bool IsGrounded () {
		RaycastHit rayhit;
		if(Physics.Raycast(this.transform.position,Vector3.down,out rayhit)) {
			if(rayhit.distance < groundDist)
				return true;
		}
		
		
		return false;
	}

	private bool IsClimbing() {
		return this.is_in_climb;  
	}


	
}
