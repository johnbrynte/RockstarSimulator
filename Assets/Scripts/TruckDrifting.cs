using UnityEngine;
using System.Collections;

public class TruckDrifting : MonoBehaviour {

	public Transform driftTarget;
	public float speed;
	private TruckSmoke smokeScript;
	private AudioSource audio;



	// Use this for initialization
	void Start () {

		audio = GetComponent<AudioSource> ();
		if (audio == null) {
						Debug.LogError ("No audio source found for the truck");
				}

		smokeScript = GetComponentInChildren<TruckSmoke> ();
	
	
	}
	
	// Update is called once per frame
	void Update () {

	
		float dist = Vector3.Distance(driftTarget.position, transform.position);

		if (driftTarget != null && dist <= 15) {
				if (!audio.isPlaying) {
						audio.Play ();
				}

				smokeScript.startSmoke();

				transform.RotateAround (driftTarget.position, Vector3.up, speed * Time.deltaTime);
		} else {
			audio.Stop ();
			smokeScript.stopSmoke();
		}
	
	}
}
