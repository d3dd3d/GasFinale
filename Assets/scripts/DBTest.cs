using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Data;
using UnityEngine;
public class DBTest : MonoBehaviour
{
    public GameObject prefabCar;
    public GameObject prefabTRK;
    public GameObject prefabFT;
    public GameObject prefabFtype;
    public TMP_Dropdown fuelType;
    public TMP_Dropdown carFuelType;
    public List<FT> FTList = new List<FT>();
    public List<Car> CarList= new List<Car>();
    public List<FD> FDList= new List<FD>();
    public List<Fuel> FuelList= new List<Fuel>();
    public List<string> Fhelp;
    public List<string> FThelp;
    public List<string> FDhelp;
    public string DBPath;
    private void Start()
    {
        DBPath=DBManager.DBPath;
        DBManager.ExecuteQueryWithoutAnswer("CREATE TABLE IF NOT EXISTS FuelTank (ftank_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, ftank_name VARCHAR(255) NOT NULL, ftank_volume INTEGER NOT NULL,ftank_ftype_id INTEGER NOT NULL)");
        DBManager.ExecuteQueryWithoutAnswer("CREATE TABLE IF NOT EXISTS Car (car_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, car_name VARCHAR(255) NOT NULL, car_volume INTEGER NOT NULL,car_ftype_id INTEGER NOT NULL)");
        DBManager.ExecuteQueryWithoutAnswer("CREATE TABLE IF NOT EXISTS TRK (TRK_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, TRK_name VARCHAR(255) NOT NULL, TRK_speed INTEGER NOT NULL)");
        DBManager.ExecuteQueryWithoutAnswer("CREATE TABLE IF NOT EXISTS Ftype (Ftype_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Ftype_name VARCHAR(255) NOT NULL, Ftype_price INTEGER NOT NULL,ftank_ftype_id INTEGER NOT NULL)");
        DataTable fTankTable = DBManager.GetTable("SELECT ftank_id, ftank_name,ftank_volume,ftank_ftype_id, ft.ftype_name FROM FuelTank left join Ftype as ft on ftank_ftype_id=ft.Ftype_id;");
        DataTable CarTable = DBManager.GetTable("SELECT car_id, car_name, car_volume, car_ftype_id, ft.Ftype_name FROM Car left join Ftype as ft on car_ftype_id=ft.Ftype_id;");
        DataTable TRKTable = DBManager.GetTable("SELECT * FROM TRK;");
        DataTable ftypeTable = DBManager.GetTable("SELECT * FROM Ftype;");
        for (int i=0;i<CarTable.Rows.Count;i++){
            Car car = new Car();
            car.id=int.Parse(CarTable.Rows[i][0].ToString());
            car.name = CarTable.Rows[i][1].ToString();
            car.volume=int.Parse(CarTable.Rows[i][2].ToString());
            car.fuel_id=int.Parse(CarTable.Rows[i][3].ToString());
            car.fuel_name=CarTable.Rows[i][4].ToString();
            CarList.Add(car);
            var copy = Instantiate(prefabCar, GameObject.Find("CarContent").transform);
            copy.GetComponentsInChildren<ObjectPars>()[0].type = 0;
            copy.GetComponentsInChildren<ObjectPars>()[0].id = int.Parse(CarTable.Rows[i][0].ToString());
            copy.GetComponentsInChildren<ObjectPars>()[0].name = CarTable.Rows[i][1].ToString();
            copy.GetComponentsInChildren<ObjectPars>()[0].par1 = int.Parse(CarTable.Rows[i][2].ToString());
            copy.GetComponentsInChildren<ObjectPars>()[0].fuel_id = int.Parse(CarTable.Rows[i][3].ToString());
            copy.GetComponentsInChildren<ObjectPars>()[0].fuel_name = CarTable.Rows[i][4].ToString();
            copy.GetComponentsInChildren<LayoutElement>()[0].GetComponentInChildren<TextMeshProUGUI>().text = CarTable.Rows[i][1].ToString();
            copy.GetComponentsInChildren<TextMeshProUGUI>()[3].text = CarTable.Rows[i][2].ToString() + " Л";
            copy.GetComponentsInChildren<TextMeshProUGUI>()[4].text = CarTable.Rows[i][4].ToString();
        }
        for (int i=0;i<fTankTable.Rows.Count;i++){
            FT ft =  new FT();
            ft.id = int.Parse(fTankTable.Rows[i][0].ToString());
            ft.name=fTankTable.Rows[i][1].ToString();
            ft.volume = int.Parse(fTankTable.Rows[i][2].ToString());
            ft.fuel_id = int.Parse(fTankTable.Rows[i][3].ToString());
            ft.fuel_name=fTankTable.Rows[i][4].ToString();
            FTList.Add(ft);
            var copy = Instantiate(prefabFT, GameObject.Find("fTContent").transform);
            copy.GetComponentsInChildren<ObjectPars>()[0].type = 1;
            copy.GetComponentsInChildren<ObjectPars>()[0].id = int.Parse(fTankTable.Rows[i][0].ToString());
            copy.GetComponentsInChildren<ObjectPars>()[0].name = fTankTable.Rows[i][1].ToString();
            copy.GetComponentsInChildren<ObjectPars>()[0].par1 = int.Parse(fTankTable.Rows[i][2].ToString());
            copy.GetComponentsInChildren<ObjectPars>()[0].fuel_id = int.Parse(fTankTable.Rows[i][3].ToString());
            copy.GetComponentsInChildren<ObjectPars>()[0].fuel_name = fTankTable.Rows[i][4].ToString();
            copy.GetComponentsInChildren<LayoutElement>()[0].GetComponentInChildren<TextMeshProUGUI>().text = fTankTable.Rows[i][1].ToString();
            copy.GetComponentsInChildren<TextMeshProUGUI>()[3].text = fTankTable.Rows[i][2].ToString() + " Л";
            copy.GetComponentsInChildren<TextMeshProUGUI>()[4].text = fTankTable.Rows[i][4].ToString();
        }
        for (int i=0;i<TRKTable.Rows.Count;i++){
            FD fd = new FD();
            fd.id = int.Parse(TRKTable.Rows[i][0].ToString());
            fd.name = TRKTable.Rows[i][1].ToString();
            fd.speed = int.Parse(TRKTable.Rows[i][2].ToString());
            FDList.Add(fd);
            var copy = Instantiate(prefabTRK, GameObject.Find("fDContent").transform);
            copy.GetComponentsInChildren<ObjectPars>()[0].type = 2;
            copy.GetComponentsInChildren<ObjectPars>()[0].id = int.Parse(TRKTable.Rows[i][0].ToString());
            copy.GetComponentsInChildren<ObjectPars>()[0].name = TRKTable.Rows[i][1].ToString();
            copy.GetComponentsInChildren<ObjectPars>()[0].par1 = int.Parse(TRKTable.Rows[i][2].ToString());
            copy.GetComponentsInChildren<LayoutElement>()[0].GetComponentInChildren<TextMeshProUGUI>().text = TRKTable.Rows[i][1].ToString();
            copy.GetComponentsInChildren<TextMeshProUGUI>()[2].text = TRKTable.Rows[i][2].ToString() + " Л/С";
        }
        for (int i=0;i<ftypeTable.Rows.Count;i++){
            Fuel fuel = new Fuel();
            fuel.id = int.Parse(ftypeTable.Rows[i][0].ToString());
            fuel.name=ftypeTable.Rows[i][1].ToString();
            fuel.price = int.Parse(ftypeTable.Rows[i][2].ToString());
            FuelList.Add(fuel);
            var copy = Instantiate(prefabFtype, GameObject.Find("fuelContent").transform);
            copy.GetComponentsInChildren<ObjectPars>()[0].type = 3;
            copy.GetComponentsInChildren<ObjectPars>()[0].id = int.Parse(ftypeTable.Rows[i][0].ToString());
            copy.GetComponentsInChildren<ObjectPars>()[0].name = ftypeTable.Rows[i][1].ToString();
            copy.GetComponentsInChildren<ObjectPars>()[0].par1 = int.Parse(ftypeTable.Rows[i][2].ToString());
            copy.GetComponentsInChildren<LayoutElement>()[0].GetComponentInChildren<TextMeshProUGUI>().text = ftypeTable.Rows[i][1].ToString();
            copy.GetComponentsInChildren<TextMeshProUGUI>()[2].text = ftypeTable.Rows[i][2].ToString() + " руб.";
        }
        setDropDown();
        FDhelp = new List<string>();
        for (int i=0;FuelList.Count>i;i++)
            FDhelp.Add(FDList[i].name);
        FThelp = new List<string>();
        for (int i=0;FuelList.Count>i;i++)
            FThelp.Add(FTList[i].name);
    }
    public void setDropDown() {
        Fhelp = new List<string>();
        for (int i=0;FuelList.Count>i;i++)
            Fhelp.Add(FuelList[i].name);
        fuelType.ClearOptions();
        fuelType.AddOptions(Fhelp);
        carFuelType.ClearOptions();
        carFuelType.AddOptions(Fhelp);
    }
    public void ReloadList(){
        DataTable fTankTable = DBManager.GetTable("SELECT ftank_id, ftank_name,ftank_volume,ftank_ftype_id, ft.ftype_name FROM FuelTank left join Ftype as ft on ftank_ftype_id=ft.Ftype_id;");
        DataTable CarTable = DBManager.GetTable("SELECT car_id, car_name, car_volume, car_ftype_id, ft.Ftype_name FROM Car left join Ftype as ft on car_ftype_id=ft.Ftype_id;");
        DataTable TRKTable = DBManager.GetTable("SELECT * FROM TRK;");
        DataTable ftypeTable = DBManager.GetTable("SELECT * FROM Ftype;");
        FuelList = new List<Fuel>();
        FTList = new List<FT>();
        FDList = new List<FD>();
        CarList = new List<Car>();
        for (int i=0;i<CarTable.Rows.Count;i++){
            Car car = new Car();
            car.id=int.Parse(CarTable.Rows[i][0].ToString());
            car.name = CarTable.Rows[i][1].ToString();
            car.volume=int.Parse(CarTable.Rows[i][2].ToString());
            car.fuel_id=int.Parse(CarTable.Rows[i][3].ToString());
            car.fuel_name=CarTable.Rows[i][4].ToString();
            CarList.Add(car);
        }
        for (int i=0;i<fTankTable.Rows.Count;i++){
            FT ft =  new FT();
            ft.id = int.Parse(fTankTable.Rows[i][0].ToString());
            ft.name=fTankTable.Rows[i][1].ToString();
            ft.volume = int.Parse(fTankTable.Rows[i][2].ToString());
            ft.fuel_id = int.Parse(fTankTable.Rows[i][3].ToString());
            ft.fuel_name=fTankTable.Rows[i][4].ToString();
            FTList.Add(ft);
        }
        for (int i=0;i<TRKTable.Rows.Count;i++){
            FD fd = new FD();
            fd.id = int.Parse(TRKTable.Rows[i][0].ToString());
            fd.name = TRKTable.Rows[i][1].ToString();
            fd.speed = int.Parse(TRKTable.Rows[i][2].ToString());
            FDList.Add(fd);
        }
        for (int i=0;i<ftypeTable.Rows.Count;i++){
            Fuel fuel = new Fuel();
            fuel.id = int.Parse(ftypeTable.Rows[i][0].ToString());
            fuel.name=ftypeTable.Rows[i][1].ToString();
            fuel.price = int.Parse(ftypeTable.Rows[i][2].ToString());
            FuelList.Add(fuel);
        }
        FDhelp = new List<string>();
        for (int i=0;FuelList.Count>i;i++)
            FDhelp.Add(FDList[i].name);
        FThelp = new List<string>();
        for (int i=0;FuelList.Count>i;i++)
            FThelp.Add(FTList[i].name);

    }
    public void ReloadFuel(){
        for (int i =0;i< GameObject.Find("CarContent").transform.childCount;i++){
            var del = GameObject.Find("CarContent").transform.GetChild(i).gameObject;
            Destroy(del);
        }
        for (int i =0;i< GameObject.Find("fTContent").transform.childCount;i++){
            var del = GameObject.Find("fTContent").transform.GetChild(i).gameObject;
            Destroy(del);
        }
        DataTable fTankTable = DBManager.GetTable("SELECT ftank_id, ftank_name,ftank_volume,ftank_ftype_id, ft.ftype_name FROM FuelTank left join Ftype as ft on ftank_ftype_id=ft.Ftype_id;");
        DataTable CarTable = DBManager.GetTable("SELECT car_id, car_name, car_volume, car_ftype_id, ft.Ftype_name FROM Car left join Ftype as ft on car_ftype_id=ft.Ftype_id;");
        for (int i=0;i<CarTable.Rows.Count;i++){
            var copy = Instantiate(prefabCar, GameObject.Find("CarContent").transform);
            copy.GetComponentsInChildren<ObjectPars>()[0].type = 0;
            copy.GetComponentsInChildren<ObjectPars>()[0].id = int.Parse(CarTable.Rows[i][0].ToString());
            copy.GetComponentsInChildren<ObjectPars>()[0].name = CarTable.Rows[i][1].ToString();
            copy.GetComponentsInChildren<ObjectPars>()[0].par1 = int.Parse(CarTable.Rows[i][2].ToString());
            copy.GetComponentsInChildren<ObjectPars>()[0].fuel_id = int.Parse(CarTable.Rows[i][3].ToString());
            copy.GetComponentsInChildren<ObjectPars>()[0].fuel_name = CarTable.Rows[i][4].ToString();
            copy.GetComponentsInChildren<LayoutElement>()[0].GetComponentInChildren<TextMeshProUGUI>().text = CarTable.Rows[i][1].ToString();
            copy.GetComponentsInChildren<TextMeshProUGUI>()[3].text = CarTable.Rows[i][2].ToString() + " Л";
            copy.GetComponentsInChildren<TextMeshProUGUI>()[4].text = CarTable.Rows[i][4].ToString();
        }
        FuelList = new List<Fuel>();
        for (int i=0;i<fTankTable.Rows.Count;i++){
             FT ft =  new FT();
            ft.id = int.Parse(fTankTable.Rows[i][0].ToString());
            ft.name=fTankTable.Rows[i][1].ToString();
            ft.volume = int.Parse(fTankTable.Rows[i][2].ToString());
            ft.fuel_id = int.Parse(fTankTable.Rows[i][3].ToString());
            ft.fuel_name=fTankTable.Rows[i][4].ToString();
            FTList.Add(ft);
            var copy = Instantiate(prefabFT, GameObject.Find("fTContent").transform);
            copy.GetComponentsInChildren<ObjectPars>()[0].type = 1;
            copy.GetComponentsInChildren<ObjectPars>()[0].id = int.Parse(fTankTable.Rows[i][0].ToString());
            copy.GetComponentsInChildren<ObjectPars>()[0].name = fTankTable.Rows[i][1].ToString();
            copy.GetComponentsInChildren<ObjectPars>()[0].par1 = int.Parse(fTankTable.Rows[i][2].ToString());
            copy.GetComponentsInChildren<ObjectPars>()[0].fuel_id = int.Parse(fTankTable.Rows[i][3].ToString());
            copy.GetComponentsInChildren<ObjectPars>()[0].fuel_name = fTankTable.Rows[i][4].ToString();
            copy.GetComponentsInChildren<LayoutElement>()[0].GetComponentInChildren<TextMeshProUGUI>().text = fTankTable.Rows[i][1].ToString();
            copy.GetComponentsInChildren<TextMeshProUGUI>()[3].text = fTankTable.Rows[i][2].ToString() + " Л";
            copy.GetComponentsInChildren<TextMeshProUGUI>()[4].text = fTankTable.Rows[i][4].ToString();
        }
        setDropDown();
    }
}
public class Fuel{
    public int id;
    public string name;
    public int price;
}
public class Car{
    public int id;
    public string name;
    public int volume;
    public int fuel_id;
    public string fuel_name;
}
public class FD{
    public int id;
    public string name;
    public int speed;
}
public class FT{
    public int id;
    public string name;
    public int volume;
    public int fuel_id;
    public string fuel_name;
}
