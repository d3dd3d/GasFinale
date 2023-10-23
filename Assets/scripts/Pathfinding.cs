using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using static GridBuildingSystem3D;

public class Pathfinding : MonoBehaviour
{

	PathRequestManager requestManager;
	GridXZ grid;

	void Start()
	{
		requestManager = GetComponent<PathRequestManager>();
		grid = SimulationSystem.Instance.grid;
	}


	public void StartFindPath(Vector3 startPos, Vector3 targetPos, GridXZ grid)
	{
		StartCoroutine(FindPath(startPos, targetPos, grid));
	}

	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos, GridXZ grid)
	{

		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;

		GridObject startNode = grid.GetGridObject(startPos);
		GridObject targetNode = grid.GetGridObject(targetPos);


		if (startNode.getIsWalkable() && targetNode.getIsWalkable())
		{
			Heap<GridObject> openSet = new Heap<GridObject>(grid.GetWidth() * grid.GetHeight());
			HashSet<GridObject> closedSet = new HashSet<GridObject>();
			openSet.Add(startNode);

			while (openSet.Count > 0)
			{
				GridObject currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);

				if (currentNode == targetNode)
				{
					Debug.Log("���������� �������");
					pathSuccess = true;
					break;
				}

				foreach (GridObject neighbour in grid.GetNeighbours(currentNode))
				{
					if (!neighbour.getIsWalkable() || closedSet.Contains(neighbour))
					{
						continue;
					}

					int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
					{
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetNode);
						neighbour.parent = currentNode;

						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
					}
				}
			}
		}
		
		yield return null;
		if (pathSuccess)
		{
			waypoints = RetracePath(startNode, targetNode);
		}
		requestManager.FinishedProcessingPath(waypoints, pathSuccess);

	}

	public bool canBuildPath(Vector3 startPos, Vector3 targetPos, GridXZ grid)
    {
		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;

		GridObject startNode = grid.GetGridObject(startPos);
		GridObject targetNode = grid.GetGridObject(targetPos);


		if (startNode.getIsWalkable() && targetNode.getIsWalkable())
		{
			Heap<GridObject> openSet = new Heap<GridObject>(grid.GetWidth() * grid.GetHeight());
			HashSet<GridObject> closedSet = new HashSet<GridObject>();
			openSet.Add(startNode);

			while (openSet.Count > 0)
			{
				GridObject currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);

				if (currentNode == targetNode)
				{
					pathSuccess = true;
					break;
				}

				foreach (GridObject neighbour in grid.GetNeighbours(currentNode))
				{
					if (!neighbour.getIsWalkable() || closedSet.Contains(neighbour))
					{
						continue;
					}

					int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
					{
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetNode);
						neighbour.parent = currentNode;

						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
					}
				}
			}
		}
		return pathSuccess;
	}

	Vector3[] RetracePath(GridObject startNode, GridObject endNode)
	{
		List<GridObject> path = new List<GridObject>();
		GridObject currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Add(startNode);

		Vector3[] waypoints = SimplifyPath(path);
		Debug.Log(waypoints.Length);
		Array.Reverse(waypoints);
		return waypoints;

	}
	public int getLengthOfPath(Vector3 startPos, Vector3 targetPos, GridXZ grid)
    {
		canBuildPath(startPos, targetPos, grid);
		GridObject startNode = grid.GetGridObject(startPos);
		GridObject endNode = grid.GetGridObject(targetPos);
		List<GridObject> path = new List<GridObject>();
		GridObject currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Add(startNode);

		return path.Count;
	}
	Vector3[] SimplifyPath(List<GridObject> path)
	{
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = new Vector2(path[0].getX() - path[1].getX(), path[0].getY() - path[1].getY());
		waypoints.Add(path[0].GetWorldPosition(path[0].getX(), path[0].getY()) + Vector3.up * 3);

		for (int i = 1; i < path.Count; i++)
		{
			waypoints.Add(path[i].GetWorldPosition(path[i].getX(), path[i].getY()) + Vector3.up * 3);
			//Vector2 directionNew = new Vector2(path[i - 1].getX() - path[i].getX(), path[i - 1].getY() - path[i].getY());
			//if (directionNew != directionOld)
			//{
			//	waypoints.Add(path[i - 1].GetWorldPosition(path[i - 1].getX(), path[i - 1].getY()) + Vector3.up * 3);
			//	directionOld = directionNew;
			//}
			
		}
		return waypoints.ToArray();
	}

	int GetDistance(GridObject nodeA, GridObject nodeB)
	{
		int dstX = Mathf.Abs(nodeA.getX() - nodeB.getX());
		int dstY = Mathf.Abs(nodeA.getY() - nodeB.getY());

		
		return 10 * dstX + 10 * dstY;
	}


}