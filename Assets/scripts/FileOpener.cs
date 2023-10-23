using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileOpener : MonoBehaviour
{
    public void clickHTML()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Aboutsystem.html");
        System.Diagnostics.Process.Start(filePath);
    }

}
