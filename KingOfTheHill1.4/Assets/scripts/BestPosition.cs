using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BestPosition : MonoBehaviour
{

    void Start()
    {
        GetComponent<Text>().text = PlayerPrefs.GetInt("Best Position").ToString();
    }
}