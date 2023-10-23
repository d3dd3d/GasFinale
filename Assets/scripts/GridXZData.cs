using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static GridBuildingSystem3D;

[Serializable]
public class GridXZData 
{

    public int width;
    public int height;
    public float cellSize;
    public float[] originPosition;
    public NestedList[] gridArray;

    public GridXZData(GridXZ grid)
    {
        width = grid.GetWidth();
        height = grid.GetHeight();
        cellSize = grid.GetCellSize();
        originPosition = new float[] { grid.getOriginPos().x, grid.getOriginPos().y, grid.getOriginPos().z };

        GridObject[,] nodeData = grid.getGridArray();
        gridArray = new NestedList[nodeData.GetLength(0)];
        for (int x = 0; x < nodeData.GetLength(0); x++)
        {
            GridObject[] arr = new GridObject[nodeData.GetLength(1)];
            for (int y = 0; y < nodeData.GetLength(1); y++)
            {
                arr[y] = nodeData[x, y]; 
            }
            gridArray[x] = new NestedList(arr);
        }
    }
}
[Serializable]
public class NestedList
{
    public GridObjectData[] row;

    public NestedList(GridObject[] arr)
    {
        row = new GridObjectData[arr.Length];
        for(int i = 0; i < row.Length; i++)
        {
            row[i] = new GridObjectData(arr[i]);
        }
    }
}
