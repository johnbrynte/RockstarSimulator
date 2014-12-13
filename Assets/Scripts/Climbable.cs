using UnityEngine;
using System.Collections;

public class Climbable : MonoBehaviour {

	public CharacterControllerLogic player;
	private double timer = 0;
	private double count = 0;
	private bool hasEntered = false;

	void Start () {
		player = GameObject.FindWithTag("PlayerBody").GetComponent<CharacterControllerLogic>();
		if(player == null)
			Debug.Log ("CHAR IS NULL ");
		//followXForm = GameObject.FindWithTag("Player").transform;
		//followXForm = character.transform;
		//lookDir = followXForm.forward;
	}

	void OnTriggerStay(Collider other) {
		count += Time.deltaTime;
		if (count < 0.5) {
			return;
		}
		count = 0;
		enableClimb(other);
		
	}

	private void enableClimb(Collider other) {
		if (other.Equals (this.player.collider)) {
			player.setClimbMode(true);
			hasEntered = true;
		}

	}
	void OnTriggerEnter(Collider other) {
		if (timer > 0) {
			return;
		}
		enableClimb(other);
	}

	void OnTriggerExit(Collider other) {
		if (other.Equals (this.player.collider)) {
			player.setClimbMode(false);
			hasEntered = false;
			timer = 1.5;
			count = 0;
		}
	}

	void Update() {
		timer -= Time.deltaTime;
	}
}
