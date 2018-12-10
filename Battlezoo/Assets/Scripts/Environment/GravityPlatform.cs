using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPlatform : Platform
{

    public float fallDistance = 5;
    public float correctSpeed = 5;
    private Rigidbody2D rb2d;

    private Vector3 startPosition;

    public float speed = 5;
    public float distance = 5;
    public GravityPlatformState gravityPlatformState = GravityPlatformState.Up;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        rb2d.isKinematic = true;
        rb2d.gravityScale = 0;
        startPosition = transform.localPosition;
    }

    void Update()
    {
        OnPlatformMoving();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            platformState = PlatformState.Active;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            platformState = PlatformState.Return;
        }
    }

    protected override void OnPlatformMoving()
    {
        if (platformState == PlatformState.Return && Vector3.Distance(transform.localPosition, startPosition) < 0.5f)
        {
            platformState = PlatformState.Idle;
            rb2d.gravityScale = 0;
        }

        // Do nothing if idle
        if (platformState == PlatformState.Idle)
        {
            return;
        }

        Vector3 targetLocation = startPosition;

        // If active move towards target Location based on the gravity platform state
        if (platformState == PlatformState.Active)
        {
            if (gravityPlatformState == GravityPlatformState.Up)
            {
                targetLocation = startPosition + Vector3.up * distance;
            }
            else if (gravityPlatformState == GravityPlatformState.Down)
            {
                targetLocation = startPosition + Vector3.down * distance;
            }
        }

        float step = speed * Time.deltaTime;

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetLocation, step);
    }
}