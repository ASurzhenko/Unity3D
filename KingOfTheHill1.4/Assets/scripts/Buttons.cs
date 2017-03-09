using UnityEngine;
using System.Collections;

public class Buttons : MonoBehaviour {

    public Sprite layer_blue, layer_red;
    public GameObject m_on, m_off;

    void Start()
    {
        if(gameObject.name == "Sound")
        {
            if(PlayerPrefs.GetString("Sound") == "no")
            {
                m_on.SetActive(false);
                m_off.SetActive(true);
            }
            else
            {
                m_on.SetActive(true);
                m_off.SetActive(false);
            }
        }
    }

    void OnMouseDown()
    {
        GetComponent<SpriteRenderer>().sprite = layer_red;
    }

    void OnMouseUp()
    {
        GetComponent<SpriteRenderer>().sprite = layer_blue;
    }

    void OnMouseUpAsButton()
    {
        if (PlayerPrefs.GetString("Sound") != "no")
        {
            GameObject.Find("Click Audio").GetComponent<AudioSource>().Play();
        }
        switch (gameObject.name)
        {
            case "Play":
                Application.LoadLevel("Game");
                break;
            case "Quit":
                Application.Quit();
                break;
            case "Sound":
                if (PlayerPrefs.GetString("Sound") != "no")
                {
                    PlayerPrefs.SetString("Sound", "no");
                    m_on.SetActive(false);
                    m_off.SetActive(true);
                }
                else
                {
                    PlayerPrefs.SetString("Sound", "yes");
                    m_on.SetActive(true);
                    m_off.SetActive(false);
                }
                break;

        }
    }
}