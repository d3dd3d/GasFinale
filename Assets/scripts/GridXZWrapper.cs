using System;
using System.Collections.Generic;
using UnityEngine;

public class GridXZWrapper
{
    public Vector3Wrapper entranceWaypoint;
    public Vector3Wrapper exitWaypoint;
    public Vector3Wrapper serviceEntranceWaypoint;
    public Vector3Wrapper serviceExitWaypoint;
    public List<Vector3Wrapper> fuelDispencerWaypoints;
    public List<Vector3Wrapper> fuelTankWaypoints;
    public Vector3Wrapper storeWaypoint;
    public GridXZData[] topology;

    public GridXZWrapper(GridXZ grid, GridXZ serviceGrid, GridXZ roadGrid, Vector3 entranceWaypoint, Vector3 exitWaypoint, Vector3 serviceEntranceWaypoint, Vector3 serviceExitWaypoint, List<Vector3> fuelDispencerWaypoints, List<Vector3> fuelTankWaypoints, Vector3 storeWaypoint)
    {
        topology = new GridXZData[] { new GridXZData(grid), new GridXZData(serviceGrid), new GridXZData(roadGrid) };
        this.entranceWaypoint = new Vector3Wrapper(entranceWaypoint);
        this.exitWaypoint = new Vector3Wrapper(exitWaypoint);
        this.serviceEntranceWaypoint = new Vector3Wrapper(serviceEntranceWaypoint);
        this.serviceExitWaypoint = new Vector3Wrapper(serviceExitWaypoint);
        this.storeWaypoint = new Vector3Wrapper(storeWaypoint);
        this.fuelDispencerWaypoints = new List<Vector3Wrapper>();
        for (int i = 0; i < fuelDispencerWaypoints.Count; i++)
        {
            this.fuelDispencerWaypoints.Add(new Vector3Wrapper(fuelDispencerWaypoints[i]));
        }
        this.fuelTankWaypoints = new List<Vector3Wrapper>();
        for (int i = 0; i < fuelTankWaypoints.Count; i++)
        {
            this.fuelTankWaypoints.Add(new Vector3Wrapper(fuelTankWaypoints[i]));
        }
    }
    [Serializable]
    public class Vector3Wrapper
    {
        public float[] coordinates;
        public Vector3Wrapper(Vector3 vector)
        {
            coordinates = new float[3] { vector.x, vector.y, vector.z };
        }
    }
}