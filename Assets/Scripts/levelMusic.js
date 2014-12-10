#pragma strict


function Update () {
	
	if (!audio.isPlaying) {
		audio.Play();
	}
}