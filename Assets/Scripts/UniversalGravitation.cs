using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalGravitation : MonoBehaviour {

	public PhysicsEngine[] physicsEngineArray;
	const float bigG = 6.67408e-11f;  // m^3⋅kg^−1⋅s^−2;

	void Start () {
		physicsEngineArray = GameObject.FindObjectsOfType<PhysicsEngine> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		CalculateGravity ();
	}

	void CalculateGravity () {
		physicsEngineArray = GameObject.FindObjectsOfType<PhysicsEngine> ();
		foreach (PhysicsEngine physicsEngineA in physicsEngineArray) {
			foreach (PhysicsEngine physicsEngineB in physicsEngineArray) {
				if (physicsEngineA != physicsEngineB ) {
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
}
