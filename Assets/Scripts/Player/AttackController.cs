using UnityEngine;
using System.Collections;

public class AttackController : MonoBehaviour {

	public float attackVelocity = 10.0f;
	public float attackForce = 2000.0f;

	private bool isAttacking = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider col) {
		if (isAttacking && col.gameObject.rigidbody != null) {
			Vector3 velocity = this.transform.forward;
			velocity.y = 0.2f;
			velocity.Normalize();
			velocity = velocity*attackForce/col.gameObject.rigidbody.mass;

			if (col.gameObject.CompareTag("Dynamic")) {
				col.gameObject.rigidbody.velocity = velocity;
				audio.Play();
			} else if(col.name.Contains("Michael")) { //uuuh, but have to be like this i guess
				EnemyModelScript enemy = col.gameObject.GetComponent<EnemyModelScript>();
				enemy.GotHit(velocity);
				audio.Play();
			}
		}
		
	}

	public void SetAttacking(bool attack) {
		isAttacking = attack;
	}
}
