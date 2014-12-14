using UnityEngine;
using System.Collections;

public class SceneFadeInOut : MonoBehaviour
{
	public float fadeSpeed = 1.5f; // Speed that the screen fades to and from black.
	public Texture introTexture;
	
	private bool sceneStarting = true; // Whether or not the scene is still fading in.
	private bool sceneTransitionFadeOut = false;
	private bool sceneTransitionFadeIn = false;
	private bool sceneEnding = false;
	private int nframes;
	
	void Awake ()
	{
		nframes = 0;
	}
	
	
	void Update ()
	{
		nframes++;
		// If the scene is starting...
		if(sceneStarting)
			// ... call the StartScene function.
			StartScene();
		if (nframes > 300)
			sceneTransitionFadeOut = true;
		if (sceneTransitionFadeIn || sceneTransitionFadeOut)
			Transition ();
		if (nframes > 2000)
			sceneEnding = true;
		if (sceneEnding)
			EndScene();
	}

	void onGUI() {
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),introTexture);
	}
	
	
	void FadeToClear ()
	{
		// Lerp the colour of the texture between itself and transparent.
		guiTexture.color = Color.Lerp(guiTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
	}
	
	
	void FadeToBlack ()
	{
		// Lerp the colour of the texture between itself and black.
		guiTexture.color = Color.Lerp(guiTexture.color, Color.black, fadeSpeed * Time.deltaTime);
	}
	
	
	void StartScene ()
	{
		// Fade the texture to clear.
		FadeToClear();
		
		// If the texture is almost clear...
		if(guiTexture.color.a <= 0.05f)
		{
			// ... set the colour to clear and disable the GUITexture.
			guiTexture.color = Color.clear;
			guiTexture.enabled = false;
			
			// The scene is no longer starting.
			sceneStarting = false;
		}
	}

	void Transition ()
	{
		// Make sure the texture is enabled.
		guiTexture.enabled = true;
		
		// Start fading towards black.
		if (sceneTransitionFadeOut) {
			FadeToBlack();
			if (guiTexture.color.a >= 0.95f) {
				sceneTransitionFadeOut = false;
				sceneTransitionFadeIn = true;
			}
		}
		if (sceneTransitionFadeIn) {
			FadeToClear();
			if(guiTexture.color.a <= 0.05f) {
				guiTexture.color = Color.clear;
				guiTexture.enabled = false;
				sceneTransitionFadeIn = false;
			}
		}

	}
	
	
	public void EndScene ()
	{
		// Make sure the texture is enabled.
		guiTexture.enabled = true;
		
		// Start fading towards black.
		FadeToBlack();
		
		// If the screen is almost black...
		if(guiTexture.color.a >= 0.95f)
			Application.LoadLevel("new_game");
	}
}
