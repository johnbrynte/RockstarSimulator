#pragma strict

public var toFollow : Transform;
private var navComponent : NavMeshAgent;

function Start () {

	//Assign the NavMeshAgent at game start
	navComponent = this.transform.GetComponent(NavMeshAgent);

}

function Update () {

	//If we have something to follow set
	if(toFollow){
	
		//Set where we should go
		navComponent.SetDestination(toFollow.position);
		
	}

}