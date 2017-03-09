using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MaxJP : MonoBehaviour {

    void Start()
    {
        GetComponent<Text>().text = string.Format("{0:0.##}", PlayerPrefs.GetFloat("Max Win Jackpot"));
    }
}
