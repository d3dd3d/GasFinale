using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAreaScript : MonoBehaviour
{
    public GameObject car;
    private bool canSpawn = true;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canSpawn)
        {
           Instantiate(car, GetComponent<Transform>().position + Vector3.up * 4, Quaternion.Euler(0f, 180f, 0f));
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    canSpawn = false;
    //    SimulationSystem.Instance.setSpawnRestriction(false);
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    canSpawn = true;
    //    SimulationSystem.Instance.setSpawnRestriction(true);
    //}
}
