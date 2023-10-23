using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Data;
using UnityEngine;

public class ErrorScript : MonoBehaviour
{
    public GameObject errText;
    public GameObject errCan;

    public void Error(int erCode){
        switch(erCode){
            case 1:{
                errCan.SetActive(true);
                errText.GetComponent<TextMeshProUGUI>().text="Данный тип топлива где-то используется, пожалуйста уберите зависимости";
                break;
            }
            case 2:{
                errCan.SetActive(true);
                errText.GetComponent<TextMeshProUGUI>().text="Такое название уже используется";
                break;
            }
            case 3:{
                errCan.SetActive(true);
                errText.GetComponent<TextMeshProUGUI>().text="Число должно быть положительным и больше нуля";
                break;
            }
            case 4:{
                errCan.SetActive(true);
                errText.GetComponent<TextMeshProUGUI>().text="Неверный формат числа";
                break;
            }
            case 5:{
                errCan.SetActive(true);
                errText.GetComponent<TextMeshProUGUI>().text="Неверный формат названия";
                break;
            }
            case 6:{
                errCan.SetActive(true);
                errText.GetComponent<TextMeshProUGUI>().text="Слишком длинный пароль";
                break;
            }
            case 7:{
                errCan.SetActive(true);
                errText.GetComponent<TextMeshProUGUI>().text="Слишком короткий пароль";
                break;
            }
            case 8:{
                errCan.SetActive(true);
                errText.GetComponent<TextMeshProUGUI>().text="Неверно введен пароль";
                break;
            }
            case 9:{
                errCan.SetActive(true);
                errText.GetComponent<TextMeshProUGUI>().text="Левая граница не должна быть больше правой";
                break;
            }
            case 10:{
                errCan.SetActive(true);
                errText.GetComponent<TextMeshProUGUI>().text="Числа должны быть положительными";
                break;
            }
            case 11:{
                errCan.SetActive(true);
                errText.GetComponent<TextMeshProUGUI>().text="Допустимый диапазон значений от 1 до 10";
                break;
            }
            case 12:{
                errCan.SetActive(true);
                errText.GetComponent<TextMeshProUGUI>().text="Допустимый диапазон значений для дисперсии от 1 до 2 и от 1 до 5 для мат. ожидания";
                break;
            }
            case 13:{
                errCan.SetActive(true);
                errText.GetComponent<TextMeshProUGUI>().text="Допустимый диапазон значений для интенсивности от 1 до 5";
                break;
            }
            case 14:{
                errCan.SetActive(true);
                errText.GetComponent<TextMeshProUGUI>().text="Допустимый диапазон значений для вероятности от 0 до 1";
                break;
            }
            case 15:{
                errCan.SetActive(true);
                errText.GetComponent<TextMeshProUGUI>().text="Здесь нельзя строить";
                break;
            }
            case 16:{
                errCan.SetActive(true);
                errText.GetComponent<TextMeshProUGUI>().text="Неверное название";
                break;
            }
            case 17:{
                errCan.SetActive(true);
                errText.GetComponent<TextMeshProUGUI>().text="В названии не должно быть пробелов";
                break;
            }
        }
    }
    public void Error(string erCode){
                errCan.SetActive(true);
                errText.GetComponent<TextMeshProUGUI>().text=erCode;
            
        
    }
}
