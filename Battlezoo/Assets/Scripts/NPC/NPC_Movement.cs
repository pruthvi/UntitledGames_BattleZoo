using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class NPC_Movement : MonoBehaviour {

    // Public Variables
    public float speed = 15f;        // Walking Speed
    public LayerMask whatIsGround;
    public Transform groundCheck;

    // Private Variables
    private Rigidbody2D rbody;
    private bool isWalkable;

    // Use this for initialization
    void Start () {
        rbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        // NPC walks at a constant Speed
        rbody.velocity = transform.position * speed * Time.deltaTime;

        // Walk in the opposite direction if the area is not walkable
        if (!isWalkable) { FlipNPC(); }
	}

    private void FixedUpdate()
    {
        // Check if the platform is walkable
        isWalkable = Physics2D.OverlapPoint(groundCheck.position, whatIsGround);
    }

    void FlipNPC()
    {
        speed = -speed;                 // Moving in Opposite Direction

        // Flipping the GameObject
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
