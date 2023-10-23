using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityClasses
{
    public class Car : EntityInterface
    {
        int fuelTankVolume;
        string fuelName;
        FuelType fuelType;

        public Car(int fuelTankVolume, string fuelName)
        {
            this.fuelTankVolume = fuelTankVolume;
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
