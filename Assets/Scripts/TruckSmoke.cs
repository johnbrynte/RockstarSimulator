using UnityEngine;
using System.Collections;

public class TruckSmoke : MonoBehaviour {

	private ParticleSystem particle;

	// Use this for initialization
	void Start () {

		particle = GetComponent<ParticleSystem> ();

		startSmoke ();
	
	}

	public void stopSmoke(){
		particle.Stop ();

	}

	public void startSmoke(){
		if(!particle.isPlaying)
		particle.Play ();
	}
}
