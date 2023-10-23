using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class ObjectPars : MonoBehaviour
{
    public int type;
    public int id;
    public string name;
    public int par1;
    public int fuel_id;
    public string fuel_name;

    public ObjectPars(ObjectParseWrapper pars)
    {
        this.type = pars.type;
        this.name = pars.name;
        this.id = pars.id;
        this.par1 = pars.par1;
        this.fuel_id = pars.fuel_id;
        this.fuel_name = pars.fuel_name;
    }

    public ObjectPars(Fuel fl)
    {
        this.type = 3;
        this.id = fl.id;
        this.name = fl.name;
        this.par1 = fl.price;
    }
    public ObjectPars(Car car)
    {
        this.type = 0;
        this.id = car.id;
        this.name = car.name;
        this.par1 = car.volume;
        this.fuel_id = car.fuel_id;
        this.fuel_name = car.fuel_name;
    }
    public ObjectPars(FT ft)
    {
        this.type = 1;
        this.id = ft.id;
        this.name = ft.name;
        this.par1 = ft.volume;
        this.fuel_id = ft.fuel_id;
        this.fuel_name = ft.fuel_name;
    }
    public ObjectPars(FD fd)
    {
        this.type = 2;
        this.id = fd.id;
        this.name = fd.name;
        this.par1 = fd.speed;
    }
}
[Serializable]
public class ObjectParseWrapper
{
    public int type;
    public int id;
    public string name;
    public int par1;
    public int fuel_id;
    public string fuel_name;
    public float curPar = 0;

    public ObjectParseWrapper(ObjectPars pars)
    {
        this.type = pars.type;
        this.name = pars.name;
        this.id = pars.id;
        this.par1 = pars.par1;
        this.fuel_id = pars.fuel_id;
        this.fuel_name = pars.fuel_name;
    }
    public ObjectParseWrapper()
    {
        this.type = 0;
        this.name = "";
        this.id = 0;
        this.par1 = 0;
        this.fuel_id = 0;
        this.fuel_name = "";
    }
    public ObjectParseWrapper(Fuel fl)
    {
        this.type = 3;
        this.id = fl.id;
        this.name = fl.name;
        this.par1 = fl.price;

    }
    public ObjectParseWrapper(Car car)
    {
        this.type = 0;
        this.id = car.id;
        this.name = car.name;
        this.par1 = car.volume;
        this.fuel_id = car.fuel_id;
        this.fuel_name = car.fuel_name;
    }
    public ObjectParseWrapper(FT ft)
    {
        this.type = 1;
        this.id = ft.id;
        this.name = ft.name;
        this.par1 = ft.volume;
        this.fuel_id = ft.fuel_id;
        this.fuel_name = ft.fuel_name;
        this.curPar = ft.volume;
    }
    public ObjectParseWrapper(FD fd)
    {
        this.type = 2;
        this.id = fd.id;
        this.name = fd.name;
        this.par1 = fd.speed;
    }
}

