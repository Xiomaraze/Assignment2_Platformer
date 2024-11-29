using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
    public enum FacingDirection
    {
        left, right
    }

    Rigidbody2D rb2;
    BoxCollider2D playerCollider;
    Vector2 playerInput = Vector2.zero; //im specifically moving the player input variable from inside fixedupdate to here to keep consistent track of the last input the player used. sorry keely
    public float Speed;
    float gravityDrag = 4f;
    List<RaycastHit2D> contacts = new List<RaycastHit2D>(); //used to check if the player is touching the ground
    //public GameObject groundObject; this was just testing to see what the tag for the ground came out as to avoid spelling mistakes
    //Rigidbody2D ground;
    //week 11 variables below
    public float apexHeight;
    Vector2 apexMax = Vector2.zero;
    Vector2 jumpStart;
    Vector2 jumpMax;
    public float apexTime;
    Vector2 jumpSpeed;
    public bool jumpMaxed = false;

    // Start is called before the first frame update
    void Start()
    {
        rb2 = gameObject.GetComponent<Rigidbody2D>();
        playerCollider = gameObject.GetComponent<BoxCollider2D>();
        //ground = groundObject.GetComponent<Rigidbody2D>();
        //Debug.Log(ground.tag);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //The input from the player needs to be determined and then passed in the to the MovementUpdate which should
        //manage the actual movement of the character.

        if (Input.GetKey(KeyCode.A))
        {
            playerInput = new Vector2(0, playerInput.y);
            playerInput = playerInput + Vector2.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            playerInput = new Vector2(0, playerInput.y);
            playerInput = playerInput + Vector2.right;
        }
        else
        {
            playerInput = new Vector2(0, playerInput.y);
        }
        playerInput = new Vector2(playerInput.x * Speed * gravityDrag, playerInput.y);

        if (Input.GetKey(KeyCode.Space))
        { 
            if (apexHeight == 0) //not me out here giving the program a hernia because i forgot to assign these for testing
            {
                Debug.Log("please assign a maximum jump height in the player inspector");
            }
            if (apexTime == 0)
            {
                Debug.Log("please assign a time required to reach maximum jump height in the player inspector");
            }
            if (IsGrounded())
            {
                rb2.gravityScale = 0f;
                jumpStart = rb2.position;
                apexMax = new Vector2(0, apexHeight);
                jumpMax = jumpStart + apexMax;
                Debug.Log(jumpMax);
                jumpSpeed = new Vector2(0, apexHeight / apexTime);
                playerInput = new Vector2 (playerInput.x, jumpSpeed.y);
            }
            else
            {
                //nothing happens when trying to jump while in the air
            }
        }

        if (!IsGrounded()) //checks if the player is touching the ground
        {
            if (rb2.velocity.y > 0f) // checks if the player is moving upwards
            {
                if (jumpMax.y <= rb2.position.y) //checks if the player has reached the max jump height based on where they started their jump
                {
                    jumpMaxed = true;
                    rb2.gravityScale = 1f;
                    rb2.velocity = new Vector2(rb2.velocity.x, 0f);
                    playerInput = new Vector2(playerInput.x, 0f);
                }
            }
        }

        MovementUpdate(playerInput);
    }

    private void MovementUpdate(Vector2 moveInput)
    {
        Vector2 leftright = new Vector2(moveInput.x, 0);
        rb2.AddForce(leftright);
        Vector2 jump = new Vector2(0, moveInput.y);
        rb2.AddForce(jump, ForceMode2D.Impulse);
    }

    public bool IsWalking()
    {
        if (rb2.velocity.x != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsGrounded()
    {//jumping complicates things huh
        int grounded = 0;
        rb2.Cast(Vector2.down, contacts);
        foreach (RaycastHit2D contact in contacts)
        {
            if (contact)
            {
                if (contact.collider.CompareTag("Ground"))
                {
                    if (contact.distance < 0.01f) { grounded++; } //finally got the jump height to work right by checking if the distance between the collider and the raycast hit is absolutely miniscule
                }
            }
        }

        if (grounded > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public FacingDirection GetFacingDirection()
    {
        if (IsWalking() && rb2.velocity.x < 0)
        {
            return FacingDirection.left;
        }
        else if (IsWalking() && rb2.velocity.x > 0)
        {
            return FacingDirection.right;
        }
        else return FacingDirection.right;
    }
}
