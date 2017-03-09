using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour 
{
    public static bool paused = false;
    
    void Start()
    {
        if (PlayerPrefs.GetString("Sound") != "no")
        {
            GetComponent<AudioSource>().Play();
        }
    }﻿

    private void OnGUI()
    {
        GUIStyle styleTime = new GUIStyle();
        if (paused)
        {
            styleTime.fontSize = 25;
            styleTime.normal.textColor = Color.red;
            GUI.Label(new Rect(Screen.width * 0.5f - 100, Screen.height * 0.5f - 12.5f, 200, 25), "PAUSED", styleTime);
        }

        if (GUI.Button(new Rect(10, 210, 90, 40), "Pause"))
        {
            paused = !paused;
            if (paused)
            {
                Time.timeScale = 0;
                GetComponent<AudioSource>().Pause();
            }
            else
            {
                Time.timeScale = 1;
                GetComponent<AudioSource>().Play();
            }
        }
    }
}
