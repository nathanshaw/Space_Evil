using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // TODO - this is hard coded for now but
    // this should be gotten from the player properties
    float movementSpeed;

    bool moveUp;
    bool moveDown;
    bool moveRight;
    bool moveLeft;

    bool fire_primary;
    bool fire_bomb;

    KeyCode upKey;
    KeyCode downKey;
    KeyCode leftKey;
    KeyCode rightKey;

    KeyCode primaryFireKey;
    KeyCode secondaryFireKey;

    // Start is called before the first frame update
    void Start()
    {
        // TODO
        // this should instead be chosen be player in options somehow?
        upKey = KeyCode.UpArrow;
        downKey = KeyCode.DownArrow;
        leftKey = KeyCode.LeftArrow;
        rightKey = KeyCode.RightArrow;

        primaryFireKey = KeyCode.Space;
        secondaryFireKey = KeyCode.LeftControl;

        movementSpeed = 10;
    }

    // Update is called once per frame
    void Update()
    {
        moveDown = Input.GetKey(downKey);
        moveUp = Input.GetKey(upKey);
        moveLeft = Input.GetKey(leftKey);
        moveRight = Input.GetKey(rightKey);
    }

    private void FixedUpdate() {
        Vector2 pos = transform.position;

        float moveAmount = movementSpeed * Time.fixedDeltaTime;

        Vector2 move = Vector2.zero;

        if (moveUp){
            move.y += moveAmount;
        }

        if (moveDown) {
            move.y -= moveAmount;
        }

        if (moveLeft) {
            move.x -= moveAmount;
        }

        if (moveRight) {
            move.x += moveAmount;

        }

        transform.position = pos;
        // TODO - cool,  but need to add fireing back in
    }

}
