using UnityEngine;
using System.Collections;

public class NextLevelScript : MonoBehaviour {

	public string NextLevelName;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "PlayerBody") {
			Application.LoadLevel(NextLevelName);
		}
	}
}
