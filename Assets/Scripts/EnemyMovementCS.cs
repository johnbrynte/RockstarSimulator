using UnityEngine;
using System.Collections;

public class EnemyMovementCS : MonoBehaviour
{
	private Transform toFollow;
	private NavMeshAgent navComponent;


	// Use this for initialization
	void Start () {

		//Assign player transform to follow
		toFollow =  GameObject.FindGameObjectWithTag("PlayerBody").transform;
		if (toFollow == null) {
			Debug.LogError("No player was found! No object with tag PlayerBody");
				}

		//Assign the NavMeshAgent at game start
		navComponent = this.GetComponent<NavMeshAgent> ();//this.transform.GetComponent(NavMeshAgent);
		
		navComponent.speed = Random.Range(5.0f, 6.0f);
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

