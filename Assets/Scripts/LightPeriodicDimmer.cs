using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class LightPeriodicDimmer : MonoBehaviour {

	private float period;
	private float time;
	private float intensity = 1.5f;

	// Use this for initialization
	void Start () {
		period = Random.Range(1, 4);
		time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time > period)
			time = 0;
		light.intensity = intensity * (0.5f + Mathf.Sin (Mathf.PI * time / period));
	}
}
