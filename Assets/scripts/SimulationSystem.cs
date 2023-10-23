using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Data;
using static Unit;

public class SimulationSystem : MonoBehaviour
{
    public static SimulationSystem Instance { get; private set; }

    public GridXZ grid;
    public GridXZ serviceGrid;
    public GridXZ roadGrid;

    public bool isACtiveInc=false;

    public Vector3 entranceWaypoint;
    public Vector3 exitWaypoint;
    public Vector3 serviceEntranceWaypoint;
    public Vector3 serviceExitWaypoint;
    public List<Vector3> fuelDispencerWaypoints;
    public List<Vector3> fuelTankWaypoints;
    public Vector3 storeWaypoint;

    public bool isActiveFT=false;

    public GridBuildingSystem3D.GridObject storeObj;
    public double[] fuelSum = new double[6];
    public double[] fuelMoney = new double[6];
    public double moneyGlobal = new double();

    public List<ObjectParseWrapper> FTl = new List<ObjectParseWrapper>();
    public List<ObjectParseWrapper> FDl = new List<ObjectParseWrapper>();
    public List<ObjectParseWrapper> Fl = new List<ObjectParseWrapper>();
    public List<ObjectParseWrapper> Carl = new List<ObjectParseWrapper>();
    public List<GridBuildingSystem3D.GridObject> FTGrids;

