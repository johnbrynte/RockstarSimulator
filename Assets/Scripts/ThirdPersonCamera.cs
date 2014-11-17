using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {

	[SerializeField]
	private float distanceAway;
	[SerializeField]
	private float distanceUp;
	[SerializeField]
	private float smooth;
	[SerializeField]
	private Transform followXForm;
	[SerializeField]
	private CharacterControllerLogicNew character;
	[SerializeField]
	private Vector3 offset = Vector3.zero;

	private Vector3 lookDir;
	private Vector3 curLookDir;
	private Vector3 targetPosition;

	private Vector3 velocityCamSmooth = Vector3.zero;
	[SerializeField]
	private float camSmoothDampTime;
	private Vector3 velocityLookDir = Vector3.zero;
	[SerializeField]
	private float lookDirDampTime;

	private CameraStates cameraState = CameraStates.Behind;

	public enum CameraStates {
		Behind,
		Free
	}

	// Use this for initialization
	void Start () {
		//character = GameObject.FindWithTag("Player").GetComponent<CharacterControllerLogicNew>();
		//if(character == null)
	///		Debug.Log ("CHAR IS NULL ");
		//followXForm = GameObject.FindWithTag("Player").transform;
		followXForm = character.transform;
		lookDir = followXForm.forward;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdate () {
		float leftX = Input.GetAxis("Horizontal");
		float leftY = Input.GetAxis("Vertical");
		//float rightX = Input.GetAxis("RightStickX");
		//float rightY = Input.GetAxis("RightStickY");

		Vector3 characterOffset = followXForm.position + offset;

		switch (cameraState) {
			case CameraStates.Behind:
				if (character.IsInLocomotion() && character.Speed > character.LocomotionThreshold) {
					lookDir = Vector3.Lerp(followXForm.right * (leftX < 0 ? 1f : -1f), followXForm.forward * (leftY < 0 ? -1f : 11f), Mathf.Abs(Vector3.Dot(this.transform.forward, followXForm.forward)));

					lookDir = characterOffset - this.transform.position;
					lookDir.y = 0f;
					lookDir.Normalize();

					curLookDir = Vector3.SmoothDamp(curLookDir, lookDir, ref velocityLookDir, lookDirDampTime);
				}
				break;
			case CameraStates.Free:
				break;
		}

		// move to the target position behind the character
		targetPosition = characterOffset + followXForm.up * distanceUp - lookDir * distanceAway;

		CompensateForWalls(characterOffset, ref targetPosition);
		SmoothPosition(this.transform.position, targetPosition);
		transform.LookAt(characterOffset);
	}

	private void SmoothPosition(Vector3 fromPos, Vector3 toPos) {
		// smooth camera position transition
		this.transform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
	}

	private void CompensateForWalls(Vector3 fromPos, ref Vector3 toPos) {
		RaycastHit wallHit = new RaycastHit();
		if (Physics.Linecast(fromPos, toPos, out wallHit)) {
			toPos = new Vector3(wallHit.point.x, toPos.y, wallHit.point.z);
		}
	}

}
