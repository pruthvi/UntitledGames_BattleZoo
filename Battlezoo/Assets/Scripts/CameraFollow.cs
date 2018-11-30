using UnityEngine;

public class CameraFollow : MonoBehaviour {

    //// private Varibles
    //private GameObject gameCamera;       // Get the Game camera

    //private Vector3 offset;         // To store the offset distance between the player and camera

    //private void Start()
    //{
    //    gameCamera = GameObject.FindGameObjectWithTag("MainCamera");    // Assigning MainCamera

    //    // Focusing the Camera on current Player
    //    gameCamera.transform.position = new Vector3(transform.position.x, transform.position.y, gameCamera.transform.position.z);

    //    offset = gameCamera.transform.position - transform.position;
    //}

    //void LateUpdate()
    //{
    //    gameCamera.transform.position = transform.position + offset;
    //}


    // Player game object
    public GameObject target;

    // Distance between player and camera
    private Vector3 offset = new Vector3(0, 0, -10);

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.transform.position + offset;
        }
    }
}
