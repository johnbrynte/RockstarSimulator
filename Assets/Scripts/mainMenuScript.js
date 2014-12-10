#pragma strict

var mainMenuTexture : Texture;
var mainMenuFont : Font;
var audioSourceIntro : AudioSource;
var audioSourceBackground : AudioSource;

private var introPlayed;

function Awake () {
	introPlayed = false;
}
 
function OnGUI() {
	GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),mainMenuTexture);

	var yPos = Screen.height /6;
	var xPos = Screen.width / 2 - 130;
	
	GUI.skin.button.font = mainMenuFont;
	GUI.skin.button.fontSize = 26;
	
	if (!audioSourceIntro.isPlaying && !introPlayed) {
		audioSourceIntro.Play();
		introPlayed = true;
	}
	
	if (!audioSourceIntro.isPlaying && !audioSourceBackground.isPlaying && introPlayed) {
		audioSourceBackground.Play();
	}
	
	if (GUI.Button(new Rect(xPos, yPos , 250, 50),"Start game")) 
	{
		Application.LoadLevel("debuglevel_1_with_mesh_and_enemies");
	}
	if (GUI.Button(new Rect(xPos, yPos * 2, 250, 50),"Quit game")) 
	{
		Application.Quit();
	}
	
	
}
