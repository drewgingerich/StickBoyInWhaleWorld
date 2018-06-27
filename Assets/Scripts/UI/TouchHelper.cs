﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helpers made by Matthew Conto
/// </summary>
public class TouchHelper {
	/// <summary>
	/// Returns the World Position of the finger touch. Note, if there is no background, the touch will be pointed to the max render distance of the camera, leading to strange results.
	/// </summary>
	/// <returns>Vector3 in world space. You can ignore the `.z` portion of the Vector3 to get the Vector2.</returns>
	public static Vector3 GetFingerWorldPosition(Touch touch) {
		Vector3 processed = Camera.main.ScreenToWorldPoint( touch.position );
		return processed;
	}

	/// <summary>
	/// Rotates the original GameObject to face the target worldposition
	/// </summary>
	/// <param name="original">Object to rotate</param>
	/// <param name="target">Location to face in worldspace</param>
	public static void RotateToFace2D( Transform original, Vector3 target ) {
		Vector3 vectorToTarget = original.position - target;
		float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
		original.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	/// <summary>
	/// Rotates using **RigidBody Physics** the original GameObject to face the target worldposition
	/// </summary>
	/// <param name="original">Rigidbody2D of GameObject to rotate</param>
	/// <param name="target">Location to face in worldspace</param>
	public static void RotateToFace2D( Rigidbody2D original, Vector3 target ) {
		Vector3 vectorToTarget = original.transform.position - target;
		float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
		original.MoveRotation( angle );
	}
}