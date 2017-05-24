using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PhysicsEngine : MonoBehaviour {

	public float mass;				// [kg]
	public Vector3 velocityVector;	// [m s^-1]
	public Vector3 netForceVector;	// N [kg m s^-2]
	public bool showTrails = true;	

	List<Vector3> forceVectorList = new List<Vector3>();
	LineRenderer lineRenderer;
	int numberOfForces;
	PhysicsEngine[] physicsEngineArray;
	const float bigG = 6.67408e-11f;  // m^3⋅kg^−1⋅s^−2;

	void Start () {
		SetupTrails ();
		physicsEngineArray = GameObject.FindObjectsOfType<PhysicsEngine> ();
	}

	void FixedUpdate () {
		RenderTrails ();
		CalculateGravity ();
		UpdatePosition ();
	}

	public void AddForce (Vector3 forceVector) {
		forceVectorList.Add (forceVector);
		Debug.Log ("Adding force " + forceVector + " to " + gameObject.name);
	}

	void CalculateGravity () {
		foreach (PhysicsEngine physicsEngineA in physicsEngineArray) {
			foreach (PhysicsEngine physicsEngineB in physicsEngineArray) {
				if (physicsEngineA != physicsEngineB && physicsEngineA != this) {
					Debug.Log ("Calculating force exerted on " + physicsEngineA.name +
					" due to the gravity of " + physicsEngineB.name);
					
					Vector3 offset = physicsEngineA.transform.position - physicsEngineB.transform.position;
					float gravityMagnitude = bigG * physicsEngineA.mass * physicsEngineB.mass / Mathf.Pow (offset.magnitude, 2f);
					Vector3 gravityVector = gravityMagnitude * offset.normalized;
					physicsEngineB.AddForce (gravityVector);
				}
			}
		}
	}

	void UpdatePosition () {
		netForceVector = Vector3.zero;
		foreach (Vector3 forceVector in forceVectorList) {
			netForceVector += forceVector;
		}

		forceVectorList = new List<Vector3> ();

		Vector3 accelerationVector = netForceVector / mass;
		velocityVector += accelerationVector * Time.deltaTime;
		transform.position += velocityVector * Time.deltaTime;
	}

	void SetupTrails () {
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
		//		lineRenderer.SetColors(Color.yellow, Color.yellow);
		lineRenderer.startColor = Color.yellow;
		lineRenderer.endColor = Color.yellow;
		//		lineRenderer.SetWidth(0.2F, 0.2F);
		lineRenderer.startWidth = 0.2f;
		lineRenderer.endWidth = 0.2f;
		lineRenderer.useWorldSpace = false;
	}

	void RenderTrails () {
		if (showTrails) {
			lineRenderer.enabled = true;
			numberOfForces = forceVectorList.Count;
			//			lineRenderer.SetVertexCount(numberOfForces * 2);
			lineRenderer.positionCount = numberOfForces * 2;
			int i = 0;
			foreach (Vector3 forceVector in forceVectorList) {
				lineRenderer.SetPosition(i, Vector3.zero);
				lineRenderer.SetPosition(i+1, -forceVector);
				i = i + 2;
			}
		} else {
			lineRenderer.enabled = false;
		}
	}
}
