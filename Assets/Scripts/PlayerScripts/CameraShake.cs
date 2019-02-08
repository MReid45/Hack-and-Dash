using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	public Animator anim;
	Animation camAnimation;

	public Camera cam;
	float camSize;
	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator> ();
		camAnimation = this.GetComponent<Animation> ();
		cam = this.GetComponent<Camera> ();
		camSize = cam.orthographicSize;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartCameraShake()
	{
		
		cam.orthographicSize = camSize - .02f;
		StartCoroutine(SetOriginal(.05f));
     //   anim.SetInteger("Jump", 1);
		//anim.Play (1);

    }


	IEnumerator SetOriginal(float delay)
	{
		yield return new WaitForSecondsRealtime (delay);
		cam.orthographicSize = camSize;
	}
}
