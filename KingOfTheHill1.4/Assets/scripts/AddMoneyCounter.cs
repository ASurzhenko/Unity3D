using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AddMoneyCounter : MonoBehaviour {

    void Start()
    {
        GetComponent<Text>().text = PlayerPrefs.GetInt("Add Money Count").ToString();
    }
}
