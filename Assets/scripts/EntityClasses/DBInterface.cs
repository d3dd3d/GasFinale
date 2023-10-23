using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EntityClasses;

public class DBInterface : MonoBehaviour
{
    public static void Add(EntityInterface entity, List<EntityInterface> list)
    {
        list.Add(entity);
    }

    public static void Change(EntityInterface oldEntity, EntityInterface newEntity, List<EntityInterface> list)
    {
        int index = list.FindIndex(s => s == oldEntity);

        if (index != -1)
            list[index] = newEntity;
    }

    public static void Delete(EntityInterface entity, List<EntityInterface> list)
    {
        list.Remove(entity);
    }

}
