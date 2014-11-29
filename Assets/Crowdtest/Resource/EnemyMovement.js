public
var toFollow: Transform;
private
var navComponent: NavMeshAgent;

function Start() {

	//Assign the NavMeshAgent at game start
	navComponent = this.transform.GetComponent(NavMeshAgent);

	navComponent.speed = Random.Range(3.0, 6.8);
	navComponent.acceleration = Random.Range(8.0, 9.0);
	navComponent.angularSpeed = Random.Range(90.0, 140.0);

}

function Update() {

	//If we have something to follow set
	if (toFollow) {

		//Set where we should go
		navComponent.SetDestination(toFollow.position);

	}

}

function GotHit() {
	//TODO
}