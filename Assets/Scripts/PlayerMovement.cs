using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D baseRb;

    [Space(25)]

    [SerializeField, Range(3,15)] float moveForce = 10;
    [SerializeField, Range(50,500)] float jumpForce = 10;
    [SerializeField, Range(3,10)] float maxVelocity = 10;

    [Space(25)]

    [SerializeField] float moveMultiplier = 2;
    [SerializeField] float jumpMultiplier = 2;

    [Space(25)]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckRadius = 0.1f;
    [SerializeField] float groundCheckOffset = 1;

    private bool isJumping = false;

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = Vector2.zero;
        isJumping = false;

        if (baseRb.linearVelocity.magnitude <= maxVelocity)
            inputVector.x = Input.GetAxisRaw("Horizontal");

        isJumping = Input.GetButtonDown("Jump");

        if(isJumping && IsGrounded())
        {
            inputVector.y = jumpForce;
        }

        baseRb.AddForce(inputVector * moveForce, ForceMode2D.Force);
    }
    private bool IsGrounded() => Physics2D.OverlapCircle(baseRb.transform.position + Vector3.up * groundCheckOffset, groundCheckRadius, groundLayer);
    public void UpgradeStats(int connectedBodyAmount)
    {
        moveForce = moveForce + connectedBodyAmount * moveMultiplier;
        jumpForce = jumpForce + connectedBodyAmount * jumpMultiplier;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(baseRb.transform.position + Vector3.up * groundCheckOffset, groundCheckRadius);
    }
}