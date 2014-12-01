#pragma strict

var gameOverTexture : Texture;
 
function OnGUI() {
	GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),gameOverTexture);

	var yPos = Screen.height /2 + 50;
	var xPos = Screen.width / 2 - 80;
	
	if (GUI.Button(new Rect(xPos, yPos , 150, 25),"Try again")) 
	{
		Application.LoadLevel("debuglevel_1_with_mesh_and_enemies");
	}
	if (GUI.Button(new Rect(xPos, yPos + 25, 150, 25),"Quit")) 
	{
		Application.Quit();
	}
}