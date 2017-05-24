using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PhysicsEngine))]
public class RocketEngine : MonoBehaviour {

	public float fuelMass; 				// [kg]
	public float maxThrust;				// kN [kg m s^-2]
	[Range(0, 1f)]
	public float thrustPercent;			// [none]
	public Vector3 thrustUnitVector;	// [none]
	PhysicsEngine physicsEngine;
	float currentThrust;				// N

	void Start () {
		physicsEngine = GetComponent<PhysicsEngine> ();
		physicsEngine.mass += fuelMass;
	}

	void FixedUpdate () {
		if (fuelMass > FuelThisUpdate ()) {
			fuelMass -= FuelThisUpdate ();
			physicsEngine.mass -= FuelThisUpdate ();
			ExertForce ();
		} else {
			Debug.LogWarning ("Out of rocket fuel");
		}
	}
		
	float FuelThisUpdate () {
		float exhaustMassFlow;												// [kg s^-1]
		float effectiveExhaustVelocity;										// [m s^-1]
		effectiveExhaustVelocity = 4462f;
		exhaustMassFlow = currentThrust / effectiveExhaustVelocity;			// [kg m s^-2 / m s^-1] = [kg s^-1]

		return exhaustMassFlow * Time.deltaTime;							// [kg]
	}

	void ExertForce () {
		currentThrust = thrustPercent * maxThrust * 1000f;
		Vector3 thrustVector = thrustUnitVector.normalized * currentThrust;	// N
		physicsEngine.AddForce (thrustVector);
	}
}
