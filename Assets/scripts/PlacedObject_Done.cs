using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject_Done : MonoBehaviour
{

    public static PlacedObject_Done Create(Vector3 worldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir, PlacedObjectTypeSO placedObjectTypeSO, ObjectType prefType)
    {
        if (placedObjectTypeSO != null)
        {
            Transform placedObjectTransform = Instantiate(placedObjectTypeSO.prefab, worldPosition, Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));

            PlacedObject_Done placedObject = placedObjectTransform.GetComponent<PlacedObject_Done>();
            placedObject.Setup(placedObjectTypeSO, origin, dir, prefType);

            return placedObject;
        }
        else
        {
            return null;
        }
    }




    public PlacedObjectTypeSO placedObjectTypeSO;
    private Vector2Int origin;
    private PlacedObjectTypeSO.Dir dir;
    public ObjectType prefab;
    private void Setup(PlacedObjectTypeSO placedObjectTypeSO, Vector2Int origin, PlacedObjectTypeSO.Dir dir, ObjectType prefType)
    {
        this.placedObjectTypeSO = placedObjectTypeSO;
        this.origin = origin;
        this.dir = dir;
        prefab = prefType;
    }
    public void Setup(PlacedObject_Done_Data data, List<PlacedObjectTypeSO> placedObjectTypeSOList)
    {
        PlacedObjectTypeSO placedObject = new PlacedObjectTypeSO(data, placedObjectTypeSOList);
        this.placedObjectTypeSO = placedObject;
        this.origin = new Vector2Int(data.origin[0], data.origin[1]);
        this.dir = data.dir;
        this.prefab = data.prefab;

    }

    public List<Vector2Int> GetGridPositionList()
    {
        return placedObjectTypeSO.GetGridPositionList(origin, dir);
    }
    public PlacedObjectTypeSO.Dir getDirection()
    {
        return dir;
    }
    public PlacedObjectTypeSO getSO()
    {
        return placedObjectTypeSO;
    }
    public Vector2Int getOrigin()
    {
        return origin;
    }
    public PlacedObjectTypeSO getPlacedObject()
    {
        return placedObjectTypeSO;
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public override string ToString()
    {
        return placedObjectTypeSO.nameString;
    }

    public bool isWalkable()
    {
        return placedObjectTypeSO.isWalkable;
    }

    public bool isStopable()
    {
        return placedObjectTypeSO.isStopable;
    }
}
