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
	private Transform followXform;
	[SerializeField]
	private Vector3 offset = Vector3.zero;

	private Vector3 targetPosition;
	private Vector3 lookDir;

	private Vector3 velocityCamSmooth = Vector3.zero;
	[SerializeField]
	private float camSmoothDampTime;

	// Use this for initialization
	void Start () {
		followXform = GameObject.FindWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdate () {
		Vector3 characterOffset = followXform.position + offset;

		lookDir = characterOffset - this.transform.position;
		lookDir.y = 0f;
		lookDir.Normalize();

		// move to the target position behind the character
		//targetPosition = followXform.position + Vector3.up * distanceUp - followXform.forward * distanceAway;
		targetPosition = characterOffset + followXform.up * distanceUp - lookDir * distanceAway;
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
