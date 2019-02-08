using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RopeSystem : MonoBehaviour {


	public GameObject ropeHingeAnchor;
	public DistanceJoint2D ropeJoint;
	public Transform crosshair;
	public SpriteRenderer crosshairSprite;
	public LineRenderer crosshairLine;
	RaycastHit2D hit;
	public PlayerController playerMovement;

	//booleans
	private bool distanceSet;
	private bool ropeAttached;


	public static float playerStamina = 15f;
	public float stamInterval = 1f;
	public GameObject staminaBar;


	//Climbing variables
	public float climbSpeed  = 4f;
	private bool isColliding;

	private Vector2 playerPosition;
	private Rigidbody2D ropeHingeAnchorRb;
	private SpriteRenderer ropeHingeAnchorSprite;


	//Rope Stuff
	public LineRenderer ropeRenderer;
	public LayerMask ropeLayerMask;
	private float ropeMaxCastDistance = 20f;
	private List<Vector2> ropePositions = new List<Vector2>();

	private AudioSource source;
	public AudioClip clang;
	public AudioClip empty;


	private int timesHooked = 0;




	// Use this for initialization
	void Awake () 
	{
		ropeJoint.enabled = false;
		playerPosition = transform.position;
		ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D> ();
		ropeHingeAnchorSprite = ropeHingeAnchor.GetComponent<SpriteRenderer> ();
		source = this.GetComponent<AudioSource> ();
	}

	void Update()
	{
		//Aiming part
		var worldMousePosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0f));
		var facingDirection = worldMousePosition - transform.position;
		var aimAngle = Mathf.Atan2 (facingDirection.y, facingDirection.x);
		if (aimAngle < 0f)
		{
			aimAngle = Mathf.PI * 2 + aimAngle;
		}

		var aimDirection = Quaternion.Euler (0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;

		playerPosition = transform.position;

		if (!ropeAttached)
		{
			if (Time.timeScale != 0)
			SetCrosshairPosition(aimAngle);
			playerMovement.isSwinging = false;

		} else
		{
			crosshairSprite.enabled = false;
			playerMovement.isSwinging = true;
			playerMovement.ropeHook = ropePositions.Last();
		}

		if (ropeAttached && !playerMovement.isGrounded)
		{
			staminaBar.SetActive (true);
			playerStamina -= stamInterval * Time.deltaTime;
			//Debug.Log (playerStamina);
		}

		if (Time.timeScale != 0)
		{
			//Calls HandleInput every frame
			HandleInput(aimDirection);

			UpdateRopePositions();

			HandleRopeLength ();
		}
	
	}

	// Code to set Crosshair position
	private void SetCrosshairPosition(float aimAngle)
	{
		if (!crosshairSprite.enabled)
		{
			crosshairSprite.enabled = true;
		}
		// float value changes radius size
		var x = transform.position.x + 5f * Mathf.Cos(aimAngle);
		var y = transform.position.y + 5f * Mathf.Sin(aimAngle);

		var crossHairPosition = new Vector3(x, y, 0);
		crosshair.transform.position = crossHairPosition;


	}


	// 1

	private void HandleInput(Vector2 aimDirection)
	{
		//Left mouse click
		if (Input.GetMouseButtonDown(0) && !playerMovement.dead)
		{
			
			
			if (ropeAttached) return;
			ropeRenderer.enabled = true;


			var hit = Physics2D.Raycast(playerPosition, aimDirection, ropeMaxCastDistance, ropeLayerMask);
			//if (timesHooked == 0)
			//	ropeJoint.distance = Vector2.Distance(playerPosition, hit.point) / 2;
			//Pointer appears when raycast hits on a surface tagged with grapple
			if (hit.collider != null && hit.collider.tag.Equals("Grapple"))
			{
				source.PlayOneShot (clang);
				ropeAttached = true;
				if (!ropePositions.Contains(hit.point))
				{
					// Brings the player half way between surface and them
					ropeJoint.distance = Vector2.Distance(playerPosition, hit.point) / 2f;
				

					ropePositions.Add(hit.point);
				//	Debug.Log (ropeJoint.distance);
					ropeJoint.enabled = true;
					ropeHingeAnchorSprite.enabled = true;


			
				}
			}
			// 5
			else
			{
				source.PlayOneShot (empty);
				ropeRenderer.enabled = false;
				ropeAttached = false;
				ropeJoint.enabled = false;
			}
			timesHooked++;
		}
			

		//Right mouse click
		if (Input.GetMouseButtonDown(1) || Input.GetButtonDown("Jump") || playerStamina <= 0)
		{
			ResetRope();
		}
	}

	// 6
	public void ResetRope()
	{
		staminaBar.SetActive (false);
		ropeJoint.enabled = false;
		ropeAttached = false;
		playerMovement.isSwinging = false;
		playerStamina = 15f;
		ropeRenderer.positionCount = 2;
		ropeRenderer.SetPosition(0, transform.position);
		ropeRenderer.SetPosition(1, transform.position);
		ropePositions.Clear();
		ropeHingeAnchorSprite.enabled = false;
	}



	private void UpdateRopePositions()
	{
		// 1
		if (!ropeAttached)
		{
			return;
		}


		// 2
		ropeRenderer.positionCount = ropePositions.Count + 1;

		// 3
		for (var i = ropeRenderer.positionCount - 1; i >= 0; i--)
		{
			if (i != ropeRenderer.positionCount - 1) // if not the Last point of line renderer
			{
				ropeRenderer.SetPosition(i, ropePositions[i]);

				// 4
				if (i == ropePositions.Count - 1 || ropePositions.Count == 1)
				{
					var ropePosition = ropePositions[ropePositions.Count - 1];
					if (ropePositions.Count == 1)
					{
						ropeHingeAnchorRb.transform.position = ropePosition;
						if (!distanceSet)
						{
							ropeJoint.distance = Vector2.Distance(transform.position, ropePosition) / 2;
							distanceSet = true;
						}
					}
					else
					{
						ropeHingeAnchorRb.transform.position = ropePosition;
						if (!distanceSet)
						{
							ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
							distanceSet = true;
						}
					}
				}
				// 5
				else if (i - 1 == ropePositions.IndexOf(ropePositions.Last()))
				{
					var ropePosition = ropePositions.Last();
					ropeHingeAnchorRb.transform.position = ropePosition;
					if (!distanceSet)
					{
						ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
						distanceSet = true;
					}
				}
			}
			else
			{
				// 6
				ropeRenderer.SetPosition(i, transform.position);
			}
		}
	}


	private void HandleRopeLength()
	{
		if (Input.GetAxis ("Vertical") >= 1f && ropeAttached && !isColliding)
		{
			ropeJoint.distance -= Time.deltaTime * climbSpeed;
		} else if (Input.GetAxis ("Vertical") < 0f && ropeAttached)
		{
			ropeJoint.distance += Time.deltaTime * climbSpeed;
		}

		//Rope cutoff at 20 units
		if (ropeJoint.distance >= 20)
			ResetRope ();
	}


	void OnTriggerStay2D(Collider2D colliderStay)
	{
		isColliding = true;
	}

	private void OnTriggerExit2D(Collider2D colliderOnExit)
	{
		isColliding = false;
	}
	//  Code to get hook to wrap around platforms, probably dont need?
	/*
	private Vector2 GetClosestColliderPointFromRaycastHit(RaycastHit2D hit, PolygonCollider2D boxCollider)
	{
		

		var distanceDictionary = boxCollider.points.ToDictionary<Vector2, float, Vector2>(
			position => Vector2.Distance(hit.point, boxCollider.transform.TransformPoint(position)), 
			position => boxCollider.transform.TransformPoint(position));


		var orderedDictionary = distanceDictionary.OrderBy(e => e.Key);
		return orderedDictionary.Any() ? orderedDictionary.First().Value : Vector2.zero;
	}
*/

}
