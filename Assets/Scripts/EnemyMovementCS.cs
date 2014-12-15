using UnityEngine;
using System.Collections;

public class EnemyMovementCS : MonoBehaviour
{
	private Transform toFollow;
	private NavMeshAgent navComponent;

	private bool active = true;
	private float timeout = 3;


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
		navComponent.stoppingDistance = 1;
	}

	// Update is called once per frame
	void Update () {
		if (!active) {
			timeout -= Time.deltaTime;
			if (timeout <= 0) {
				active = true;
			}
		}
		//If we have something to follow set
		if (toFollow && active) {
			
			//Set where we should go
			navComponent.SetDestination(toFollow.position);
			
		}
	}

	public void GotHit() {
		active = false;
		timeout = 3;
	}
}

