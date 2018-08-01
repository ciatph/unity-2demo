using UnityEngine;
using System.Collections;

// Unity checks if a component is attached, else it creates one
[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour {

	public int offsetX = 2;			// offset so that we don't get any weird errors
	
	// these are used for checking if we need to instantiate stuff
	public bool hasARightBuddy = false;		
	public bool hasALeftBuddy = false;
	
	public bool reverseScale = false;	// used if object is not tilable`
	
	private float spriteWidth = 0f;		// wdth of our element
	private Camera cam;
	private Transform myTransform;

	void Awake () {
		cam = Camera.main;
		myTransform = transform;
	}
	
	// Use this for initialization
	void Start () {
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer>(); 
		spriteWidth = sRenderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {
		// does it still need buddies? If not do nothing
		if(hasALeftBuddy == false || hasARightBuddy == false){
			// calculate the length of the center of the camera to "b" (rightmost viewable side of camera)
			// calculate the camera's extend (half the width) of what the camera can see in world coordinates
			float camHorizontalExtend = cam.orthographicSize * Screen.width/Screen.height;
			
			// calculate the x pos where cam can see the edge of sprite
			float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth/2) - camHorizontalExtend;
			float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth/2) + camHorizontalExtend;
		
			// check if we can see the edge of the element and then calling MakeNewBuddy if we can
			if(cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false){
				MakeNewBuddy(1);
				hasARightBuddy = true;
			}
			else if(cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBuddy == false){
				MakeNewBuddy(-1);
				hasALeftBuddy = true;
			}
		}
	}
	
	// A function that creates a buddy on the side required
	void MakeNewBuddy (int rightOrLeft) {
		// calculate the new position for my new buddy
		Vector3 newPosition = new Vector3 (myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
		
		// instantiate our new buddy and store him in a variable
		Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;
		
		// if not tileable let's reverse the x size of out object to get rid of ugly seams
		if(reverseScale == true){
			newBuddy.localScale = new Vector3(newBuddy.localScale.x *- 1, newBuddy.localScale.y, newBuddy.localScale.z);
		}
		
		// set the parent of newly-spawned objects to a GameObject to tidy things up
		newBuddy.parent = myTransform.parent; 
		
		if(rightOrLeft > 0){
			newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
		}
		else{
			newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
		}
	}
}
