using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {

    public enum TravelMode { TimeBased, DistanceBased }
    public enum OnHit { Destory, Pass, Bounce }

    public TravelMode travelMode;
    public OnHit onHit;

    public float maxTimeTravel = 2;
    public float maxDistanceToTravel = 10;
    private Vector2 startPoint;

	// Use this for initialization
	void Start () {
        switch (travelMode)
        {
            case TravelMode.TimeBased:
                Destroy(gameObject, maxTimeTravel);
                break;
            case TravelMode.DistanceBased:
                startPoint = transform.position;
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (travelMode == TravelMode.DistanceBased)
        {
            if (Vector2.Distance(transform.position, startPoint) >= maxDistanceToTravel)
            {
                Destroy(gameObject);
            }
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (onHit)
        {
            case OnHit.Destory:
                Destroy(gameObject);
                break;
            case OnHit.Bounce:
                // Reset the startPoint so that if bullet bounce off object it will only travel the maxDistanceToTravel
                startPoint = transform.position;
                GetComponent<Rigidbody2D>().velocity = -GetComponent<Rigidbody2D>().velocity;
                break;
        }
    }
}
