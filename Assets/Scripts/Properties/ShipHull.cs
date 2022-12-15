using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Each ship object contains a hull
public class ShipHull {
	 
    // how many CP points does this cost?
	public float Cost;

    // used to determine how much the ship weights
    public int Mass;

	// ship colntains info about max speed

    // not sure what this does
	public GameObject Explosion;

	// back reference to the player...
	public int sourcePlayerID;

}