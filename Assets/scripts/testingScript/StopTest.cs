using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTest : MonoBehaviour
{
    Unit unit;
    
    public float velocity;
    Transform transform;
    private void Awake()
    {
        this.unit = GetComponent<Unit>();
        this.transform = GetComponentInParent<Transform>();
    }

    private void Update()
    {
        if (unit.noObstacle) {
            transform.position += new Vector3(velocity * Time.deltaTime, 0, 0);
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        unit.noObstacle = false;
    }
    private void OnTriggerExit(Collider other)
    {
        unit.noObstacle = true;
    }
}
