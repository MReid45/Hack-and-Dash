using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	private PlayerScore score;
	//Layer Masks
	public LayerMask playerMask;


	// floats
	public float speed = 10f;
	public float maxSpeed = 5f;
	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;


	[Range(1,10)]
	public float jumpvelocity;

	//Tranforms and RB's
	Transform myTransform;
	Rigidbody2D myBody;

	//Booleans
	public bool dead = false;
	public bool isSwinging;
	public bool isGrounded;
	public bool isChild; 
	bool doCamShake;
	bool canMakeThud = true;

	// For Swinging physics
	public Vector2 ropeHook;
	public float swingForce = 4f;

	// Collisio stuff
	public CircleCollider2D groundCollider;
	public LayerMask groundLayers;
    public Animator anim;

    public float groundingTolerance = .1f;
	public float jumpingTolerance = .1f;
	private float moveHorizontal;
	public float spawnDuration = .3f;

	//To precisely detect jumping
	float lostGroundingTime;
	float lastJumpTime;
	float lastInputJump;
	float defaultYPos;

    double idleSeconds = 0; //check how long char doesn't move before turns idle

    int faceDirection = 0; //character sprite facing left or right


	//Respawning stuff
	protected Vector2 respawnPos;
	protected float threshold = -15f;
	public int deathCount;
	RopeSystem system;

	//Sound Stuff
	AudioSource source;
	public AudioClip jumpSound;
	public AudioClip landThud;
	PolygonCollider2D thisCollider;

	//Reference to Camera gameobject
	public GameObject mainCamera;
	CameraShake cameraShake;
	CameraUpDown camUpDown;
    private const float ThreePiOverFour = Mathf.PI * 3 / 4;
    private const float PiOverFour = Mathf.PI / 4;

    // Use this for initialization
    void Start () {
		defaultYPos = mainCamera.transform.position.y;
		deathCount = 0;
		myBody = this.GetComponent<Rigidbody2D> ();
		myTransform = this.GetComponent<Transform>();
        anim = GetComponent<Animator>();
		system = GetComponent<RopeSystem> ();
		source = GetComponent<AudioSource> ();
        respawnPos = myTransform.position;
		camUpDown = mainCamera.GetComponent<CameraUpDown> ();
		GameObject playerScoreObject = GameObject.FindGameObjectWithTag ("PlayerScore");
		score = playerScoreObject.GetComponent<PlayerScore> ();
		thisCollider = this.GetComponent<PolygonCollider2D> ();
	}

    // Update is called once per frame
    void Update()
    {
       // mainCamera.GetComponent<CameraShake>().anim.SetInteger("Jump", 0);
        //check if char moving to make idle or active
        if (!Input.anyKey)
        {
            idleSeconds++;
        }
        else
        {
        // If a button is being pressed restart counter to Zero
         idleSeconds = 0;
        }
        if (idleSeconds > 100)
        {
            anim.SetInteger("State", 1);
            anim.SetInteger("Facing", faceDirection);
        }
        else
        {
            anim.SetInteger("State", 0);
            anim.SetInteger("Facing", faceDirection);
        }

        moveHorizontal = Input.GetAxis("Horizontal");

        directionOfSprite(); //sprite face left or right

        isGrounded = CheckGrounded ();

		if (isGrounded)
		{
			camUpDown.enabled = true;
			if (doCamShake)
			{
				source.volume = .5f;
				if (canMakeThud)
				{
					
					//source.PlayOneShot (landThud);
					mainCamera.GetComponent<CameraShake>().StartCameraShake();
                    canMakeThud = false;
					StartCoroutine (SoundDelay (.8f));
				}
				// camera shake
				doCamShake = false;
			}
		} else
			doCamShake = true;

        //isGrounded = Physics2D.Linecast (myTransform.position, tagGround.position, playerMask);

        //Checks to see if move buttons have been pressed
		if (moveHorizontal < 0f || moveHorizontal > 0f && dead == false)
		{

			if (isSwinging)
			{
			// 1 - Get a normalized direction vector from the player to the hook point
				var playerToHookDirection = (ropeHook - (Vector2)transform.position).normalized;
			// 2 - Inverse the direction to get a perpendicular direction
				Vector2 perpendicularDirection;
				if (moveHorizontal < 0)
				{
					perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
					var leftPerpPos = (Vector2)transform.position - perpendicularDirection * -3f;
					Debug.DrawLine(transform.position, leftPerpPos, Color.green, 0f);
				}else 
				{
					perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
					var rightPerpPos = (Vector2)transform.position + perpendicularDirection * 3f;
					Debug.DrawLine(transform.position, rightPerpPos, Color.green, 0f);
				}
				var force = perpendicularDirection * swingForce; // Swinging needs a tweak, increases over time instead of decreasing when input is 0
				myBody.AddForce(force, ForceMode2D.Force);
			}
			else // meaning player is on ground 
			Move (Input.GetAxisRaw("Horizontal"));
        //Store the current horizontal input in the float moveHorizontal.
		}

		if (moveHorizontal == 0 && isGrounded)
		{
			//myBody.constraints.Equals(
			//myBody.isKinematic = false;
			//myBody.drag = 10;
		}
		//Better jumping 
		if (myBody.velocity.y < 0)
			myBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		else if (myBody.velocity.y > 0 && !Input.GetButton ("Jump"))
		{
			myBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}
			

		//Jump Check
		if ((CheckJumpInput () && PermissionToJump () && !isSwinging && dead == false))
		{
            anim.SetInteger("State", 2);
            Jump ();
		}


		CheckDeath ();
        if (dead == true)
        {
            anim.SetInteger("State", 3);
            Die();
        }


		//Keep camera in position, needs work
		if (!isGrounded)
		{
			camUpDown.enabled = false;
		}

    }

    public virtual void Move(float horzontalInput){
		//myBody.isKinematic = true;
		if (!dead)
		{
			Vector2 moveVel = myBody.velocity;
			moveVel.x = horzontalInput * speed;
			myBody.velocity = moveVel;

		}
	}

	public void Jump()
	{
		source.PlayOneShot (jumpSound);
		myBody.velocity = Vector2.up * jumpvelocity;
		lastJumpTime = Time.time;
		isGrounded = false;
	}


	bool CheckGrounded()
	{
		if (groundCollider.IsTouchingLayers (groundLayers)) {
			return true;
		}

		return false;

 
	}

	bool PermissionToJump ()
	{
		bool wasJustgrounded = Time.time < lostGroundingTime + groundingTolerance;
		bool hasJustJumped = Time.time < lastJumpTime + groundingTolerance + Time.deltaTime;
		return (isGrounded || wasJustgrounded) && !hasJustJumped;
	}

	bool CheckJumpInput ()
	{
		if (Input.GetButtonDown("Jump")) {
			lastInputJump = Time.time;
			return true;
		}
		if (Time.time < lastInputJump + jumpingTolerance) {
			return true;
		}
		return false;
	}

	public void CheckDeath() {
		if (transform.position.y < threshold) {
            dead = true;
		}
	}

	public void Die()
	{
		dead = true;
        StartCoroutine(DeathAnim(1.4f));
		thisCollider.enabled = false;
    }


	public void SetCheckpoint(Vector2 pos) {
		respawnPos = pos;
	}

	// Respawns the player
	public void Respawn() {
        if (dead == true)
        {
			deathCount++;
		//	Debug.Log ("Death Count" + deathCount);
            transform.position = respawnPos;
            myBody.velocity = new Vector2(0, 0);
            dead = false;
            this.GetComponent<SpriteRenderer>().enabled = true;
            score.SubtractScore(50);
        }
        this.GetComponent<SpriteRenderer>().enabled = true;
		thisCollider.enabled = true;
    }

    // Detects collision with the moving platform
    void OnCollisionEnter2D(Collision2D other)
    {
		if (other.gameObject.tag == "MovingPlatform")
        {
			transform.parent = other.transform;
			isGrounded = true;
        }

    }

    // Detects continous collision with the moving platform
    void OnCollisionStay2D(Collision2D hit) {
		if (hit.gameObject.tag == "MovingPlatform") {
			transform.parent = hit.transform;
			isGrounded = true;
		}
	}

    // Detects collision exit with the moving platform
    void OnCollisionExit2D(Collision2D exit) {

		if (exit.gameObject.tag == "MovingPlatform")
        {
            transform.parent = null;
			isGrounded = false;
        }
    }

	IEnumerator SoundDelay(float time)
	{
		yield return new WaitForSecondsRealtime (time);
		canMakeThud = true;
	}

    void directionOfSprite()
    {
        //check which way player is 
		if (Input.anyKey && !dead)
        {
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");
            Vector2 movement = new Vector2(horizontal, vertical);
            movement.Normalize();
            float angle = Mathf.Atan2(movement.y, movement.x);
            if (angle > ThreePiOverFour || angle < -ThreePiOverFour)
                faceDirection = 1;
            else if (angle < PiOverFour && angle > -PiOverFour)
                faceDirection = 0;
        }
    }

	IEnumerator SpawnDelay(float time)
	{
		yield return new WaitForSecondsRealtime (time);
		Respawn ();
	}
    IEnumerator DeathAnim(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        this.GetComponent<SpriteRenderer>().enabled = false;
        //this.gameObject.SetActive (false);
        StartCoroutine(SpawnDelay(spawnDuration));
        system.ResetRope();
        Respawn();
    }
}
