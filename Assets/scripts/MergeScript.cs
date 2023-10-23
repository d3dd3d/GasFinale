using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MergeScript : MonoBehaviour
{
    public ErrorScript erc;
    public void Save(){
        var lol = GameObject.Find("InputSave").GetComponent<TMP_InputField>().text;
        if(GameObject.Find("InputSave").GetComponent<TMP_InputField>().text.Trim(' ')=="")
            erc.Error(16);
        else if (GameObject.Find("InputSave").GetComponent<TMP_InputField>().text.IndexOf(" ")!=-1)
            erc.Error(17);
    }
}
