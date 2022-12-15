using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 
public class PlayerShip {
	 
	 // calculated by added together the hull and weapons and weapon attachments
	 public int Mass;
	 public int MoveSpeed = 10;

	 ShipHull Hull;


    // not sure what this does
	public GameObject Explosion;

	// back reference to the player...
	public int sourcePlayerID;

}
