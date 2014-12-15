using UnityEngine;
using System.Collections;

public class EndingScript : MonoBehaviour {

	public Texture endScreen;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKey)
			Application.LoadLevel("new_game");
	}

	void OnGUI() {
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), endScreen, ScaleMode.ScaleAndCrop, true, 0);
	}
}
