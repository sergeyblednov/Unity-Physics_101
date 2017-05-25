using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour {

	public float maxLaunchSpeed;
	public AudioClip launchSound;
	public AudioClip windupSound;
	public PhysicsEngine ballToLaunch;

	AudioSource audioSource;
	public float launchSpeed;
	float extraSpeedPerFrame;

	void Start () {
		audioSource = GetComponent<AudioSource> ();
		audioSource.clip = windupSound;
		extraSpeedPerFrame = (maxLaunchSpeed * Time.fixedDeltaTime) / audioSource.clip.length;
	}

	void OnMouseDown () {
		launchSpeed = 0;
		InvokeRepeating ("IncreaseLaunchSpeed", 0.5f, Time.fixedDeltaTime);
		audioSource.clip = windupSound;
		audioSource.Play ();
	}

	void OnMouseUp () {
		audioSource.Stop ();
		CancelInvoke ();
		audioSource.clip = launchSound;
		audioSource.Play ();

		PhysicsEngine ball = Instantiate (ballToLaunch) as PhysicsEngine;
		ball.transform.parent = GameObject.Find ("Launched Balls").transform;
		Vector3 launchVelocity = new Vector3 (1, 1, 0).normalized * launchSpeed;
		ball.velocityVector = launchVelocity;
	}

	void IncreaseLaunchSpeed () {
		if (launchSpeed <= maxLaunchSpeed) {
			launchSpeed += extraSpeedPerFrame;
		}
	}
}
