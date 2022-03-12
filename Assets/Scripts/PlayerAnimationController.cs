using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    #region Serialized fields
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;
    #endregion

    #region private fields
    private float movementBlendValue;
    private Vector3 direction;
    private bool isForward;
    private bool isBackward;
    #endregion

    #region public fields
    [HideInInspector] public bool canMove;
    #endregion

    private void Start()
    {
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        direction = playerController.direction;
        isBackward = direction.z < 0;
        isForward = direction.z > 0 || direction.x != 0;
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerController.isGrounded)
            animator.SetTrigger(AnimationHashValues.JUMP_ANIMATION);
        if (Input.GetKeyDown(KeyCode.E) && playerController.isPickable)
            animator.SetTrigger(AnimationHashValues.PICKUP_ANIMATION);
        ControlLocomotion(isForward, isBackward, direction);
    }

    private void ControlLocomotion(bool isForward, bool isBackward, Vector3 direction)
    {
        if (isForward)
        {
            movementBlendValue = 0.5f;
        }
        if (isBackward)
        {
            movementBlendValue = -0.5f;
        }
        if (isForward && playerController.isRunning)
        {
            movementBlendValue = 1;
        }
        if (isBackward && playerController.isRunning)
        {
            movementBlendValue = -1;
        }
        if (direction.magnitude == 0)
        {
            movementBlendValue = 0;
        }
        animator.SetFloat(AnimationHashValues.LOCOMOTION_BLEND_TREE, movementBlendValue);
    }

    private void CannotMove()
    {
        canMove = false;
    }

    private void CanMove()
    {
        canMove = true;
    }
}
