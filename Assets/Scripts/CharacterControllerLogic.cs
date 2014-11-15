using UnityEngine;
using System.Collections;

public class CharacterControllerLogic : MonoBehaviour {
	
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
	
	private float speed = 0.0f;
	private float direction = 0.0f;
	private float horizontal = 0.0f;
	private float vertical = 0.0f;

    private Vector3 currentDirection = Vector3.zero;
    private Vector3 targetDirection = Vector3.zero;

	private AnimatorStateInfo stateInfo;

	private int m_LocomotionId = 0;

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

			horizontal = Input.GetAxis("Horizontal");
			vertical = Input.GetAxis("Vertical");
			
			// basically h*h+v*v (but faster?)
			speed = new Vector2(horizontal, vertical).sqrMagnitude;
			
			//StickToWorldspace(this.transform, gamecam.transform, ref direction, ref speed);
            StickToWorldspace(this.transform, gamecam.transform, ref targetDirection);

			animator.SetFloat("speed", speed);
			//animator.SetFloat("direction", direction, directionDampTime, Time.deltaTime);
		}
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

}
