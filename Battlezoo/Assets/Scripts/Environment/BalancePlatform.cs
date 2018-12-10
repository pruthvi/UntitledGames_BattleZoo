using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancePlatform : Platform {

    public float maxTurnAngle;

    public Vector3 centerPoint;

    public BalancePlatformState balancePlatformState = BalancePlatformState.Balanced;

    // Use this for initialization
    void Start () {
        centerPoint = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        OnPlatformMoving();
	}

    protected override void OnPlatformMoving()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.position.x < centerPoint.x)
        {

        }
    }
}
