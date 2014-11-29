using UnityEngine;
using System.Collections;

public class EnemyMovementCS : MonoBehaviour
{
	public Transform toFollow;
	public NavMeshAgent navComponent;


	// Use this for initialization
	void Start () {
		//Assign the NavMeshAgent at game start
		navComponent = this.GetComponent<NavMeshAgent> ();//this.transform.GetComponent(NavMeshAgent);
		
		navComponent.speed = Random.Range(3.0f, 6.8f);
		navComponent.acceleration = Random.Range(8.0f, 9.0f);
		navComponent.angularSpeed = Random.Range(90.0f, 140.0f);
	}

	// Update is called once per frame
	void Update () {
		//If we have something to follow set
		if (toFollow) {
			
			//Set where we should go
			navComponent.SetDestination(toFollow.position);
			
		}
	}

	public void GotHit() {
		//Debug.Log ("Im hit!");
		Destroy (this.gameObject);
	}
}