    public int[] fDQueueCounter;
    private int[] fDPathLength;
    private bool isPause = true;
    private bool canSpawn;
    private float timeMultiplier = 1;
    public GameObject car;
    public GameObject sedanpref;
    public float timeBtwSpawns;
    public float oneSec = 1f;
    [SerializeField] private List<PlacedObjectTypeSO> placedObjectTypeSOList = null;
    void Awake()
    {
        Instance = this;
        string json = SaveLoadSystem.load(SaveLoadSystem.fileName);
        RandomLaws.timeGlobal = RandomLaws.timeGlobalOrigin;
        var map = JsonUtility.FromJson<GridXZWrapper>(json);
        GameObject.Find("time_text").GetComponent<TextMeshProUGUI>().text = RandomLaws.timeGlobal.ToString("HH:mm:ss");
        
        grid = new GridXZ(map.topology[0], placedObjectTypeSOList);
        serviceGrid = new GridXZ(map.topology[1], placedObjectTypeSOList);
        roadGrid = new GridXZ(map.topology[2], placedObjectTypeSOList);

        this.entranceWaypoint = new Vector3(map.entranceWaypoint.coordinates[0], map.entranceWaypoint.coordinates[1], map.entranceWaypoint.coordinates[2]);
        this.exitWaypoint = new Vector3(map.exitWaypoint.coordinates[0], map.exitWaypoint.coordinates[1], map.exitWaypoint.coordinates[2]);
        this.serviceEntranceWaypoint = new Vector3(map.serviceEntranceWaypoint.coordinates[0], map.serviceEntranceWaypoint.coordinates[1], map.serviceEntranceWaypoint.coordinates[2]);
        this.serviceExitWaypoint = new Vector3(map.serviceExitWaypoint.coordinates[0], map.serviceExitWaypoint.coordinates[1], map.serviceExitWaypoint.coordinates[2]);
        this.fuelDispencerWaypoints = new List<Vector3>();
        Pathfinding pf = new Pathfinding();
        this.fDQueueCounter = new int[map.fuelDispencerWaypoints.Count];
        this.fDPathLength = new int[map.fuelDispencerWaypoints.Count];
        for (int i = 0; i < map.fuelDispencerWaypoints.Count; i++)
        {
            this.fuelDispencerWaypoints.Add(new Vector3(map.fuelDispencerWaypoints[i].coordinates[0], map.fuelDispencerWaypoints[i].coordinates[1], map.fuelDispencerWaypoints[i].coordinates[2]));
            this.fDQueueCounter[i] = 0;
            this.fDPathLength[i] = pf.getLengthOfPath(entranceWaypoint, fuelDispencerWaypoints[i], grid);
        }
        sortWaypoints();
        this.fuelTankWaypoints = new List<Vector3>();
        for (int i = 0; i < map.fuelTankWaypoints.Count; i++)
        {
            this.fuelTankWaypoints.Add(new Vector3(map.fuelTankWaypoints[i].coordinates[0], map.fuelTankWaypoints[i].coordinates[1], map.fuelTankWaypoints[i].coordinates[2]));
        }
        this.storeWaypoint = new Vector3(map.storeWaypoint.coordinates[0], map.storeWaypoint.coordinates[1], map.storeWaypoint.coordinates[2]);
        timeBtwSpawns = RandomLaws.zk.GenTime();
       

        for (int i = 0; i < this.grid.GetWidth(); i++)
        {
            for (int j = 0; j < this.grid.GetHeight(); j++)
            {
                if (grid.GetGridObject(i, j).GetPlacedObject() != null)
                {
                    if (grid.GetGridObject(i, j).GetPlacedObject().placedObjectTypeSO.Equals(placedObjectTypeSOList[0]))
                    {
                        var lol = grid.GetGridObject(i, j).dbData;
                        FDl.Add(lol);
                    }
                    if (grid.GetGridObject(i, j).GetPlacedObject().placedObjectTypeSO.Equals(placedObjectTypeSOList[2]))
                    {
                        storeObj = grid.GetGridObject(i, j);
                        storeObj.dbData.par1 = 100000;
                    }

                }
            }
        }
        for (int i = 0; i < this.serviceGrid.GetWidth(); i++)
        {
            for (int j = 0; j < this.serviceGrid.GetHeight(); j++)
            {
                if (serviceGrid.GetGridObject(i, j).GetPlacedObject() != null)
                {
                    if (serviceGrid.GetGridObject(i, j).GetPlacedObject().placedObjectTypeSO.Equals(placedObjectTypeSOList[5]))
                    {
                        var lol = serviceGrid.GetGridObject(i, j).dbData;
                        FTl.Add(lol);
                    }
                }
            }
        }
        for (int i = 0; i < FTl.Count; i++)
        {
            var lol2 = serviceGrid.GetGridObject(fuelTankWaypoints[i]);
            FTGrids.Add(lol2);
        }
        DataTable ftypeTable = DBManager.GetTable("SELECT * FROM Ftype;");
        storeObj.dbData.curPar = 0;
        DataTable CarTable = DBManager.GetTable("SELECT car_id, car_name, car_volume, car_ftype_id, ft.Ftype_name FROM Car left join Ftype as ft on car_ftype_id=ft.Ftype_id;");

        for (int i = 0; i < CarTable.Rows.Count; i++)
        {
            ObjectParseWrapper car = new ObjectParseWrapper();
            car.id = int.Parse(CarTable.Rows[i][0].ToString());
            car.name = CarTable.Rows[i][1].ToString();
            car.par1 = int.Parse(CarTable.Rows[i][2].ToString());
            car.fuel_id = int.Parse(CarTable.Rows[i][3].ToString());
            car.fuel_name = CarTable.Rows[i][4].ToString();
            Carl.Add(car);
        }
        for (int i = 0; i < ftypeTable.Rows.Count; i++)
        {
            ObjectParseWrapper fuel = new ObjectParseWrapper();
            fuel.id = int.Parse(ftypeTable.Rows[i][0].ToString());
            fuel.fuel_name = ftypeTable.Rows[i][1].ToString();
            fuel.par1 = int.Parse(ftypeTable.Rows[i][2].ToString());
            Fl.Add(fuel);
        }
        List<ObjectParseWrapper> tmp = new List<ObjectParseWrapper>();
        ObjectParseWrapper index = new ObjectParseWrapper();
        for (int i = 0; i < FTl.Count; i++)
        {
            for (int j = 0; j < Fl.Count; j++)
            {
                if (FTl[i].fuel_id == Fl[j].id)
                    index = Fl[j];
            }
            if (tmp.IndexOf(index) == -1)
            {
                tmp.Add(index);
            }
        }
        Fl = tmp;
        for (int i = 0; i < Fl.Count; i++)
        {
            GameObject.Find("FuelName" + (i + 1)).GetComponent<TextMeshProUGUI>().text = "Объем проданного топлива " + Fl[i].fuel_name;
        }
        tmp = new List<ObjectParseWrapper>();
        for (int i = FTl.Count; i < 6; i++)
        {
            GameObject.Find("FuelName" + (i + 1)).SetActive(false);
            GameObject.Find("FuelVolume" + (i + 1)).SetActive(false);
        }
        for (int i = 0; i < Fl.Count; i++)
        {
            for (int j = 0; j < Carl.Count; j++)
            {
                if (Carl[j].fuel_id == Fl[i].id)
                    index = Carl[j];
            }
            if (tmp.IndexOf(index) == -1)
            {
                tmp.Add(index);
            }
        }
        for (int i = 0; i < Fl.Count; i++)
        {
            GameObject.Find("FuelNameMoney" + (i + 1)).GetComponent<TextMeshProUGUI>().text = "Заработок с топлива " + Fl[i].fuel_name;
        }
        Carl = tmp;
        for (int i = Fl.Count; i < 6; i++)
        {
            GameObject.Find("FuelNameMoney" + (i + 1)).SetActive(false);
            GameObject.Find("FuelMoney" + (i + 1)).SetActive(false);
        }

        timeBtwSpawns = RandomLaws.zk.GenTime();
        //GameObject.Find("SpawnArea").transform.position = roadGrid.GetWorldPosition(roadGrid.GetWidth() - 1, 0);
    }
    private void Update(){
        if(!isPause){
            if(timeBtwSpawns<=0){
                timeBtwSpawns=RandomLaws.zk.GenTime();
                System.Random rn = new System.Random();
                if (rn.NextDouble()<RandomLaws.inChance)
                    CreateCar(CarType.simpleCar);
                //вызов метода создания тачки
                // Instantiate(car,new Vector3(60, 0, 15),Quaternion.Euler(0f,0f,0f));
                
                //вызов метода создания тачки

            }
            else{
                timeBtwSpawns-=Time.deltaTime;
            }
            if(oneSec<=0){
                oneSec = 1f;
                RandomLaws.timeGlobal = RandomLaws.timeGlobal.AddSeconds(1);
                GameObject.Find("time_text").GetComponent<TextMeshProUGUI>().text = RandomLaws.timeGlobal.ToString("HH:mm:ss");
            }
            else{
                oneSec-=Time.deltaTime;
            }

            if (storeObj.dbData.curPar >= 85000)
            {
                if (!isACtiveInc){
                    CreateCar(CarType.incassation);
                    isACtiveInc=!isACtiveInc;
                }
                
            }
            for (int i = 0; i < FTl.Count; i++)
            {
                if (serviceGrid.GetGridObject(fuelTankWaypoints[i]).dbData.curPar / serviceGrid.GetGridObject(fuelTankWaypoints[i]).dbData.par1 <= 0.15)
                {
                    //вызов тб к заправке
                    if (!isActiveFT){
                        CreateCar(CarType.fuelTruck);
                        isActiveFT=!isActiveFT;
                    }
                   
                }
            }
        }

        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    CreateCar();
        //}
    }
    public void Pasue(){
        isPause = !isPause;
        if (isPause){
            Time.timeScale = 0;
        }
        else
            Time.timeScale = timeMultiplier;
        
    }
    //Smoke (SpEeD uP)
    public void TimeSpeedUp(){
        timeMultiplier+=0.25f;
        Time.timeScale=timeMultiplier;
        GameObject.Find("SpeedText").GetComponent<TextMeshProUGUI>().text = (double)timeMultiplier + "x\n(скорость прохождения времени)";
    }
    //Smoke (Slowed+reverb)
    public void TimeSlow(){
        if (timeMultiplier!=0.25f)
            timeMultiplier-=0.25f;
        Time.timeScale=timeMultiplier;
        GameObject.Find("SpeedText").GetComponent<TextMeshProUGUI>().text = (double)timeMultiplier + "x\n(скорость прохождения времени)";    
    }
    public void setSpawnRestriction(bool setter)
    {
        this.canSpawn = setter;
    }
    private void sortWaypoints()
    {
        int temp;
        Vector3 pointTemp;
        for (int i = 0; i < fDPathLength.Length - 1; i++)
       {
           for (int j = i + 1; j < fDPathLength.Length; j++)
           {
               if (fDPathLength[i] > fDPathLength[j])
               {
                   pointTemp = new Vector3(fuelDispencerWaypoints[i].x, fuelDispencerWaypoints[i].y, fuelDispencerWaypoints[i].z);
                   fuelDispencerWaypoints[i] = fuelDispencerWaypoints[j];
                   fuelDispencerWaypoints[j] = new Vector3(pointTemp.x, pointTemp.y, pointTemp.z);

                   temp = fDPathLength[i];
                   fDPathLength[i] = fDPathLength[j];
                   fDPathLength[j] = temp;
               }
           }
       }
    }
    public Vector3 getLeastBusyFD()
    {
        int ans = 0;
        for(int i = 0; i < fDQueueCounter.Length; i++)
        {
            if(ans > fDQueueCounter[i])
            {
                ans = fDQueueCounter[i];
            }
        }
        fDQueueCounter[ans]++;
        return fuelDispencerWaypoints[ans];
    }

