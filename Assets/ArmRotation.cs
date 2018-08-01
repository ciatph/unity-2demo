using UnityEngine;
using System.Collections;

public class ArmRotation : MonoBehaviour {

	public int rotationOffset = 90;
	
	// Update is called once per frame
	void Update () {
		// subtracting the position of the player from the mouse position
		Vector3 difference = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;		
		difference.Normalize ();	// normalizing the vector. Meaning that all the sum of the vector will be equal to 1.
	
		// Find the angle to rotate the arm given the difference of the player & mouse direction, then convert it to degrees
		float rotZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0f, 0f, rotZ + rotationOffset);
	}
}
