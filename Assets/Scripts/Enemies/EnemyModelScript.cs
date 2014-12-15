using UnityEngine;
using System.Collections;

public class EnemyModelScript : MonoBehaviour {

	GameObject enemyAgent;
	public float minDistance;
	public float maxDistance;
	public float maxVelocity;
	public float turnSmooth = 10;

	private bool active = true;
	private float timeout = 3;

	// Use this for initialization
	void Start () {
		foreach (Transform child in transform.parent.transform)
		{
			if (child.gameObject.tag == "Enemy")
			{
				enemyAgent = child.gameObject;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Should not be here, but cant be in movement cause they are attached to different things. This is what we can get from the collision detection AFAIK /peter
	public void GotHit(Vector3 force) {
		active = false;
		timeout = 3;
		enemyAgent.GetComponent<EnemyMovementCS>().GotHit();
		rigidbody.velocity = Vector3.zero;
		rigidbody.AddForce(force);
	}

	void FixedUpdate () {
		if (!active) {
			timeout -= Time.deltaTime;
			if (timeout <= 0) {
				active = true;
			}
		}
		Vector3 direction = enemyAgent.transform.position - transform.position;
		direction.y = 0;
		direction.Normalize();
		//Distance check to agent
		float distance = Vector3.Distance(transform.position, enemyAgent.transform.position);
		if (distance > minDistance && active) {
			float velocity = distance / Time.fixedDeltaTime;
			if (velocity > maxVelocity) {
					//velocity = maxVelocity;
			}
			rigidbody.velocity = Vector3.Lerp (rigidbody.velocity, direction * velocity, Time.fixedDeltaTime);
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.FromToRotation(Vector3.forward, direction), turnSmooth*Time.fixedDeltaTime);

			//If we are to far away
			if (distance > maxDistance) {
				float d = distance - maxDistance;
				Vector3 moveDir = transform.position - enemyAgent.transform.position;
				moveDir.Normalize ();
				enemyAgent.GetComponent<NavMeshAgent> ().transform.position += moveDir * d;
			} else {
			}
		}

	}
}
