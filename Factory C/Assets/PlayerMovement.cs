using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Unity.Netcode;


[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement : NetworkBehaviour
{
    private bool controllable;
    public bool Controllable
    {
        get
        {
            return controllable;
        }

        set
        {
            controllable = value;
            if (!value) rb.velocity = new Vector2(rb.velocity.x / 10, rb.velocity.y);
            onControllableUpdate?.Invoke();
        }
    }

    public Action onControllableUpdate;
    [SerializeField] private float movementSpeed = 14f;
    [SerializeField] private float jumpVelocity = 14f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D feetCollider;
    [Range(0f, 0.5f)]
    [SerializeField] private float coyoteThreshold = 0.1f; // time for which the player can still jump after leaving solid ground
    private float nonGroundedTime = 1.5f;
    private bool grounded;
    private Rigidbody2D rb;
    private Quaternion rotationGoal;
    private SpriteRenderer spriteRenderer;

    public NetworkVariable<bool> flipX = new(writePerm: NetworkVariableWritePermission.Owner);

    override public void OnNetworkSpawn()
    {
        rb = GetComponent<Rigidbody2D>();
        Controllable = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        flipX.OnValueChanged += (_, newValue) =>
        {
            spriteRenderer.flipX = newValue;
        };
    }

    private bool IsGrounded()
    {
        var colliders = new List<Collider2D>();
        var filter = new ContactFilter2D
        {
            useTriggers = false,
            useLayerMask = true,
            layerMask = groundLayer,
        };
        feetCollider.OverlapCollider(filter, colliders);
        return colliders.Count > 0;
    }

    private void UpdateNonGroundedTime()
    {
        if (IsGrounded()) nonGroundedTime = 0;
        else nonGroundedTime += Time.fixedDeltaTime;
    }

    void FixedUpdate()
    {
        if(!IsOwner) return;

        if (Controllable)
        {
            UpdateNonGroundedTime();
            float horizontalMove = Input.GetAxis("Horizontal");
            flipX.Value = horizontalMove < 0;
            float verticalMove = rb.velocity.y;
            if (Input.GetAxis("Jump") > 0 && nonGroundedTime <= coyoteThreshold)
            {
                verticalMove = jumpVelocity;
            }
            rb.velocity = new Vector2(horizontalMove * movementSpeed, verticalMove);
        }

        float offset = 0.45f;
        float rayLength = 1.25f;

        RaycastHit2D left = Physics2D.Raycast(transform.position - new Vector3(offset, 0), -Vector2.up, rayLength, groundLayer);
        RaycastHit2D right = Physics2D.Raycast(transform.position + new Vector3(offset, 0), -Vector2.up, rayLength, groundLayer);
        if (left.collider != null && right.collider != null)
        {
            if (left.normal == right.normal)
            {
                rotationGoal = Quaternion.Euler(0, 0, Mathf.Atan2(left.normal.y, left.normal.x) * Mathf.Rad2Deg - 90);
            }
            else
            {
                rotationGoal = Quaternion.Euler(0, 0, 0);
            }
        }
        else
        {
            rotationGoal = Quaternion.Euler(0, 0, 0);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationGoal, 0.3f);
    }
}
