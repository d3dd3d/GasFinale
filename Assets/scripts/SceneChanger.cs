using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Globalization;
using TMPro;

public class SceneChanger : MonoBehaviour
{
    public ErrorScript erc;
    public InputField inf;
    public GameObject thisPanel;
    public GameObject TopSize;
    public GameObject BDpanel;
    public GameObject PanelMain;
    public GameObject DBTest;
    public GameObject DBMain;
    public GameObject TopSys;
    public int code;
    public string password = "";
    public void changeScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
    public void changePassword() {
        password = inf.text;
    }
    public void setCode(int c){
        code =c;
    }
    public void changetoProtectedScene()
    {
        if (password == "12345") {
            switch(code){
                case 0:
                    TopSize.SetActive(true);
                    thisPanel.SetActive(false);
                    break;
                case 1:
                    BDpanel.SetActive(true);
                    PanelMain.SetActive(false);
                    DBTest.SetActive(true);
                    DBMain.SetActive(true);
                    thisPanel.SetActive(false);
                    break;
                case 2:
                    TopSys.GetComponent<TopSys>().TopsLoad(1);
                    //SceneManager.LoadScene("CreationScene");
                    break;
            }
            
        }
        else if (password.Length>32){
            erc.Error(6);
        }
        else if (password.Length<7){
            erc.Error(7);
        }
        else{
            erc.Error(8);
        }
        
    }
    public void changetoCreate()
    {
        int width1 = 0;
        int width2 = 0;
        int height = 0;
        var y1 = false;
        var y2 = false;
        var y3 = false;
        y1 = int.TryParse(GameObject.Find("WidthAZSIn").GetComponent<TMP_InputField>().text,out width1);
        y2 = int.TryParse(GameObject.Find("WidthServIn").GetComponent<TMP_InputField>().text,out width2);
        y3 = int.TryParse(GameObject.Find("Height").GetComponent<TMP_InputField>().text,out height);
        if (y1&&y2&&y3){
            if(width1>0&&width2>0&&height>0){
                if(width1>=12&&width1<=30){
                    if(width2>=3&&width2<=5){
                        if(height>=8&&height<=20){
                            SaveLoadSystem.width1 = width1;
                            SaveLoadSystem.width2 = width2;
                            SaveLoadSystem.height = height;
                            SceneManager.LoadScene("CreationScene");
                        }
                        else{
                            erc.Error("Диапазон высоты АЗС от 8 до 20");
                        }
                    }
                    else{
                        erc.Error("Диапазон длины служебной области от 3 до 5");
                    }
                }
                else{
                    erc.Error("Диапазон длины АЗС от 12 до 30");
                }
            }
            else{
                erc.Error("Числа должны быть положительными");
            }
        }
        else{
            erc.Error("Неверный формат числа");
        }
        
    }
    public void NonDetermGo() {
        var z1=false;
        var z2=false;
        var z3=false;
        double inChance = 0;
        var y3 = false;
        y3 = double.TryParse(GameObject.Find("ChanceField").GetComponent<TMP_InputField>().text,out inChance);
        if (y3){
            if (inChance>0){
                if(inChance<=1){
                    z2=true;
                }
                else{
                    erc.Error(14);
                }
            }
            else{
                erc.Error(3);
            }
        }
        else{
            erc.Error(4);
        }
        CultureInfo enUS = new CultureInfo("ru-RU");
        DateTime time = DateTime.Now;
        var y4 = false;
        y4 = DateTime.TryParse(GameObject.Find("StartingTime").GetComponent<TMP_InputField>().text,out time);
        if (y4){
            z3=true;
        }
        else{
            erc.Error(4);
        }
        if (GameObject.Find("NonDetermSettings").GetComponentsInChildren<Toggle>()[0].isOn==true){
            int left_bord = 0;
            int right_bord = 0;
            var y1 = false;
            var y2 = false;
            y1 = int.TryParse(GameObject.Find("LeftBorder").GetComponent<TMP_InputField>().text,out left_bord);
            y2 = int.TryParse(GameObject.Find("RightBorder").GetComponent<TMP_InputField>().text,out right_bord);
            if (y1&&y2){
                if(left_bord>0&&right_bord>0){
                    if (left_bord<right_bord){
                        if ((left_bord<=10)&&(right_bord<=10)){
                            z1=true;
                            if (z1&&z2&&z3){
                                RandomLaws.param1 = (double)left_bord;
                                RandomLaws.param2 = (double)right_bord;
                                RandomLaws.zk=new RandomLaws.Ravn();
                                RandomLaws.inChance=inChance;
                                RandomLaws.timeGlobal=time;
                                RandomLaws.timeGlobalOrigin=time;
                                SceneManager.LoadScene("SimulationScene");
                            }
                        }
                        else{
                            erc.Error(11);
                        }
                    }
                    else{
                        erc.Error(9);
                    }
                }
                else{
                    erc.Error(10);
                }
            }
            else{
                erc.Error(4);
            }
        }
        else if (GameObject.Find("NonDetermSettings").GetComponentsInChildren<Toggle>()[1].isOn==true){
            int disp = 0;
            int expect = 0;
            var y1 = false;
            var y2 = false;
            y1 = int.TryParse(GameObject.Find("Dispersion").GetComponent<TMP_InputField>().text,out disp);
            y2 = int.TryParse(GameObject.Find("Expectation").GetComponent<TMP_InputField>().text,out expect);
            if (y1&&y2){
                if(disp>0&&expect>0){
                    if ((disp<=2)&&(expect<=5)){
                        z1=true;
                        if (z1&&z2&&z3){
                            RandomLaws.param1 = (double)expect;
                            RandomLaws.param2 = (double)disp;
                            RandomLaws.inChance=inChance;
                            RandomLaws.zk=new RandomLaws.Norm();
                            RandomLaws.timeGlobal=time;
                            RandomLaws.timeGlobalOrigin=time;
                            SceneManager.LoadScene("SimulationScene");
                        }
                    }
                    else{
                        erc.Error(12);
                    }
                }
                else{
                    erc.Error(10);
                }
            }
            else{
                erc.Error(4);
            }
        }
        else if(GameObject.Find("NonDetermSettings").GetComponentsInChildren<Toggle>()[2].isOn==true) {
            double intens = 0;
            var y1 = false;
            y1 = double.TryParse(GameObject.Find("Intensity").GetComponent<TMP_InputField>().text,out intens);
            if (y1){
                if(intens>0){
                    if ((intens>=1)&&(intens<=5)){
                        z1=true;
                        if (z1&&z2&&z3){
                            RandomLaws.param1 = intens;
                            RandomLaws.inChance=inChance;
                            RandomLaws.zk=new RandomLaws.Exp();
                            RandomLaws.timeGlobal=time;
                            RandomLaws.timeGlobalOrigin=time;
                            SceneManager.LoadScene("SimulationScene");
                            
                        }
                    }
                    else{
                        erc.Error(13);
                    }
                }
                else{
                    erc.Error(10);
                }
            }
            else{
                erc.Error(4);
            }
        }
    }
    public void DetermGo(){
        int timeBtwSpawnsSet = 0;
        var y1 = false;
        var z1 = false;
        var z2 = false;
        var z3 = false;
        y1 = int.TryParse(GameObject.Find("TimeChanceField").GetComponent<TMP_InputField>().text,out timeBtwSpawnsSet);
        if (y1){
            if (timeBtwSpawnsSet>0){
                if(timeBtwSpawnsSet>=1&&timeBtwSpawnsSet<=10){
                    z1 = true;
                }
                else{
                    erc.Error("Диапазон допустимых значений для детерминированного потока от 1 до 10 секунд");
                }
            }
            else{
                erc.Error("Число должно быть положительным");
            }
        }
        else{
            erc.Error(4);
        }


        double inChance = 0;
        var y3 = false;
        y3 = double.TryParse(GameObject.Find("ChanceField").GetComponent<TMP_InputField>().text,out inChance);
        if (y3){
            if (inChance>0){
                if(inChance<=1){
                    z2 = true;
                }
                else{
                    erc.Error(14);
                }
            }
            else{
                erc.Error(3);
            }
        }
        else{
            erc.Error(4);
        }
        CultureInfo enUS = new CultureInfo("ru-RU");
        DateTime time = DateTime.Now;
        var y4 = false;
        y4 = DateTime.TryParse(GameObject.Find("StartingTime").GetComponent<TMP_InputField>().text,out time);
        if (y4){
            z3 = true;
        }
        else{
            erc.Error(4);
        }
        if (z1&&z2&&z3){
            RandomLaws.inChance=inChance;
            RandomLaws.isDeterm=true;
            RandomLaws.timeBtw=timeBtwSpawnsSet;
            RandomLaws.timeGlobal=time;
            RandomLaws.timeGlobalOrigin=time;
            SceneManager.LoadScene("SimulationScene");
            //запись параметров
        }
    }

    public void exit()
    {
        Application.Quit();
    }
}
