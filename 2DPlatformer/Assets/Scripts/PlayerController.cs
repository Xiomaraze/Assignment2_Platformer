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
    List<RaycastHit2D> contacts = new List<RaycastHit2D>();
    //public GameObject groundObject; this was just testing to see what the tag for the ground came out as to avoid spelling mistakes
    //Rigidbody2D ground;

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(playerInput);
        }

        MovementUpdate(playerInput);
    }

    private void MovementUpdate(Vector2 moveInput)
    {

        rb2.AddForce(moveInput * Speed * gravityDrag);
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
    {
        int grounded = 0;
        rb2.Cast(Vector2.down, contacts, Mathf.Infinity);
        foreach (RaycastHit2D contact in contacts)
        {
            if (contact)
            {
                if (contact.collider.CompareTag("Ground"))
                {
                    grounded++;
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
