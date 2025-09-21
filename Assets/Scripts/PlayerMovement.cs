using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D baseRb;

    [Space(25)]

    [SerializeField, Range(3,15)] float moveForce = 10;
    [SerializeField, Range(50,500)] float jumpForce = 10;
    [SerializeField, Range(3,10)] float maxVelocity = 10;

    [SerializeField] Vector2 moveForceClamp = new Vector2(3, 15);
    [SerializeField] Vector2 jumpForceClamp = new Vector2(50, 500);

    [Space(25)]

    [SerializeField] float moveMultiplier = 2;
    [SerializeField] float jumpMultiplier = 2;

    [Space(25)]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckRadius = 0.1f;
    [SerializeField] float groundCheckOffset = 1;

    private bool isJumping = false;
    private ConnectionManager cm;
    private void Awake()
    {
        cm = GetComponent<ConnectionManager>();
        //cm.OnConnectionCountChanged += UpgradeStats;
    }

    private void Update()
    {
        HandleMovement();
    }
    public void DieAnimation()
    {
        baseRb.AddForce(Vector2.up * 70, ForceMode2D.Impulse);
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
    private void UpgradeStats(int connectedBodyAmount)
    {
        moveForce =  Mathf.Clamp(moveForce * connectedBodyAmount * moveMultiplier, moveForceClamp.x, moveForceClamp.y);
        jumpForce = Mathf.Clamp(jumpForce * connectedBodyAmount * jumpMultiplier, jumpForceClamp.x, jumpForceClamp.y);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(baseRb.transform.position + Vector3.up * groundCheckOffset, groundCheckRadius);
    }
}