using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityClasses
{
    public class FuelTank : EntityInterface
    {
        int volume;
        string fuelName;
        FuelType fuelType;

        public FuelTank(int volume, string fuelName)
        {
            this.volume = volume;
            this.fuelName = fuelName;
        }

        public void Add(List<EntityInterface> list)
        {
            throw new NotImplementedException();
        }

        public void Change()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
