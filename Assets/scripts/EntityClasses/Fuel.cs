using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityClasses
{
    public enum FuelType { AI92, AI95 };
    public class Fuel : MonoBehaviour, EntityInterface
    {
        string name;
        int price;

        public Fuel(string name, int price)
        {
            this.name = name;
            this.price = price;
        }
        public void Add(List<EntityInterface> list)
        {
            throw new System.NotImplementedException();
        }

        public void Change()
        {
            throw new System.NotImplementedException();
        }

        public void Delete()
        {
            throw new System.NotImplementedException();
        }
    }
}
