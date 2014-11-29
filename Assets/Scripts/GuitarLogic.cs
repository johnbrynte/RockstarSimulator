using UnityEngine;
using System.Collections;

public class GuitarLogic : MonoBehaviour {

	public bool infiniteAttacks = true;
	public int attacksLeft = 10;
	public float attackCooldown = 0.5f;

	public Transform playerSpine;
	public CharacterControllerLogic playerLogic;

	public bool startsOnPlayer = true;

	public float angularSpeed = 10f;
	public float radius = 0.7f;
	public float swingAngle = 180f;
	private float currentAngle = 0f;
	//private float maxAngle;
	//private float angularSpeedRad;
	//private float maxAngleRad;
	private float radFactor;

	private float coolDown;
	private bool isCooldown = false;


	private int STATE_PLAYER = 0;
	private int STATE_PICKUP = 1; //Note: picking up guitars not implemented (yet?)
	private int currentState;


	private bool attacking = false;


	// Use this for initialization
	void Start () {
		if (startsOnPlayer) {
			AttachToPlayer();
			currentState = STATE_PLAYER;
		} else {
			currentState = STATE_PICKUP;
		}

		//maxAngle = sw

		//angularSpeedRad = angularSpeed * (Mathf.PI / 180f);
		//maxAngleRad = (90f + swingAngle) * (Mathf.PI / 180f);
		radFactor = Mathf.PI / 180;
	}

	void Update() {
		if (isCooldown) {
			coolDown -= Time.deltaTime;
			if(coolDown <= 0)
				isCooldown = false;
		}
	}

	void FixedUpdate() {
		if (attacking) {
			AttackTransformation();
			if(currentAngle > (90+swingAngle)) {
				//attack ended
				attacking = false;
				ResetPositionToBack();
				coolDown = attackCooldown;
				isCooldown = true;
			}
		}
	}

	void OnTriggerEnter(Collider col) {
		//Debug.Log ("Collision!");

		if(attacking && col.gameObject.CompareTag("Enemy")) {
			//Debug.Log ("Hit!");
			//col.gameObject.GetComponent<EnemyMovementCS>().GotHit();
			EnemyMovementCS enemy = col.gameObject.GetComponent<EnemyMovementCS>();
			enemy.GotHit();
			if(!infiniteAttacks) {
				attacksLeft -= 1;
				if(attacksLeft <= 0) {
					Break();
				}
			}
		}

	}

	public void AttachToPlayer() {

		this.transform.parent = playerSpine;
		ResetPositionToBack ();
		this.playerLogic.setWeapon (this);
	}

	public void ResetPositionToBack() {
		this.transform.localPosition = new Vector3 (0.02267391f, 0.001418967f, 0.1993044f);
		this.transform.localEulerAngles = new Vector3(0, 83.2177f, 90.00002f);
	}

	public void Attack() {
		if (!attacking && currentState == STATE_PLAYER && !isCooldown) {
			//position guitar in attack position
			this.transform.localPosition = new Vector3 (-0.1173791f, -0.5730895f, -0.1939585f);
			this.transform.localEulerAngles = new Vector3(90.0f, 0f, 0f);
			currentAngle = 90;

			attacking = true;
		}

	}

	private void AttackTransformation() {
		currentAngle += angularSpeed;
		float newX = Mathf.Cos (-currentAngle*radFactor) * radius;
		float newY = Mathf.Sin (-currentAngle*radFactor) * radius;
		this.transform.localPosition = new Vector3 (this.transform.localPosition.x, newY, newX);
		//this.transform.localPosition.x = newX;
		//this.transform.localPosition.y = newY;
		//this.transform.rotation.y = currentAngle;
		//Vector3 tmp = this.transform.localEulerAngles;
		//this.transform.localEulerAngles = new Vector3 (currentAngle, 0, 0);

		this.transform.Rotate (new Vector3 (angularSpeed, 0, 0));
		//this.transform.Translate
	}

	public void Break() {
		this.playerLogic.setWeapon(null);
		Destroy (gameObject);
	}


}
