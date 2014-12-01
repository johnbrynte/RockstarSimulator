#pragma strict

function OnTriggerEnter(collider : Collider) {
	if (collider.name == 'Body')
		Application.LoadLevel("game_over");
}