using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Globalization;
using TMPro;
public class TopSys : MonoBehaviour
{
    public GameObject prefab;
    public GameObject content;
    public GameObject butt;
    public GameObject butt1;
    public ErrorScript erc;
    public GameObject setPanel;
    
    public void TopLoad(){
        //Загрузка топологии из файла
        
        List<string> ss =  SaveLoadSystem.loadFiles();
        // // Присвоение полей если надо
        // copy.GetComponentsInChildren<ObjectPars>()[0].id = ind;
        // copy.GetComponentsInChildren<ObjectPars>()[0].name = FTName.text;
        // copy.GetComponentsInChildren<ObjectPars>()[0].par1 = int.Parse(FTVolume.text);
        // copy.GetComponentsInChildren<ObjectPars>()[0].fuel_id = db.FuelList[fuelType.value].id;
        // copy.GetComponentsInChildren<ObjectPars>()[0].fuel_name = db.FuelList[fuelType.value].name;

        //Вывод топологии
        for (int i=0;i<ss.Count;i++){
            var copy = Instantiate(prefab, content.transform);
            copy.GetComponentsInChildren<LayoutElement>()[0].GetComponentInChildren<TextMeshProUGUI>().text = ss[i];
        }
        
        // setLinks(copy);
    }
    public void DeleteTops(GameObject Top){
        for (int i =0;i< GameObject.Find("TopContent").transform.childCount;i++){
            var del = GameObject.Find("TopContent").transform.GetChild(i).gameObject;
            Destroy(del);
        }
    }

    public void setPrefab(GameObject prefab) {
        GameObject.Find("TopSysM").GetComponent<TopSys>().prefab = prefab;
    }

    public void changeButton(TextMeshProUGUI item) {
        GameObject.Find("TopSysM").GetComponent<TopSys>().butt.GetComponentInChildren<TextMeshProUGUI>().text = "Загрузить " + item.text;
        GameObject.Find("TopSysM").GetComponent<TopSys>().butt1.GetComponentInChildren<TextMeshProUGUI>().text = "Редактировать " + item.text;  
    }
    public void TopsLoad(int code){
        
        var text1 = GameObject.Find("TopSysM").GetComponent<TopSys>().butt.GetComponentInChildren<TextMeshProUGUI>().text;
        if (GameObject.Find("TopSysM").GetComponent<TopSys>().butt.GetComponentInChildren<TextMeshProUGUI>().text.EndsWith("Выбрать"))
            erc.Error("Топология не выбрана, выберите топологию");
        else{
            
            if (code==0){
                var text = GameObject.Find("TopSysM").GetComponent<TopSys>().butt.GetComponentInChildren<TextMeshProUGUI>().text.Substring(10);
                SaveLoadSystem.fileName = text;
                setPanel.SetActive(true);
                GameObject.Find("TopSelector").SetActive(false);
            }
            else if (code==1){
                var text = GameObject.Find("TopSysM").GetComponent<TopSys>().butt1.GetComponentInChildren<TextMeshProUGUI>().text.Substring(14);
                SaveLoadSystem.fileName = text;
                SceneManager.LoadScene("CreationScene");
            }
            //GameObject.Find("SceneChanger").GetComponent<SceneChanger>().changeScene("SimulationScene");
            // GameObject.Find("TopSelector").SetActive(false);
            // GameObject.Find("SettingsPanel").SetActive(true);
        }
    }

    

}
