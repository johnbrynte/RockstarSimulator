using UnityEngine;
using System.Collections;

public class Climbable : MonoBehaviour {

	public CharacterControllerLogicNew player;
	private bool hasEntered = false;	

	void Start () {
		player = GameObject.FindWithTag("Player").GetComponent<CharacterControllerLogicNew>();
		if(player == null)
				Debug.Log ("CHAR IS NULL ");
		//followXForm = GameObject.FindWithTag("Player").transform;
		//followXForm = character.transform;
		//lookDir = followXForm.forward;
	}

	void OnTriggerEnter(Collider other) {
		if (other.Equals (this.player.collider) && !hasEntered) {
			player.setClibmMode(true);
			hasEntered = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.Equals (this.player.collider)) {
			player.setClibmMode(false);
			hasEntered = false;
		}
	}
}
