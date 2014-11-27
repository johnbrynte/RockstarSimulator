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
	private CharacterControllerLogic character;
	[SerializeField]
	private Vector3 offset = Vector3.zero;
	
	public float distanceAwayMultiplier = 1.5f;
	public float distanceUpMultiplier = 5f;
	public float freeThreshold = .1f;
	public Vector2 camMinDistFromChar = new Vector2 (1f, -.5f);
	public float rightStickThreshold = .1f;
	public float freeRotationDegreePerSecond = 5f;
    public float zoomSpeed = .4f;

    private Transform parentRig;
	private Vector3 lookDir;
	private Vector3 curLookDir;
	private Vector3 targetPosition;
    private Vector3 characterOffset;
    private float prevZoom = 0f;

	private Vector3 velocityCamSmooth = Vector3.zero;
	[SerializeField]
	private float camSmoothDampTime;
	private Vector3 velocityLookDir = Vector3.zero;
	[SerializeField]
	private float lookDirDampTime;

	private Vector3 savedRigToGoal;
	private float distanceAwayFree;
	private float distanceUpFree;
	private Vector2 rightStickPrevFrame = Vector2.zero;

	public enum CameraStates {
		Behind,
		Free
	}

	private CameraStates cameraState = CameraStates.Behind;

	// Use this for initialization
	void Start () {
        parentRig = this.transform.parent;
		curLookDir = followXForm.forward;
        distanceAwayFree = distanceAway;
        distanceUpFree = distanceUp;
        characterOffset = followXForm.position + offset;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate () {
		float leftX = Input.GetAxis("Horizontal");
		float leftY = Input.GetAxis("Vertical");
		float rightX = Input.GetAxis("RightStickX");
		float rightY = Input.GetAxis("RightStickY");

		characterOffset = followXForm.position + offset;//Vector3.Lerp(characterOffset, followXForm.position + offset, targetSmoothing);
		targetPosition = Vector3.zero;

		if (Mathf.Abs(rightX) > freeThreshold || Mathf.Abs(rightY) > freeThreshold) {
			cameraState = CameraStates.Free;
			savedRigToGoal = Vector3.zero;
		} else {
            cameraState = CameraStates.Behind;
        }

		switch (cameraState) {
			case CameraStates.Behind:
				if (character.IsInLocomotion()) {
					lookDir = Vector3.Lerp(followXForm.right * (leftX < 0 ? 1f : -1f), followXForm.forward * (leftY < 0 ? -1f : 11f), Mathf.Abs(Vector3.Dot(this.transform.forward, followXForm.forward)));

					lookDir = characterOffset - this.transform.position;
					lookDir.y = 0f;
					lookDir.Normalize();

					curLookDir = Vector3.SmoothDamp(curLookDir, lookDir, ref velocityLookDir, lookDirDampTime);
                    curLookDir.Normalize();
                }
                // move to the target position behind the character
                targetPosition = characterOffset + followXForm.up * distanceUpFree - curLookDir * distanceAwayFree;
				break;
			case CameraStates.Free:
				Vector3 rigToGoalDirection = Vector3.Normalize(characterOffset - this.transform.position);
				rigToGoalDirection.y = 0f;
                Vector3 rigToGoal = characterOffset - parentRig.position;
                rigToGoal.y = 0f;

                if (rightY < -rightStickThreshold && rightY <= rightStickPrevFrame.y)
                {
                    distanceUpFree = Mathf.Lerp(Mathf.Abs(this.transform.position.y - characterOffset.y), distanceUp * distanceUpMultiplier, Mathf.Abs(rightY) * zoomSpeed);
                    distanceAwayFree = Mathf.Lerp(rigToGoal.magnitude, distanceAway * distanceAwayMultiplier, Mathf.Abs(rightY) * zoomSpeed);
                    targetPosition = characterOffset + followXForm.up * distanceUpFree - rigToGoalDirection * distanceAwayFree;
                }
                else if(rightY > rightStickThreshold && rightY >= rightStickPrevFrame.y)
                {
                    distanceUpFree = Mathf.Lerp(Mathf.Abs(this.transform.position.y - characterOffset.y), camMinDistFromChar.y, rightY * zoomSpeed);
                    distanceAwayFree = Mathf.Lerp(rigToGoal.magnitude, camMinDistFromChar.x, rightY * zoomSpeed);
                    targetPosition = characterOffset + followXForm.up * distanceUpFree - rigToGoalDirection * distanceAwayFree;
                }

                if (rightX != 0 || rightY != 0)
                {
                    savedRigToGoal = rigToGoalDirection;
                }

                parentRig.RotateAround(characterOffset, followXForm.up, freeRotationDegreePerSecond * (Mathf.Abs(rightX) > rightStickThreshold ? rightX : 0f));

                if (targetPosition == Vector3.zero) {
                    targetPosition = characterOffset + followXForm.up * distanceUpFree - savedRigToGoal * distanceAwayFree;
                }

                curLookDir = parentRig.rotation * Vector3.forward;

                SmoothCameraPosition(targetPosition);
                this.transform.LookAt(characterOffset);
				break;
		}

        if (cameraState != CameraStates.Free)
        {
    		CompensateForWalls(characterOffset, ref targetPosition);
    		SmoothCameraPosition(targetPosition);
    		this.transform.LookAt(characterOffset);
        }

        rightStickPrevFrame.x = rightX;
        rightStickPrevFrame.y = rightY;
	}
    
	private void SmoothCameraPosition(Vector3 toPos) {
        parentRig.position = Vector3.Lerp(parentRig.position, toPos, camSmoothDampTime);
		// smooth camera position transition
		//parentRig.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
	}

	private void CompensateForWalls(Vector3 fromPos, ref Vector3 toPos) {
		RaycastHit wallHit = new RaycastHit();
		if (Physics.Linecast(fromPos, toPos, out wallHit)) {
            // avoid the player (should probably use a layer mask)
            if (wallHit.collider != character.GetComponent<CapsuleCollider>())
    			toPos = new Vector3(wallHit.point.x, toPos.y, wallHit.point.z);
		}
	}

}
