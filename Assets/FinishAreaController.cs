using UnityEngine;
using System.Collections;

public class FinishAreaController : MonoBehaviour {

	//When a collision is triggered

//	void OnCollisionEnter(Collision collision) {
	void OnTriggerEnter(Collider collision) {

		//if it is the player
		if (collision.gameObject.tag == "Player"){

			//Load the correct level
			Application.LoadLevel("new_game");

			}
	}
		                                          


}
