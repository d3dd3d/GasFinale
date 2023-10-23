using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static GridBuildingSystem3D;

[Serializable]
public class GridObjectData 
{

    private GridObject node;
    private PlacedObject_Done building;

    public GridObjectData(GridObject node)
    {
        x = node.getX();
        y = node.getY();

        if (node.placedObject != null)
        {
            isWalkable = node.getIsWalkable();
            isStopable = node.placedObject.isStopable();
            if (node.dbData != null)
                dbData = node.dbData;
            placedObject = new PlacedObject_Done_Data(node.GetPlacedObject(), this);

        }
        else
        {
            isWalkable = false;
            isStopable = false;
            placedObject = new PlacedObject_Done_Data(null, this);
        }



    }

    public ObjectParseWrapper dbData;
    public int x;
    public int y;
    public bool isWalkable;
    public bool isStopable;
    public PlacedObject_Done_Data placedObject;
}
[Serializable]
public class PlacedObject_Done_Data
{
    public int[] origin;
    public PlacedObjectTypeSO.Dir dir;
    public PlacedObjectTypeSO_Data placedObject;
    public ObjectType prefab;
    public PlacedObject_Done_Data(PlacedObject_Done objectData, GridObjectData node)
    {
        if (objectData != null)
        {
            origin = new int[] { objectData.getOrigin().x, objectData.getOrigin().y };
            dir = objectData.getDirection();
            prefab = objectData.prefab;
            placedObject = new PlacedObjectTypeSO_Data(objectData.getSO());
        }
        else
        {
            origin = new int[] { node.x, node.y };
            dir = PlacedObjectTypeSO.Dir.Down;
            placedObject = null;
            prefab = ObjectType.empty;
        }
    }
}
[Serializable]
public class PlacedObjectTypeSO_Data
{
    public string nameString;

    public int width;
    public int height;
    public bool isWalkable;
    public bool isStopable;

    public PlacedObjectTypeSO_Data(PlacedObjectTypeSO data)
    {
        nameString = data.nameString;
        width = data.width;
        height = data.height;
        isWalkable = data.isWalkable;
        isStopable = data.isStopable;
    }
}
public enum ObjectType
{
    fuelDispencer,
    road,
    store,
    entrance,
    exit,
    fuelTank,
    infoTable,
    empty
}