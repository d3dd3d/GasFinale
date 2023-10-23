using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;
using TMPro;
public class GridBuildingSystem3D : MonoBehaviour {
    
    public static GridBuildingSystem3D Instance { get; private set; }

    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;

    public DBTest dbT;
    public GameObject rightPanel;
    public bool isCopy = false;
    public GridObject selectedGO;
    public ErrorScript erc;
    public static ObjectPars prefabObj;

    public Vector3 entranceWaypoint;
    public Vector3 exitWaypoint;
    public Vector3 serviceEntranceWaypoint;
    public Vector3 serviceExitWaypoint;
    public List<Vector3> fuelDispencerWaypoints;
    public List<Vector3> fuelTankWaypoints;
    public Vector3 storeWaypoint;
    private List<int> buildingCounter;
    float cellSize;
    public GridXZ grid;
    public GridXZ serviceGrid;
    public GridXZ roadGrid;
    [SerializeField] private List<PlacedObjectTypeSO> placedObjectTypeSOList = null;
    private PlacedObjectTypeSO placedObjectTypeSO;
    private PlacedObjectTypeSO.Dir dir;
    private ObjectType prefab;
    private void Awake()
    {
        Instance = this;
        cellSize = 10f;
        if (SaveLoadSystem.fileName == null)
        {
            int gridWidth = SaveLoadSystem.width1;
            int gridHeight = SaveLoadSystem.height;
            int grid2Width = SaveLoadSystem.width2;

            buildingCounter = new List<int>(7) { 0, 0, 0, 0, 0, 0, 0 };
            grid = new GridXZ(gridWidth, gridHeight, cellSize, new Vector3(0, 0, 0), "АЗС");
            serviceGrid = new GridXZ(grid2Width, gridHeight, cellSize, new Vector3(gridWidth * cellSize, 0, 0), "Служебная часть");
            roadGrid = new GridXZ(gridWidth + grid2Width, 1, cellSize, new Vector3(0, 0, -cellSize), "");
            fuelDispencerWaypoints = new List<Vector3>();
            for (int i = 0; i < gridWidth + grid2Width; i++)
            {
                PlacedObject_Done placedObject = PlacedObject_Done.Create(roadGrid.GetWorldPosition(i, 0), new Vector2Int(i, 0), dir, placedObjectTypeSOList[1], ObjectType.road);
                roadGrid.GetGridObject(i, 0).SetPlacedObject(placedObject);
            }
        }
        else
        {
            string json = SaveLoadSystem.load(SaveLoadSystem.fileName);
            SaveLoadSystem.fileName = null;
            var map = JsonUtility.FromJson<GridXZWrapper>(json);
            this.grid = new GridXZ(map.topology[0], placedObjectTypeSOList);
            this.serviceGrid = new GridXZ(map.topology[1], placedObjectTypeSOList);
            this.roadGrid = new GridXZ(map.topology[2], placedObjectTypeSOList);
            this.entranceWaypoint = new Vector3(map.entranceWaypoint.coordinates[0], map.entranceWaypoint.coordinates[1], map.entranceWaypoint.coordinates[2]);
            this.exitWaypoint = new Vector3(map.exitWaypoint.coordinates[0], map.exitWaypoint.coordinates[1], map.exitWaypoint.coordinates[2]);
            this.serviceEntranceWaypoint = new Vector3(map.serviceEntranceWaypoint.coordinates[0], map.serviceEntranceWaypoint.coordinates[1], map.serviceEntranceWaypoint.coordinates[2]);
            this.serviceExitWaypoint = new Vector3(map.serviceExitWaypoint.coordinates[0], map.serviceExitWaypoint.coordinates[1], map.serviceExitWaypoint.coordinates[2]);
            this.fuelDispencerWaypoints = new List<Vector3>();
            for (int i = 0; i < map.fuelDispencerWaypoints.Count; i++)
            {
                this.fuelDispencerWaypoints.Add(new Vector3(map.fuelDispencerWaypoints[i].coordinates[0], map.fuelDispencerWaypoints[i].coordinates[1], map.fuelDispencerWaypoints[i].coordinates[2]));
            }
            this.fuelTankWaypoints = new List<Vector3>();
            for (int i = 0; i < map.fuelDispencerWaypoints.Count; i++)
            {
                this.fuelTankWaypoints.Add(new Vector3(map.fuelTankWaypoints[i].coordinates[0], map.fuelTankWaypoints[i].coordinates[1], map.fuelTankWaypoints[i].coordinates[2]));
            }
            buildingCounter = new List<int>(7) { 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < this.grid.GetWidth(); i++)
            {
                for (int j = 0; j < this.grid.GetHeight(); j++)
                {
                    if (grid.GetGridObject(i, j).GetPlacedObject() != null)
                    {
                        if (grid.GetGridObject(i, j).GetPlacedObject().placedObjectTypeSO.Equals(placedObjectTypeSOList[0]))
                        {
                            buildingCounter[0]++;
                        }
                        if (grid.GetGridObject(i, j).GetPlacedObject().placedObjectTypeSO.Equals(placedObjectTypeSOList[1]))
                        {
                            buildingCounter[1]++;
                        }
                        if (grid.GetGridObject(i, j).GetPlacedObject().placedObjectTypeSO.Equals(placedObjectTypeSOList[2]))
                        {
                            buildingCounter[2]++;
                        }
                        if (grid.GetGridObject(i, j).GetPlacedObject().placedObjectTypeSO.Equals(placedObjectTypeSOList[3]))
                        {
                            buildingCounter[3]++;
                        }
                        if (grid.GetGridObject(i, j).GetPlacedObject().placedObjectTypeSO.Equals(placedObjectTypeSOList[4]))
                        {
                            buildingCounter[4]++;
                        }
                        if (grid.GetGridObject(i, j).GetPlacedObject().placedObjectTypeSO.Equals(placedObjectTypeSOList[5]))
                        {
                            buildingCounter[5]++;
                        }
                        if (grid.GetGridObject(i, j).GetPlacedObject().placedObjectTypeSO.Equals(placedObjectTypeSOList[6]))
                        {
                            buildingCounter[6]++;
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
                        if (serviceGrid.GetGridObject(i, j).GetPlacedObject().placedObjectTypeSO.Equals(placedObjectTypeSOList[0]))
                        {
                            buildingCounter[0]++;
                        }
                        if (serviceGrid.GetGridObject(i, j).GetPlacedObject().placedObjectTypeSO.Equals(placedObjectTypeSOList[1]))
                        {
                            buildingCounter[1]++;
                        }
                        if (serviceGrid.GetGridObject(i, j).GetPlacedObject().placedObjectTypeSO.Equals(placedObjectTypeSOList[2]))
                        {
                            buildingCounter[2]++;
                        }
                        if (serviceGrid.GetGridObject(i, j).GetPlacedObject().placedObjectTypeSO.Equals(placedObjectTypeSOList[3]))
                        {
                            buildingCounter[3]++;
                        }
                        if (serviceGrid.GetGridObject(i, j).GetPlacedObject().placedObjectTypeSO.Equals(placedObjectTypeSOList[4]))
                        {
                            buildingCounter[4]++;
                        }
                        if (serviceGrid.GetGridObject(i, j).GetPlacedObject().placedObjectTypeSO.Equals(placedObjectTypeSOList[5]))
                        {
                            buildingCounter[5]++;
                        }
                        if (serviceGrid.GetGridObject(i, j).GetPlacedObject().placedObjectTypeSO.Equals(placedObjectTypeSOList[6]))
                        {
                            buildingCounter[6]++;
                        }
                    }
                }
            }
            this.storeWaypoint = new Vector3(map.storeWaypoint.coordinates[0], map.storeWaypoint.coordinates[1], map.storeWaypoint.coordinates[2]);
        }
    }

    [Serializable]
    public class GridObject : IHeapItem<GridObject> {

        public ObjectParseWrapper dbData = new ObjectParseWrapper();
        private GridXZ grid;
        [SerializeField]
        private int x;
        [SerializeField]
        private int y;
        [SerializeField]
        private bool isWalkable;
        [SerializeField]
        private bool isStopable;
        public bool isOcupied = false;
        public PlacedObject_Done placedObject;
        public int gCost;
        public int hCost;
        public GridObject parent;
        int heapIndex;
        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }
        public GridObject(GridXZ grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;
            placedObject = null;
            isWalkable = false;
            isStopable = false;
        }

        public GridObject(GridXZ grid, GridObjectData data, List<PlacedObjectTypeSO> list)
        {
            this.grid = grid;
            this.x = data.x;
            this.y = data.y;
            this.dbData = data.dbData;
            PlacedObject_Done_Data dat = data.placedObject;
            PlacedObject_Done placedObject = PlacedObject_Done.Create(GetWorldPosition(this.x, this.y), new Vector2Int(dat.origin[0], dat.origin[1]), dat.dir, list[(int)dat.prefab], dat.prefab);
            this.placedObject = placedObject;
            // if (this.placedObject!=null){
            //     this.dbData = this.placedObject.gameObject.Instantiate(prefabObj) as ObjectPars;//data.dbData;
            //     // this.dbData.type = data.dbData.type;
            //     // this.dbData.id=data.dbData.id;
            //     // this.dbData.name = data.dbData.name;
            //     // this.dbData.par1=data.dbData.par1;
            //     // this.dbData.fuel_id = data.dbData.fuel_id;
            //     // this.dbData.fuel_name=data.dbData.fuel_name;
            // }
            isWalkable = data.isWalkable;
            isStopable = data.isStopable;
        }
        public int getX() {
            return x;
        }
        public int getY()
        {
            return y;
        }
        public Vector3 GetWorldPosition(int x, int z)
        {
            return new Vector3(x, 0, z) * grid.GetCellSize() + grid.getOriginPos() - new Vector3(grid.GetCellSize(), 0, grid.GetCellSize());
        }
        public override string ToString() {
            return x + ", " + y + "\n" + placedObject;
        }

        public void SetPlacedObject(PlacedObject_Done placedObject) {
            this.placedObject = placedObject;
            isWalkable = placedObject.isWalkable();
            isStopable = placedObject.isStopable();
            grid.TriggerGridObjectChanged(x, y, isWalkable);
        }

        public void ClearPlacedObject() {
            placedObject = null;
            isWalkable = false;
            grid.TriggerGridObjectChanged(x, y, false);
        }

        public PlacedObject_Done GetPlacedObject() {
            return placedObject;
        }

        public bool CanBuild() {
            return placedObject == null;
        }
        public bool getIsWalkable()
        {
            return isWalkable;
        }

        public int HeapIndex {
            get
            {
                return heapIndex;
            }
            set
            {
                heapIndex = value;
            }
        }

        public int CompareTo(GridObject other)
        {
            int compare = fCost.CompareTo(other.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(other.hCost);
            }
            return -compare;
        }
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && !IsMouseOverUI() && grid.isInsideGrid(Mouse3D.GetMouseWorldPosition())) {
            if (placedObjectTypeSO != null){
                Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
                if (grid.isInsideGrid(mousePosition)) { }
                grid.GetXZ(mousePosition, out int x, out int z);

                Vector2Int placedObjectOrigin = new Vector2Int(x, z);
                placedObjectOrigin = grid.ValidateGridPosition(placedObjectOrigin);
    
                // Test Can Build
                List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);
                bool canBuild = true;
                foreach (Vector2Int gridPosition in gridPositionList) {
                    if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild() || prefab == ObjectType.fuelTank) {
                        Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y);
                        string buildingType = "";
                        switch (prefab)
                        {
                            case ObjectType.fuelTank:
                                buildingType = "ТБ";
                                break;
                        }
                        UtilsClass.CreateWorldTextPopup("Здесь нельзя строить " + buildingType, placedObjectWorldPosition);
                        canBuild = false;
                        break;
                    }
                }

                if (canBuild && isValidCount())
                {
                    Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y);
                    Vector3 offset = new Vector3();
                    offset = PlacedObjectTypeSO.GetNeighbourNodeStat(dir);
                    bool isTRK = true;
                    var neighborGr = grid.GetGridObject(placedObjectWorldPosition + offset * cellSize);
                    if (prefab != ObjectType.fuelDispencer)
                        isTRK = false;
                    if ((!isTRK) || ((neighborGr != null) && (neighborGr.dbData.id == 0)))
                    {
                        PlacedObject_Done placedObject = PlacedObject_Done.Create(placedObjectWorldPosition, placedObjectOrigin, dir, placedObjectTypeSO, prefab);
                        buildingCounter[(int)prefab]++;
                        if (placedObject.isStopable())
                        {
                            switch (prefab)
                            {
                                case ObjectType.fuelDispencer:
                                    fuelDispencerWaypoints.Add(placedObjectWorldPosition + offset * cellSize);
                                    if (!isCopy)
                                    {
                                        selectedGO = grid.GetGridObject(x, z);
                                        selectedGO.dbData = new ObjectParseWrapper();
                                        selectedGO.dbData.type = 2;
                                        selectedGO.dbData.id = dbT.FDList[0].id;
                                        selectedGO.dbData.name = dbT.FDList[0].name;
                                        selectedGO.dbData.par1 = dbT.FDList[0].speed;//new ObjectPars(dbT.FDList[0]);
                                        neighborGr.dbData = selectedGO.dbData;
                                    }
                                    else
                                    {
                                        var dataS = selectedGO.dbData;
                                        neighborGr.dbData = new ObjectParseWrapper();
                                        selectedGO = grid.GetGridObject(x, z);
                                        selectedGO.dbData = dataS;
                                        neighborGr.dbData = selectedGO.dbData;
                                        isCopy = false;
                                    }
                                    break;
                                case ObjectType.store:
                                    offset = placedObject.placedObjectTypeSO.GetNeighbourNode(dir);
                                    storeWaypoint = placedObjectWorldPosition + offset * cellSize;
                                    break;
                                case ObjectType.entrance:
                                    entranceWaypoint = placedObjectWorldPosition;
                                    break;
                                case ObjectType.exit:
                                    exitWaypoint = placedObjectWorldPosition;
                                    break;
                            }
                        }
                        foreach (Vector2Int gridPosition in gridPositionList)
                        {
                            grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                        }
                        DeselectObjectType();
                    }
                    else
                    {
                        UtilsClass.CreateWorldTextPopup("Здесь нельзя построить еще одну ТРК", placedObjectWorldPosition);
                    }
                }
                else if (!isValidCount())
                {
                    Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y);
                    UtilsClass.CreateWorldTextPopup("Превышен предел построек", placedObjectWorldPosition);
                }
                else
                {
                    Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y);
                    UtilsClass.CreateWorldTextPopup("Здесь нельзя строить", placedObjectWorldPosition);
                }
            }
            else if (grid.GetGridObject(Mouse3D.GetMouseWorldPosition()).GetPlacedObject() != null){
                int x, z;
                grid.GetXZ(Mouse3D.GetMouseWorldPosition(),out x, out z);
                var placedObject = grid.GetGridObject(x, z).GetPlacedObject();
                selectedGO = grid.GetGridObject(x, z);
                if (placedObject.placedObjectTypeSO.Equals(placedObjectTypeSOList[0]))
                {
                    rightPanel.SetActive(true);
                    GameObject.Find("panel_name").GetComponent<TextMeshProUGUI>().text="ТРК";
                    GameObject.Find("panel_par1").GetComponent<TextMeshProUGUI>().text="Скорость";
                    GameObject.Find("panel_pars1").GetComponent<TextMeshProUGUI>().text=selectedGO.dbData.par1.ToString() + " Л/C";
                    GameObject.Find("panel_par2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("panel_pars2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().ClearOptions();
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().AddOptions(dbT.FDhelp);
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().value = dbT.FDhelp.IndexOf(selectedGO.dbData.name);
                }
                if (placedObject.placedObjectTypeSO.Equals(placedObjectTypeSOList[2]))
                {
                    rightPanel.SetActive(true);
                    GameObject.Find("panel_name").GetComponent<TextMeshProUGUI>().text="Касса";
                    GameObject.Find("panel_par1").GetComponent<TextMeshProUGUI>().text="";
                    GameObject.Find("panel_pars1").GetComponent<TextMeshProUGUI>().text="";
                    GameObject.Find("panel_par2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("panel_pars2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().ClearOptions();
                    List<string> tmplist = new List<string>();
                    tmplist.Add("Касса");
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().AddOptions(tmplist);
                }
                if (placedObject.placedObjectTypeSO.Equals(placedObjectTypeSOList[1]))
                {
                    rightPanel.SetActive(true);
                    GameObject.Find("panel_name").GetComponent<TextMeshProUGUI>().text="Дорога";
                    GameObject.Find("panel_par1").GetComponent<TextMeshProUGUI>().text="";
                    GameObject.Find("panel_pars1").GetComponent<TextMeshProUGUI>().text="";
                    GameObject.Find("panel_par2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("panel_pars2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().ClearOptions();
                    List<string> tmplist = new List<string>();
                    tmplist.Add("Дорога");
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().AddOptions(tmplist);
                }
                if (placedObject.placedObjectTypeSO.Equals(placedObjectTypeSOList[3]))
                {
                    rightPanel.SetActive(true);
                    GameObject.Find("panel_name").GetComponent<TextMeshProUGUI>().text="Въезд";
                    GameObject.Find("panel_par1").GetComponent<TextMeshProUGUI>().text="";
                    GameObject.Find("panel_pars1").GetComponent<TextMeshProUGUI>().text="";
                    GameObject.Find("panel_par2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("panel_pars2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().ClearOptions();
                    List<string> tmplist = new List<string>();
                    tmplist.Add("Въезд");
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().AddOptions(tmplist);
                }
                if (placedObject.placedObjectTypeSO.Equals(placedObjectTypeSOList[4]))
                {
                    rightPanel.SetActive(true);
                    GameObject.Find("panel_name").GetComponent<TextMeshProUGUI>().text="Выезд";
                    GameObject.Find("panel_par1").GetComponent<TextMeshProUGUI>().text="";
                    GameObject.Find("panel_pars1").GetComponent<TextMeshProUGUI>().text="";
                    GameObject.Find("panel_par2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("panel_pars2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().ClearOptions();
                    List<string> tmplist = new List<string>();
                    tmplist.Add("Выезд");
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().AddOptions(tmplist);
                }
                if (placedObject.placedObjectTypeSO.Equals(placedObjectTypeSOList[5]))
                {
                    rightPanel.SetActive(true);
                    GameObject.Find("panel_name").GetComponent<TextMeshProUGUI>().text="Топливный Бак";
                    GameObject.Find("panel_par1").GetComponent<TextMeshProUGUI>().text="Вместимость";
                    GameObject.Find("panel_pars1").GetComponent<TextMeshProUGUI>().text=selectedGO.dbData.par1.ToString() + " Л";
                    GameObject.Find("panel_par2").GetComponent<TextMeshProUGUI>().text = "Тип топлива";
                    GameObject.Find("panel_pars2").GetComponent<TextMeshProUGUI>().text = selectedGO.dbData.fuel_name;
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().ClearOptions();
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().AddOptions(dbT.FThelp);
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().value = dbT.FThelp.IndexOf(selectedGO.dbData.name);
                }
                if (placedObject.placedObjectTypeSO.Equals(placedObjectTypeSOList[6]))
                {
                    rightPanel.SetActive(true);
                    GameObject.Find("panel_name").GetComponent<TextMeshProUGUI>().text="Табло";
                    GameObject.Find("panel_par1").GetComponent<TextMeshProUGUI>().text="";
                    GameObject.Find("panel_pars1").GetComponent<TextMeshProUGUI>().text="";
                    GameObject.Find("panel_par2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("panel_pars2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().ClearOptions();
                    List<string> tmplist = new List<string>();
                    tmplist.Add("Табло");
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().AddOptions(tmplist);
                }
            }
            else{
                rightPanel.SetActive(false);
                selectedGO=null;
            }
        }

        if (Input.GetMouseButtonDown(0) && !IsMouseOverUI() && serviceGrid.isInsideGrid(Mouse3D.GetMouseWorldPosition()))
        {
            if (placedObjectTypeSO != null){
                Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
                serviceGrid.GetXZ(mousePosition, out int x, out int z);

                Vector2Int placedObjectOrigin = new Vector2Int(x, z);
                placedObjectOrigin = serviceGrid.ValidateGridPosition(placedObjectOrigin);

                // Test Can Build
                List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);
                bool canBuild = true;
                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    if (!serviceGrid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild() || prefab == ObjectType.store || prefab == ObjectType.fuelDispencer|| prefab == ObjectType.infoTable)
                    {
                        Vector3 placedObjectWorldPosition = serviceGrid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y);
                        string buildingType = "";
                        switch (prefab)
                        {
                            case ObjectType.store:
                                buildingType = "кассу";
                                break;
                            case ObjectType.infoTable:
                                buildingType = "табло";
                                break;
                            case ObjectType.fuelDispencer:
                                buildingType = "ТРК";
                                break;
                        }
                        UtilsClass.CreateWorldTextPopup("Здесь нельзя строить " + buildingType, placedObjectWorldPosition);
                        canBuild = false;
                        break;
                    }
                }

                if (canBuild && isValidCount())
                {
                    Vector3 placedObjectWorldPosition = serviceGrid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y);
                    bool isTRK = true;
                    Vector3 offset = new Vector3();
                    offset = PlacedObjectTypeSO.GetNeighbourNodeStat(dir);
                    var neighborGr = serviceGrid.GetGridObject(placedObjectWorldPosition + offset * cellSize);
                    if (prefab != ObjectType.fuelTank)
                        isTRK = false;
                    if ((!isTRK) || ((neighborGr != null) && (neighborGr.dbData.id == 0)))
                    {
                        PlacedObject_Done placedObject = PlacedObject_Done.Create(placedObjectWorldPosition, placedObjectOrigin, dir, placedObjectTypeSO, prefab);
                        buildingCounter[(int)prefab]++;
                        if (placedObject.isStopable())
                        {
                            if (placedObject.isStopable())
                            {
                                switch (prefab)
                                {
                                    case ObjectType.fuelTank:
                                        offset = placedObject.placedObjectTypeSO.GetNeighbourNode(dir);
                                        fuelTankWaypoints.Add(placedObjectWorldPosition + offset * cellSize);
                                        if (!isCopy)
                                        {
                                            selectedGO = serviceGrid.GetGridObject(x, z);
                                            selectedGO.dbData = new ObjectParseWrapper();
                                            selectedGO.dbData.type = 1;
                                            selectedGO.dbData.curPar = dbT.FTList[0].volume;
                                            selectedGO.dbData.id = dbT.FTList[0].id;
                                            selectedGO.dbData.name = dbT.FTList[0].name;
                                            selectedGO.dbData.par1 = dbT.FTList[0].volume;
                                            selectedGO.dbData.fuel_id = dbT.FTList[0].fuel_id;
                                            selectedGO.dbData.fuel_name = dbT.FTList[0].fuel_name;//new ObjectPars(dbT.FTList[0]);
                                            neighborGr.dbData = new ObjectParseWrapper();
                                            neighborGr.dbData = selectedGO.dbData;
                                        }
                                        else
                                        {
                                            neighborGr.dbData = new ObjectParseWrapper();
                                            var dataS = selectedGO.dbData;
                                            selectedGO = serviceGrid.GetGridObject(x, z);
                                            selectedGO.dbData = dataS;
                                            neighborGr.dbData = selectedGO.dbData;
                                            isCopy = false;
                                        }
                                        break;
                                    case ObjectType.entrance:
                                        serviceEntranceWaypoint = placedObjectWorldPosition;
                                        break;
                                    case ObjectType.exit:
                                        serviceExitWaypoint = placedObjectWorldPosition;
                                        break;
                                }
                            }
                        }
                        foreach (Vector2Int gridPosition in gridPositionList)
                        {
                            serviceGrid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                        }
                        DeselectObjectType();
                    }
                    else
                    {
                        UtilsClass.CreateWorldTextPopup("Здесь нельзя построить еще один ТБ", placedObjectWorldPosition);
                    }


                }
                else if (!isValidCount()) {
                    Vector3 placedObjectWorldPosition = serviceGrid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y);
                    UtilsClass.CreateWorldTextPopup("Превышен предел построек", placedObjectWorldPosition);
                }
                else
                {
                    Vector3 placedObjectWorldPosition = serviceGrid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y);
                    UtilsClass.CreateWorldTextPopup("Здесь нельзя строить", placedObjectWorldPosition);
                }
            }
            else if (serviceGrid.GetGridObject(Mouse3D.GetMouseWorldPosition()).GetPlacedObject() != null)
            {
                int x, z;
                serviceGrid.GetXZ(Mouse3D.GetMouseWorldPosition(), out x, out z);
                var placedObject = serviceGrid.GetGridObject(x, z).GetPlacedObject();
                selectedGO = serviceGrid.GetGridObject(x, z);
                if (placedObject.placedObjectTypeSO.Equals(placedObjectTypeSOList[0]))
                {
                    rightPanel.SetActive(true);
                    GameObject.Find("panel_name").GetComponent<TextMeshProUGUI>().text = "ТРК";
                    GameObject.Find("panel_par1").GetComponent<TextMeshProUGUI>().text = "Скорость";
                    GameObject.Find("panel_pars1").GetComponent<TextMeshProUGUI>().text = selectedGO.dbData.par1.ToString() + " Л/C";
                    GameObject.Find("panel_par2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("panel_pars2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().ClearOptions();
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().AddOptions(dbT.FDhelp);
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().value = dbT.FDhelp.IndexOf(selectedGO.dbData.name);
                }
                if (placedObject.placedObjectTypeSO.Equals(placedObjectTypeSOList[2]))
                {
                    rightPanel.SetActive(true);
                    GameObject.Find("panel_name").GetComponent<TextMeshProUGUI>().text = "Касса";
                    GameObject.Find("panel_par1").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("panel_pars1").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("panel_par2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("panel_pars2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().ClearOptions();
                    List<string> tmplist = new List<string>();
                    tmplist.Add("Касса");
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().AddOptions(tmplist);
                }
                if (placedObject.placedObjectTypeSO.Equals(placedObjectTypeSOList[1]))
                {
                    rightPanel.SetActive(true);
                    GameObject.Find("panel_name").GetComponent<TextMeshProUGUI>().text = "Дорога";
                    GameObject.Find("panel_par1").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("panel_pars1").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("panel_par2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("panel_pars2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().ClearOptions();
                    List<string> tmplist = new List<string>();
                    tmplist.Add("Дорога");
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().AddOptions(tmplist);
                }
                if (placedObject.placedObjectTypeSO.Equals(placedObjectTypeSOList[3]))
                {
                    rightPanel.SetActive(true);
                    GameObject.Find("panel_name").GetComponent<TextMeshProUGUI>().text = "Въезд";
                    GameObject.Find("panel_par1").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("panel_pars1").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("panel_par2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("panel_pars2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().ClearOptions();
                    List<string> tmplist = new List<string>();
                    tmplist.Add("Въезд");
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().AddOptions(tmplist);
                }
                if (placedObject.placedObjectTypeSO.Equals(placedObjectTypeSOList[4]))
                {
                    rightPanel.SetActive(true);
                    GameObject.Find("panel_name").GetComponent<TextMeshProUGUI>().text = "Выезд";
                    GameObject.Find("panel_par1").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("panel_pars1").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("panel_par2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("panel_pars2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().ClearOptions();
                    List<string> tmplist = new List<string>();
                    tmplist.Add("Выезд");
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().AddOptions(tmplist);
                }
                if (placedObject.placedObjectTypeSO.Equals(placedObjectTypeSOList[5]))
                {
                    rightPanel.SetActive(true);
                    GameObject.Find("panel_name").GetComponent<TextMeshProUGUI>().text = "Топливный Бак";
                    GameObject.Find("panel_par1").GetComponent<TextMeshProUGUI>().text = "Вместимость";
                    GameObject.Find("panel_pars1").GetComponent<TextMeshProUGUI>().text = selectedGO.dbData.par1.ToString() + " Л";
                    GameObject.Find("panel_par2").GetComponent<TextMeshProUGUI>().text = "Тип топлива";
                    GameObject.Find("panel_pars2").GetComponent<TextMeshProUGUI>().text = selectedGO.dbData.fuel_name;
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().ClearOptions();
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().AddOptions(dbT.FThelp);
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().value = dbT.FThelp.IndexOf(selectedGO.dbData.name);
                }
                if (placedObject.placedObjectTypeSO.Equals(placedObjectTypeSOList[6]))
                {
                    rightPanel.SetActive(true);
                    GameObject.Find("panel_name").GetComponent<TextMeshProUGUI>().text = "Табло";
                    GameObject.Find("panel_par1").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("panel_pars1").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("panel_par2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("panel_pars2").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().ClearOptions();
                    List<string> tmplist = new List<string>();
                    tmplist.Add("Табло");
                    GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().AddOptions(tmplist);
                }
            }
            else
            {
                rightPanel.SetActive(false);
                selectedGO = null;
            }
        }
        if (Input.GetKeyDown(KeyCode.R)) {
           dir = PlacedObjectTypeSO.GetNextDir(dir);
       }
       if (Input.GetKeyDown(KeyCode.Alpha1)) { placedObjectTypeSO = placedObjectTypeSOList[0]; prefab = ObjectType.fuelDispencer; RefreshSelectedObjectType(); }
       if (Input.GetKeyDown(KeyCode.Alpha2)) { placedObjectTypeSO = placedObjectTypeSOList[1]; prefab = ObjectType.road; RefreshSelectedObjectType(); }
       if (Input.GetKeyDown(KeyCode.Alpha3)) { placedObjectTypeSO = placedObjectTypeSOList[2]; prefab = ObjectType.store; RefreshSelectedObjectType(); }
       if (Input.GetKeyDown(KeyCode.Alpha4)) { placedObjectTypeSO = placedObjectTypeSOList[3]; prefab = ObjectType.entrance; RefreshSelectedObjectType(); }
       if (Input.GetKeyDown(KeyCode.Alpha5)) { placedObjectTypeSO = placedObjectTypeSOList[4]; prefab = ObjectType.exit; RefreshSelectedObjectType(); }
       if (Input.GetKeyDown(KeyCode.Alpha6)) { placedObjectTypeSO = placedObjectTypeSOList[5]; prefab = ObjectType.fuelTank; RefreshSelectedObjectType(); }
       if (Input.GetKeyDown(KeyCode.F)) { var car = GameObject.Find("sedanpref"); car.transform.position = entranceWaypoint + Vector3.up * 3; car.GetComponent<Unit>().request();}
       if (Input.GetKeyDown(KeyCode.Q)) {
            if (isValidTopology()) { 
                 GridXZWrapper data = new GridXZWrapper(grid, serviceGrid, roadGrid, entranceWaypoint, exitWaypoint, serviceEntranceWaypoint, serviceExitWaypoint, fuelDispencerWaypoints, fuelTankWaypoints, storeWaypoint); 
                 string json = JsonUtility.ToJson(data); 
                 SaveLoadSystem.init(); 
                 SaveLoadSystem.save("try.txt", json);
                Debug.Log("Сохранение успешно");
            } else
            {
                //erc.Error("Не удалось сохранить");
                //Debug.Log("Не удалось сохранить");
            }
        }
       if (Input.GetKeyDown(KeyCode.Alpha0)) { DeselectObjectType(); }
   
   
       if (Input.GetMouseButtonDown(1) && grid.isInsideGrid(Mouse3D.GetMouseWorldPosition())) {
           Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
           if (grid.GetGridObject(mousePosition) != null) {
               // Valid Grid Position
               PlacedObject_Done placedObject = grid.GetGridObject(mousePosition).GetPlacedObject();
               if (placedObject != null) {
                    if (placedObject.isStopable())
                    {
                        ObjectType buildingType = placedObject.prefab;
                        switch (buildingType)
                        {
                            case ObjectType.fuelDispencer:
                                Vector3 waypoint = grid.GetWorldPosition(placedObject.getOrigin().x, placedObject.getOrigin().y);
                                Vector3 offset = placedObject.placedObjectTypeSO.GetNeighbourNode(placedObject.getDirection()) * grid.GetCellSize();
                                waypoint += offset;
                                fuelDispencerWaypoints.Remove(waypoint);
                                break;
                        }
                        buildingCounter[(int)placedObject.prefab]--;
                    };
                    // Demolish
                    placedObject.DestroySelf();
                    
                   List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
                   foreach (Vector2Int gridPosition in gridPositionList) {
                       grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                   }
               }
           }
       }

        if (Input.GetMouseButtonDown(1) && serviceGrid.isInsideGrid(Mouse3D.GetMouseWorldPosition()))
        {
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
            if (serviceGrid.GetGridObject(mousePosition) != null)
            {
                // Valid Grid Position
                PlacedObject_Done placedObject = serviceGrid.GetGridObject(mousePosition).GetPlacedObject();
                if (placedObject != null)
                {
                    if (placedObject.isStopable())
                    {
                        ObjectType buildingType = placedObject.prefab;
                        switch (buildingType)
                        {
                            case ObjectType.fuelTank:
                                Vector3 waypoint = serviceGrid.GetWorldPosition(placedObject.getOrigin().x, placedObject.getOrigin().y);
                                Vector3 offset = placedObject.placedObjectTypeSO.GetNeighbourNode(placedObject.getDirection()) * serviceGrid.GetCellSize();
                                waypoint += offset;
                                var neighborGr = grid.GetGridObject(waypoint);
                                neighborGr.dbData = new ObjectParseWrapper();
                                fuelTankWaypoints.Remove(waypoint);
                                break;
                        }
                        buildingCounter[(int)placedObject.prefab]--;
                    };
                    // Demolish
                    placedObject.DestroySelf();

                    List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
                    foreach (Vector2Int gridPosition in gridPositionList)
                    {
                        serviceGrid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                    }
                }
            }
        }
    }
    public void SaveTop(){
        var lol = GameObject.Find("InputSave").GetComponent<TMP_InputField>().text;
        if(GameObject.Find("InputSave").GetComponent<TMP_InputField>().text.Trim(' ')=="")
            erc.Error(16);
        else if (GameObject.Find("InputSave").GetComponent<TMP_InputField>().text.IndexOf(" ")!=-1)
            erc.Error(17);
        if (isValidTopology()) { 
                GridXZWrapper data = new GridXZWrapper(grid, serviceGrid, roadGrid, entranceWaypoint, exitWaypoint, serviceEntranceWaypoint, serviceExitWaypoint, fuelDispencerWaypoints, fuelTankWaypoints, storeWaypoint); 
                string json = JsonUtility.ToJson(data); 
                SaveLoadSystem.init(); 
                SaveLoadSystem.save(lol + ".txt", json);
                SaveLoadSystem.save(lol + ".txt", json);
                Debug.Log("Сохранение успешно");
                GameObject.Find("SceneChanger").GetComponent<SceneChanger>().changeScene("MainMenu");
            } else
            {
                //erc.Error("Не удалось сохранить");
                //Debug.Log("Не удалось сохранить");
            }
    }
    private bool isValidCount()
    {
        int[] validationArr = new int[7] { 6, int.MaxValue, 1, 2, 2, 5, 1 };
        return buildingCounter[(int)prefab] < validationArr[(int)prefab];
    }
    public void DeleteSelectedGO()
    {
        PlacedObject_Done placedObject = selectedGO.GetPlacedObject();
        if (placedObject.isStopable())
        {
            GridObject neighborGr;
            Vector3 waypoint;
            Vector3 offset;
            ObjectType buildingType = placedObject.prefab;
            switch (buildingType)
            {
                case ObjectType.fuelTank:
                    waypoint = serviceGrid.GetWorldPosition(placedObject.getOrigin().x, placedObject.getOrigin().y);
                    offset = placedObject.placedObjectTypeSO.GetNeighbourNode(placedObject.getDirection()) * serviceGrid.GetCellSize();
                    waypoint += offset;
                    neighborGr = grid.GetGridObject(waypoint);
                    neighborGr.dbData = new ObjectParseWrapper();
                    fuelTankWaypoints.Remove(waypoint);
                    break;
                case ObjectType.fuelDispencer:
                    waypoint = grid.GetWorldPosition(placedObject.getOrigin().x, placedObject.getOrigin().y);
                    offset = placedObject.placedObjectTypeSO.GetNeighbourNode(placedObject.getDirection()) * grid.GetCellSize();
                    waypoint += offset;
                    neighborGr = grid.GetGridObject(waypoint);
                    neighborGr.dbData = new ObjectParseWrapper();
                    fuelDispencerWaypoints.Remove(waypoint);
                    break;
            }
            buildingCounter[(int)placedObject.prefab]--;
        };
        // Demolish
        placedObject.DestroySelf();

        List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
        foreach (Vector2Int gridPosition in gridPositionList)
        {
            if (serviceGrid.GetGridObject(gridPosition.x, gridPosition.y) != null)
                serviceGrid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
        }
        foreach (Vector2Int gridPosition in gridPositionList)
        {
            if (grid.GetGridObject(gridPosition.x, gridPosition.y) != null)
                grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
        }
        rightPanel.SetActive(false);
        selectedGO = null;
    }
    public void CopySelectedGO()
    {

        isCopy = true;
        switch (selectedGO.placedObject.prefab)
        {
            case ObjectType.fuelDispencer:
                placedObjectTypeSO = placedObjectTypeSOList[0];
                prefab = ObjectType.fuelDispencer; RefreshSelectedObjectType();
                break;
            case ObjectType.road:
                placedObjectTypeSO = placedObjectTypeSOList[1];
                prefab = ObjectType.road; RefreshSelectedObjectType();
                break;
            case ObjectType.store:
                placedObjectTypeSO = placedObjectTypeSOList[2];
                prefab = ObjectType.store; RefreshSelectedObjectType();
                break;
            case ObjectType.entrance:
                placedObjectTypeSO = placedObjectTypeSOList[3];
                prefab = ObjectType.entrance; RefreshSelectedObjectType();
                break;
            case ObjectType.exit:
                placedObjectTypeSO = placedObjectTypeSOList[4];
                prefab = ObjectType.exit; RefreshSelectedObjectType();
                break;
            case ObjectType.fuelTank:
                placedObjectTypeSO = placedObjectTypeSOList[5];
                prefab = ObjectType.fuelTank; RefreshSelectedObjectType();
                break;
            case ObjectType.infoTable:
                placedObjectTypeSO = placedObjectTypeSOList[6];
                prefab = ObjectType.infoTable; RefreshSelectedObjectType();
                break;
        }
        rightPanel.SetActive(false);
    }
    public void DropDownSelectionChange()
    {
        if (selectedGO.dbData.type == 2)
        {
            selectedGO.dbData = new ObjectParseWrapper(dbT.FDList[GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().value]);
            GameObject.Find("panel_pars1").GetComponent<TextMeshProUGUI>().text = selectedGO.dbData.par1.ToString() + " Л/C";
        }
        if (selectedGO.dbData.type == 1)
        {
            selectedGO.dbData = new ObjectParseWrapper(dbT.FTList[GameObject.Find("DropdownType").GetComponent<TMP_Dropdown>().value]);
            GameObject.Find("panel_pars1").GetComponent<TextMeshProUGUI>().text = selectedGO.dbData.par1.ToString() + " Л";
            GameObject.Find("panel_pars2").GetComponent<TextMeshProUGUI>().text = selectedGO.dbData.fuel_name;
        }

    }
    private void DeselectObjectType() {
        placedObjectTypeSO = null; prefab = ObjectType.empty; RefreshSelectedObjectType();
    }

    private void RefreshSelectedObjectType() {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }
    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    public void SetPO(int code){
        if (code==0) { placedObjectTypeSO = placedObjectTypeSOList[0]; prefab = ObjectType.fuelDispencer; RefreshSelectedObjectType(); }
       if (code==1) { placedObjectTypeSO = placedObjectTypeSOList[1]; prefab = ObjectType.road; RefreshSelectedObjectType(); }
       if (code==2) { placedObjectTypeSO = placedObjectTypeSOList[2]; prefab = ObjectType.store; RefreshSelectedObjectType(); }
       if (code==3) { placedObjectTypeSO = placedObjectTypeSOList[3]; prefab = ObjectType.entrance; RefreshSelectedObjectType(); }
       if (code==4) { placedObjectTypeSO = placedObjectTypeSOList[4]; prefab = ObjectType.exit; RefreshSelectedObjectType(); }
       if (code==5) { placedObjectTypeSO = placedObjectTypeSOList[5]; prefab = ObjectType.fuelTank; RefreshSelectedObjectType(); }
       if (code==6) { placedObjectTypeSO = placedObjectTypeSOList[6]; prefab = ObjectType.infoTable; RefreshSelectedObjectType(); }
       isCopy = false;
    }


    public Vector3 GetMouseWorldSnappedPosition() {
        Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
        grid.GetXZ(mousePosition, out int x, out int z);

        if (placedObjectTypeSO != null) {
            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z);
            return placedObjectWorldPosition;
        } else {
            return mousePosition;
        }
    }

    public Quaternion GetPlacedObjectRotation() {
        if (placedObjectTypeSO != null) {
            return Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0);
        } else {
            return Quaternion.identity;
        }
    }

    public PlacedObjectTypeSO GetPlacedObjectTypeSO() {
        return placedObjectTypeSO;
    }
    public bool isValidTopology()
    {
        for (int i = 0; i < buildingCounter.Count; i++)
        {
            if (buildingCounter[i] == 0)
            {
                erc.Error("В топологии отсутсвует " + placedObjectTypeSOList[i].nameString);
                Debug.Log("В топологии отсутсвует " + placedObjectTypeSOList[i].nameString);
                return false;
            }
        }
        bool hasEntrance = false;
        bool hasExit = false;
        // Проверка входа и выхода главной сетки
        for (int i = 0; i < grid.GetWidth(); i++)
        {
            GridObject node = grid.GetGridObject(i, 0);
            ObjectType nodeType = ObjectType.empty;
            if (node.GetPlacedObject() != null)
            {
                nodeType = node.GetPlacedObject().prefab;
            }
            if (nodeType == ObjectType.exit)
            {
                hasExit = true;
            }
            if (nodeType == ObjectType.entrance && hasExit)
            {
                hasExit = true;
            }
            if (hasEntrance && !hasExit)
            {
                erc.Error("Неправильно поставлены вход и выход в АЗС");
                Debug.Log("Неправильно поставлены вход и выход в АЗС");
                return false;
            }
        }
        if (hasEntrance && hasExit)
        {
            return true;
        }
        hasEntrance = false;
        hasExit = false;
        // Проверка входа и выхода служебной сетки
        for (int i = 0; i < serviceGrid.GetWidth(); i++)
        {
            GridObject node = serviceGrid.GetGridObject(0, i);
            ObjectType nodeType = ObjectType.empty;
            if (node.GetPlacedObject() != null)
            {
                nodeType = node.GetPlacedObject().prefab;
            }
            if (nodeType == ObjectType.exit)
            {
                hasExit = true;
            }
            if (nodeType == ObjectType.entrance)
            {
                hasExit = true;
            }
            if (hasEntrance && !hasExit)
            {
                erc.Error("Неправильно поставлены вход и выход в служебной части АЗС");
                Debug.Log("Неправильно поставлены вход и выход в служебной части АЗС");
                return false;
            }
        }
        if (hasEntrance && hasExit)
        {
            return true;
        }

        Pathfinding pf = GetComponent<Pathfinding>();
        bool pathsAreValid = true;
        for (int i = 0; i < fuelDispencerWaypoints.Count; i++)
        {
            pathsAreValid = pf.canBuildPath(entranceWaypoint, fuelDispencerWaypoints[i], grid);
            if (!pathsAreValid)
            {
                erc.Error("Не все пути доезжают до ТРК");
                Debug.Log("Не все пути доезжают до ТРК");
                return false;
            }
            pathsAreValid = pathsAreValid = pf.canBuildPath(fuelDispencerWaypoints[i], storeWaypoint, grid);
            if (!pathsAreValid)
            {
                erc.Error("Не все пути доезжают до кассы");
                Debug.Log("Не все пути доезжают до кассы");
                return false;
            }
        }
        pathsAreValid = pf.canBuildPath(storeWaypoint, exitWaypoint, grid);
        if (!pathsAreValid)
        {
            erc.Error("Не все пути доезжают до выезда АЗС");
            Debug.Log("Не все пути доезжают до выезда АЗС");
            return false;
        }
        for (int i = 0; i < fuelTankWaypoints.Count; i++)
        {
            pathsAreValid = pf.canBuildPath(serviceEntranceWaypoint, fuelTankWaypoints[i], serviceGrid);
            if (!pathsAreValid)
            {
                erc.Error("Не все пути доезжают до ТБ");
                Debug.Log("Не все пути доезжают до ТБ");
                return false;
            }
            pathsAreValid = pf.canBuildPath(fuelTankWaypoints[i], serviceExitWaypoint, serviceGrid);
            if (!pathsAreValid)
            {
                erc.Error("Не все пути доезжают до выхода служебной части");
                Debug.Log("Не все пути доезжают до выхода служебной части");
                return false;
            }
        }
        int lengthOfStorePath = pf.getLengthOfPath(storeWaypoint, exitWaypoint, grid);
        for (int i = 0; i < fuelDispencerWaypoints.Count; i++)
        {
            int FDPathLength = pf.getLengthOfPath(fuelDispencerWaypoints[i], exitWaypoint, grid);
            if (lengthOfStorePath > FDPathLength)
            {
                erc.Error("Касса должна находиться ближе к выходу, чем ТРК");
                Debug.Log("Касса должна находиться ближе к выходу, чем ТРК");
                return false;
            }
        }

        return true;
    }
}
