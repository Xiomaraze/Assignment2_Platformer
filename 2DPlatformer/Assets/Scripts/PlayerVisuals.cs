using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script manages updating the visuals of the character based on the values that are passed to it from the PlayerController.
/// NOTE: You shouldn't make changes to this script when attempting to implement the functionality for the W10 journal.
/// </summary>
public class PlayerVisuals : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer bodyRenderer;
    public PlayerController playerController;

    private int isWalkingHash, isGroundedHash, isDyingHash, isIdleHash;

    // Start is called before the first frame update
    void Start()
    {
        isWalkingHash = Animator.StringToHash("IsWalking");
        isGroundedHash = Animator.StringToHash("IsGrounded");
        isDyingHash = Animator.StringToHash("IsDying");
        isIdleHash = Animator.StringToHash("IsDying");
    }

    // Update is called once per frame
    void Update()
    {
        VisualsUpdate();
    }

    //It is not recommended to make changes to the functionality of this code for the W10 journal.
    private void VisualsUpdate()
    {
        animator.SetBool(isWalkingHash, playerController.IsWalking());
        animator.SetBool(isGroundedHash, playerController.IsGrounded());
        animator.SetBool(isDyingHash, playerController.IsDying());
        animator.SetBool(isIdleHash, playerController.IsIdle());
        switch (playerController.GetFacingDirection())
        {
            case PlayerController.FacingDirection.left:
                bodyRenderer.flipX = true;
                break;
            case PlayerController.FacingDirection.right:
            default:
                bodyRenderer.flipX = false;
                break;
        }
        switch (playerController.GetCharacterState())
        {
            case PlayerController.CharacterState.idle:
                //animator.CrossFade(isIdleHash, 0f);
                break;
            case PlayerController.CharacterState.walking:
                //animator.CrossFade(isWalkingHash, 0f);
                break;
            case PlayerController.CharacterState.jumping:
                //animator.CrossFade(isGroundedHash, 0f);
                break;
            case PlayerController.CharacterState.dead:
                //animator.CrossFade(isDyingHash, 0f);
                break;
        }
    }
}
