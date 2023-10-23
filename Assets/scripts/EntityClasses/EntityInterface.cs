using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EntityInterface /*: MonoBehaviour*/
{
    public void Add(List<EntityInterface> list);
    public void Change();
    public void Delete();

}
