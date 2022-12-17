using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class does not inheriet from MonoBehaviour
// as it is not directly attached to any game object
// instead it should store the player stats that persist through levels
public class PlayerProperties {
	public PlayerShip ship;
	public string name;
	public int score;

	// 0 for player 1 and 1 for player 2
	public int playerID;
	// for keeping track of players where there is more than one?
	// Player Money / CP
	public int CPs;

	// Movement Speed should be set by the ship mass and engine power
	public float maxMovementSpeed;
	public float maxHitPoints;

	// Health / Shield / Armor / etc...
	public float currentHitPoints;
	public float healthPercent;


	public int UpdateCPs(int CP_Delta) {
		// TODO - should display on screen that the CP number has changed
		CPs += CP_Delta;
		return CPs;
	}

	public int UpdateScore(int Score_Delta) {
		score += Score_Delta;
		return score;
	}
};