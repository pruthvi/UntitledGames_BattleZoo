using UnityEngine;

public class CameraFollow : MonoBehaviour {
    // Player game object transform
    public Transform target;

    // Distance between player and camera
    private Vector3 offset = new Vector3(0, 0, -1);

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.transform.position + offset;
        }
    }
}
