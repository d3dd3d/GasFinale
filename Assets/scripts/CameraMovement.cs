using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) pos.z += 1f;
        if (Input.GetKey(KeyCode.A)) pos.x -= 1f;
        if (Input.GetKey(KeyCode.S)) pos.z -= 1f;
        if (Input.GetKey(KeyCode.D)) pos.x += 1f;

        float moveSpeed = 50;

        transform.position += pos * moveSpeed * Time.deltaTime;
    }
}
