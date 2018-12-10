using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancePlatform : Platform {

    public float maxTurnAngle = 30;
    public float rotateSpeed = 50;

    public Vector3 centerPoint;

    public BalancePlatformState balancePlatformState = BalancePlatformState.Balanced;

    private Rigidbody2D rb2d;

    private float width;

    // Use this for initialization
    void Start () {
        centerPoint = transform.localPosition;
        rb2d = GetComponent<Rigidbody2D>();
        //Safe check
        maxTurnAngle = Mathf.Abs(maxTurnAngle) >= 90 ? 89 : Mathf.Abs(maxTurnAngle);
        width = GetComponent<BoxCollider2D>().bounds.extents.x;
	}
	
	// Update is called once per frame
	void Update () {
        OnPlatformMoving();
	}

    protected override void OnPlatformMoving()
    {
        if (platformState == PlatformState.Idle)
        {
            return;
        }

        if (platformState == PlatformState.Active)
        {
            float angle = transform.localEulerAngles.z < 90 ? transform.localEulerAngles.z : 360 - transform.localEulerAngles.z;
            if (angle >= maxTurnAngle)
            {
                rb2d.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePosition;
                platformState = PlatformState.Return;
            }
        }
        else if (platformState == PlatformState.Return)
        {
            transform.Rotate((transform.localEulerAngles.z < 90 ? Vector3.back : Vector3.forward) * rotateSpeed * Time.deltaTime);
            if (transform.localEulerAngles.z < 1)
            {
                platformState = PlatformState.Idle;
                rb2d.constraints = RigidbodyConstraints2D.FreezePosition;
            }
        }


    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            platformState = PlatformState.Active;
            rb2d.constraints = RigidbodyConstraints2D.FreezePosition;

        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            platformState = PlatformState.Return;
            rb2d.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }
}