    public void CreateCar(CarType carType)
    {
        var copy = Instantiate(car, roadGrid.GetWorldPosition(roadGrid.GetWidth() - 1, 0) + Vector3.up * 3, Quaternion.identity);
        System.Random rn = new System.Random();
        Unit carP = copy.GetComponent<Unit>();
        carP.carType = carType;
        carP.dbData = Carl[rn.Next(0, Carl.Count - 1)];
        carP.currentFuelAmount = rn.Next(-1, carP.dbData.par1 - 1);
        carP.currrentOrigin = carP.currentFuelAmount;
    }
    public float TimeForFuel(Unit carP, GridBuildingSystem3D.GridObject TRKp)
    {
        return (float)((carP.dbData.par1 - carP.currentFuelAmount) / TRKp.dbData.par1);
    }
    public bool FuelProccess(Unit carP, GridBuildingSystem3D.GridObject TRKp)
    {
        bool isStop = false;
        float curSum = 0f;
        int countFT = 0;
        for (int i = 0; i < FTl.Count; i++)
        {
            if (FTl[i].fuel_id == carP.dbData.fuel_id)
            {
                curSum += FTl[i].curPar;
                countFT++;
            }
        }
        for (int i = 0; i < Fl.Count; i++)
        {
            if (Fl[i].id == carP.dbData.fuel_id)
            {
                fuelSum[i] += (double)(carP.dbData.par1 - carP.currrentOrigin);
                fuelMoney[i] += (double)((carP.dbData.par1 - carP.currrentOrigin) * Fl[i].par1);
                moneyGlobal += (double)((carP.dbData.par1 - carP.currrentOrigin) * Fl[i].par1);
                GameObject.Find("FuelVolume" + (i + 1)).GetComponent<TextMeshProUGUI>().text = fuelSum[i] + " л";
                GameObject.Find("FuelMoney" + (i + 1)).GetComponent<TextMeshProUGUI>().text = fuelMoney[i] + " руб";
                GameObject.Find("MoneyDigit").GetComponent<TextMeshProUGUI>().text = moneyGlobal + " руб";
            }
        }



        for (int i = 0; i < FTl.Count; i++)
        {
            if (FTl[i].fuel_id == carP.dbData.fuel_id)
                FTl[i].curPar -= TRKp.dbData.par1 / countFT;
        }
        isStop = true;
        carP.currentFuelAmount = carP.dbData.par1;
        return isStop;
    }
    public bool FuelTankProccess(GridBuildingSystem3D.GridObject Tankp)
    {
        bool isStop = false;
        isStop = true;
        Tankp.dbData.curPar = Tankp.dbData.par1;
        isActiveFT=!isActiveFT;
        return isStop;
    }
    public bool MoneyPayProccess(Unit carP)
    {
        bool isStop = false;
        for (int i = 0; i < Fl.Count; i++)
        {
            if (Fl[i].id == carP.dbData.fuel_id)
            {
                storeObj.dbData.curPar += Fl[i].par1 * (carP.currentFuelAmount - carP.currrentOrigin);
            }
        }
        isStop = true;
        storeObj.dbData.curPar = storeObj.dbData.par1;
        return isStop;
    }
    public bool MoneyProccess()
    {
        bool isStop = false;
        isStop = true;
        storeObj.dbData.curPar = storeObj.dbData.par1;
        isACtiveInc=!isACtiveInc;
        return isStop;
    }

    public float FuelTankTime(GridBuildingSystem3D.GridObject Tankp)
    {
        return (float)(Tankp.dbData.par1 - Tankp.dbData.curPar) / 50;
    }
    public float MoneyPayTime(Unit carP)
    {
        float time = 0f;
        for (int i = 0; i < Fl.Count; i++)
        {
            if (Fl[i].id == carP.dbData.fuel_id)
            {
                time = (float)((carP.dbData.par1 - carP.currrentOrigin) * Fl[i].par1 / 500);
            }
        }
        return time;
    }
    public float MoneyTakeTime()
    {
        return (float)((storeObj.dbData.curPar) / 50000);
    }


}
