/// <summary>
/// Worked on by: Mark Porowicz
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {

    public float padForce = 1000;
    private Collision2D collision;
    private bool jumping = false;

    Transform platTransform;
	private AudioSource source;

    void Awake()
    {
        platTransform = this.GetComponent<Transform>();
		source = this.GetComponent<AudioSource> ();
    }

    // Detects object entering collision
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!jumping)
        {
            jumping = true;
            collision = coll;
			source.Play ();
        };
    }

    void Update()
    {
        if (jumping)
        {
			float angle = this.transform.rotation.z;
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();
           rb.velocity = new Vector3(0, 0, 0);
            rb.AddForce(new Vector2(0f, padForce));
            jumping = false;
        }

    }

}