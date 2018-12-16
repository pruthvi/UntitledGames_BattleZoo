using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(PolygonCollider2D))]
//[RequireComponent(typeof(Rigidbody2D))]
public class NPC_Movement : MonoBehaviour {

    // Public Variables
    public float _walkingSpeed = 15f;           // Walking Speed
    public float _leftPos = 0.0f;       // Starting Position 
    public float _rightPos = 6.0f;      // Player can Patrol till Right Position
    public int _dir = 1;              // 1 for right & -1 for Left (FLIPPING)

    // Private Variables
    private bool isWalkable;
    private float _originalPos;         // Original Position
    private Vector2 walkDirection;

    // Power UP
    private int _randomPowerUp;

    public float _powerupLife = 10.0f;
  //  public PowerUpManager powerup;

    // Use this for initialization
    void Start () {

        // Storing Players original Positon
        this._originalPos = this.transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {

        // NPC walks at a constant Speed
        walkDirection.x = _walkingSpeed * Time.deltaTime;

        if ((_dir == 1) && transform.position.x >= _originalPos + _rightPos)
        {
            _dir = -1;      // Change Direction
            FlipNPC();      // Flip NPC
        }
        // Walk in the opposite direction
        else if ((_dir == -1) && transform.position.x <= _originalPos - _leftPos)
        {
            _dir = 1;       // Change Direction
            FlipNPC();      // Flip NPC
        }

        transform.Translate(walkDirection);
	}

    void FlipNPC()
    {
        // Moving in Opposite Direction
        _walkingSpeed = -_walkingSpeed;                 

        // Flipping the GameObject
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
  
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Destroy(gameObject);            // Destroy Bullet
            Destroy(other.gameObject);      // Destroy Scientist
        //    SpawnPowerUp();
        }
    }

    // private void SpawnPowerUp()
    // {
    //     Vector2 position = transform.position;
    //     if (powerup.powerUp.Length != 0)
    //     {
    //         // Spawn Random PowerUp
    //         _randomPowerUp = Random.Range(1, powerup.powerUp.Length);

    //         var randomPowerUp = powerup.powerUp[_randomPowerUp].Object;
    //         var powerupInstantiate = Instantiate(randomPowerUp, position, Quaternion.identity);

    //         Destroy(powerupInstantiate, _powerupLife);        // PowerUp Will Destroy in Certain Time
    //     }

    // }


}
