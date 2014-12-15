#pragma strict

function OnTriggerEnter(collider : Collider) {
	if (collider.name.Contains('Michael'))
		Application.LoadLevel("game_over");
}