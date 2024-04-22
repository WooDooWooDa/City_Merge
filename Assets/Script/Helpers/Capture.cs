using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Capture : MonoBehaviour
{
    private void CaptureScreen()
    {
        int count = Directory.GetFiles(Application.persistentDataPath + "/capture/").Length;
        ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/capture/capture" + count + ".png");
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(20, 20, 150, 75), "Capture")) {
            CaptureScreen();
        }
    }
}
