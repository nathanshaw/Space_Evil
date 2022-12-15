using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties {
	public PlayerMovement movementController;
	public PlayerShip ship;
	public string name;
	public int score;
	// for keeping track of players where there is more than one?
};

public class PlayerOne : PlayerProperties {
	private int player_ID = 1;
}

public class PlayerTwo: PlayerProperties {
	private int player_ID = 2;
}