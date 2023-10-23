using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
	public CarType carType;
	public Transform target;
	public ObjectParseWrapper dbData;
	float speed = 40f;
	Vector3[] path;
	Vector3[] pointsOfInterest;
	GridXZ grid;
	int currentWaypointIndex;
    int targetIndex;
	public bool noObstacle = true;
	public float currentFuelAmount;
	public float currrentOrigin;
	
    private void Start()
    {
        switch (carType)
        {
			case CarType.simpleCar:
				pointsOfInterest = new Vector3[7];
				pointsOfInterest[0] = SimulationSystem.Instance.entranceWaypoint - new Vector3(0, 0, SimulationSystem.Instance.grid.GetCellSize());
				pointsOfInterest[1] = SimulationSystem.Instance.entranceWaypoint;
				pointsOfInterest[2] = SimulationSystem.Instance.getLeastBusyFD();
				pointsOfInterest[3] = SimulationSystem.Instance.storeWaypoint;
				pointsOfInterest[4] = SimulationSystem.Instance.exitWaypoint;
				pointsOfInterest[5] = SimulationSystem.Instance.exitWaypoint - new Vector3(0, 0, SimulationSystem.Instance.grid.GetCellSize());
				pointsOfInterest[6] = pointsOfInterest[6] - new Vector3(1000, 0, 0);
				grid = SimulationSystem.Instance.grid;
				break;
			case CarType.fuelTruck:
				pointsOfInterest = new Vector3[6];
				pointsOfInterest[0] = SimulationSystem.Instance.serviceEntranceWaypoint - new Vector3(0, 0, SimulationSystem.Instance.grid.GetCellSize());
				pointsOfInterest[1] = SimulationSystem.Instance.serviceEntranceWaypoint;
				System.Random rn = new System.Random();
				pointsOfInterest[2] = SimulationSystem.Instance.fuelTankWaypoints[rn.Next(0, SimulationSystem.Instance.fuelTankWaypoints.Count - 1)]; // �����
				pointsOfInterest[3] = SimulationSystem.Instance.serviceExitWaypoint;
				pointsOfInterest[4] = SimulationSystem.Instance.serviceExitWaypoint - new Vector3(0, 0, SimulationSystem.Instance.grid.GetCellSize());
				pointsOfInterest[5] = pointsOfInterest[5] - new Vector3(1000, 0, 0);
				grid = SimulationSystem.Instance.serviceGrid;
				break;
			case CarType.incassation:
				pointsOfInterest = new Vector3[5];
				pointsOfInterest[0] = SimulationSystem.Instance.entranceWaypoint - new Vector3(0, 0, SimulationSystem.Instance.grid.GetCellSize());
				pointsOfInterest[1] = SimulationSystem.Instance.entranceWaypoint;
				pointsOfInterest[2] = SimulationSystem.Instance.storeWaypoint;
				pointsOfInterest[3] = SimulationSystem.Instance.exitWaypoint;
				pointsOfInterest[4] = SimulationSystem.Instance.exitWaypoint - new Vector3(0, 0, SimulationSystem.Instance.grid.GetCellSize());
				pointsOfInterest[5] = pointsOfInterest[4] - new Vector3(1000, 0, 0);
				grid = SimulationSystem.Instance.grid;
				break;
		}
		currentWaypointIndex = 0;
		StartCoroutine(moveToPath());
	}

    
    public void request() {
		PathRequestManager.RequestPath(transform.position + Vector3.up * 3, pointsOfInterest[currentWaypointIndex] + Vector3.up * 3, OnPathFound, grid);
	}
	public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
	{
		if (pathSuccessful)
		{
			path = newPath;
			Debug.Log("path length" + path.Length);
			targetIndex = 0;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}
	public void coroutineStarter()
    {
		StartCoroutine(moveToPath());
    }
	public IEnumerator FollowPath()
	{
		int x, z;
		Vector3 currentWaypoint = path[0];
		while (true)
		{
			if (transform.position == currentWaypoint + Vector3.up * 3)
			{
				targetIndex++;
					
				if (targetIndex == path.Length)
				{
					switch (carType)
					{
						case CarType.simpleCar:
							Debug.Log("����� ��������");
							Debug.Log(currentWaypoint + " " + pointsOfInterest[2]);
							if ((currentWaypoint - Vector3.up * 3).Equals(pointsOfInterest[2]))
							{
								Debug.Log("����� ����");
								int tempx, tempy;
								grid.GetXZ(path[targetIndex - 1], out tempx, out tempy);
								grid.GetGridObject(tempx, tempy).isOcupied = true;
								yield return new WaitForSeconds(SimulationSystem.Instance.TimeForFuel(this, grid.GetGridObject(tempx, tempy)));
								SimulationSystem.Instance.FuelProccess(this, grid.GetGridObject(tempx, tempy));
								grid.GetGridObject(tempx, tempy).isOcupied = false;
							}
							if ((currentWaypoint - Vector3.up * 3).Equals(pointsOfInterest[3]))
							{
								int tempx, tempy;
								grid.GetXZ(path[targetIndex - 1], out tempx, out tempy);
								grid.GetGridObject(tempx, tempy).isOcupied = true;
								yield return new WaitForSeconds(SimulationSystem.Instance.MoneyPayTime(this));
								SimulationSystem.Instance.MoneyPayProccess(this);
								grid.GetGridObject(tempx, tempy).isOcupied = false;
							}
							break;
						case CarType.fuelTruck:
							if ((currentWaypoint - Vector3.up * 3).Equals(pointsOfInterest[2]))
							{
								int tempx, tempy;
								grid.GetXZ(path[targetIndex - 1], out tempx, out tempy);
								grid.GetGridObject(tempx, tempy).isOcupied = true;
								yield return new WaitForSeconds(SimulationSystem.Instance.FuelTankTime(grid.GetGridObject(tempx, tempy)));
								SimulationSystem.Instance.FuelTankProccess(grid.GetGridObject(tempx, tempy));
								var lol = grid.GetGridObject(tempx, tempy);
								grid.GetGridObject(tempx, tempy).isOcupied = false;
							}
							break;
						case CarType.incassation:
							if ((currentWaypoint - Vector3.up * 3).Equals(pointsOfInterest[2]))
							{
								int tempx, tempy;
								grid.GetXZ(path[targetIndex - 1], out tempx, out tempy);
								grid.GetGridObject(tempx, tempy).isOcupied = true;
								yield return new WaitForSeconds(SimulationSystem.Instance.MoneyTakeTime());
								SimulationSystem.Instance.MoneyProccess();
								grid.GetGridObject(tempx, tempy).isOcupied = false;
							}
							break;
					}
					if (currentWaypointIndex < pointsOfInterest.Length - 3) //����� �� ��������
					{
						Debug.Log("����� ����");//��� ���� ������� � ��������� ���� ��� ������ ���������.	
						currentWaypointIndex++;
						PathRequestManager.RequestPath(transform.position, pointsOfInterest[currentWaypointIndex], OnPathFound, grid);
					}
					else
					{
						Debug.Log("move to path");
						StartCoroutine(moveToPath());
					}
					yield break;
                }
                else
                {
					
					grid.GetXZ(path[targetIndex - 1], out x, out z);
					
					grid.GetGridObject(x, z).isOcupied = true;
					Debug.Log(x + " " + z + " is ocupied " + grid.GetGridObject(x, z).isOcupied);
					grid.GetXZ(path[targetIndex], out x, out z);
					yield return new WaitUntil(() => !grid.GetGridObject(x, z).isOcupied);
					grid.GetXZ(path[targetIndex - 1], out x, out z);
					grid.GetGridObject(x, z).isOcupied = false;
					Debug.Log(x + " " + z + " is free " + !grid.GetGridObject(x, z).isOcupied);
					currentWaypoint = path[targetIndex];
				}
			}
            
				transform.position = Vector3.MoveTowards(transform.position, currentWaypoint + Vector3.up * 3, speed * Time.deltaTime);
				Vector3 movementDir = currentWaypoint - transform.position;
				movementDir.Normalize();
				if (movementDir != Vector3.zero) {
				transform.rotation = Quaternion.LookRotation(movementDir, Vector3.up);
			}
				yield return null;
			
		}
	}
	public IEnumerator moveToPath()
    {
		Vector3 currentWaypoint = pointsOfInterest[currentWaypointIndex];
		while (true) {
			
			if (transform.position != currentWaypoint + Vector3.up * 3)
			{
				transform.position = Vector3.MoveTowards(transform.position, currentWaypoint + Vector3.up * 3, speed * Time.deltaTime);
				Vector3 movementDir = currentWaypoint - transform.position - Vector3.up * 3;
				movementDir.Normalize();
				if (movementDir != Vector3.zero)
				{
					Quaternion.LookRotation(movementDir, Vector3.up);
				}
				yield return null;
			}
			else if (currentWaypointIndex == 1)
			{
				currentWaypointIndex++;
				PathRequestManager.RequestPath(transform.position, pointsOfInterest[2], OnPathFound, grid);
				yield break;
			}
			else
			{
				currentWaypointIndex++;
				currentWaypoint = pointsOfInterest[currentWaypointIndex];
			}
		}
		
			
	}
	public void OnDrawGizmos()
	{
		if (path != null)
		{
			for (int i = targetIndex; i < path.Length; i++)
			{
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i] + Vector3.up * 3, Vector3.one);

				if (i == targetIndex)
				{
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else
				{
					Gizmos.DrawLine(path[i - 1], path[i]);
				}
			}
		}
	}

	public enum CarType
    {
		simpleCar,
		fuelTruck,
		incassation
    }
}