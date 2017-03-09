using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurrentScore : MonoBehaviour
{

    void Start()
    {
        GetComponent<Text>().text = string.Format("{0:0.##}", PlayerPrefs.GetFloat("Score"));
    }
}
