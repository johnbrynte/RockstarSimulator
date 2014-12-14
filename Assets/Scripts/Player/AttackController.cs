using UnityEngine;
using System.Collections;

public class AttackController : MonoBehaviour {

	public float attackVelocity = 10.0f;
	public float attackForce = 50.0f;

	private bool isAttacking = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider col) {
		if (isAttacking) {
			if (col.gameObject.CompareTag("Dynamic")) {
				Vector3 direction = this.transform.forward;
				direction.y = 0.2f;
				direction.Normalize();
				col.gameObject.rigidbody.velocity = direction*attackForce/col.gameObject.rigidbody.mass;
				audio.Play();
			}
		}
		
	}

	public void SetAttacking(bool attack) {
		isAttacking = attack;
	}
}
