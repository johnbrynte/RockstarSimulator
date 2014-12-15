using UnityEngine;
using System.Collections;

public class BarrelScript : MonoBehaviour {

	private float killDelay;
	private bool kill = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (kill && (killDelay -= Time.deltaTime) < 0)
			Destroy(gameObject);
	}

	public void SetToKill(float delay) {
		killDelay = delay;
		kill = true;
	}
}
