using UnityEngine;
using System.Collections;

public class BarrelCannonScript : MonoBehaviour {

	public Rigidbody barrel;
	public float shootingInterval = 3.0f;
	public float shootingSpeed = 100.0f;
	public float barrelLifetime = 3.0f;

	private float time;

	// Use this for initialization
	void Start () {
		time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time >= shootingInterval) {
			time = 0;
			Rigidbody barrelClone = (Rigidbody) Instantiate(barrel, transform.position, transform.rotation);
			barrelClone.velocity = transform.forward * shootingSpeed;

			barrelClone.GetComponent<BarrelScript>().SetToKill(barrelLifetime);
		}
	}
}
