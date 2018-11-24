using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    // Public Varibles
    public GameObject player;

    // Private Variables
    private Vector3 offset;         // To store the offset distance between the player and camera

    private void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
