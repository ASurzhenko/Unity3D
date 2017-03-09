using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseReaction : MonoBehaviour
{

    public Player player;
    public OtherOpponent other;
    public Opponent opponent;
    private float jackpot;
    private bool startTimer;
    public float jackpotToWinner;
    public List<int> positionsList = new List<int>();
    public List<int> maxPositionsList = new List<int>();
    public int maxPosition = 0;
    public int currentPosition = 0;
    public int newPosition = 0;
    public int minutes;
    private float seconds;
    public int playersOnline;

    void Start()
    {
        timer(3, 00);
    }

    void Update()
    {
        plOnline();
        
        if (startTimer)
        {
            seconds -= Time.deltaTime;
            if (seconds < 0)
            {
                minutes--;
                seconds = 60;
            }
        }

        if(newPosition > currentPosition)
        {
            timer(3, 00);
            currentPosition = newPosition;
        }

        if (minutes < 0)
        {
            if (PlayerPrefs.GetString("Sound") != "no")
            {
                GetComponent<AudioSource>().Play();
            }

            timer(3, 00);

            StartCoroutine(getAllPositions());

            StartCoroutine(getWinnerPositions());
        }

        foreach (int i in positionsList)
        {
            if (i > maxPosition)
            {
                maxPosition = i;
            }
        }

        if (PlayerPrefs.GetFloat("Max Win Jackpot") < jackpotToWinner)
        {
            PlayerPrefs.SetFloat("Max Win Jackpot", jackpotToWinner);
        }
    }

    private void timer(int minutes, float seconds)
    {
        this.minutes = minutes;
        this.seconds = seconds;
        startTimer = true;
    }

    IEnumerator getAllPositions()
    {
        
        GameObject players = GameObject.FindGameObjectWithTag("Player");
        players.GetComponent<Player>().giveYourPosition = true;
        
        GameObject[] opponents = GameObject.FindGameObjectsWithTag("Opponent");
        for (int i = 0; i < opponents.Length; i++)
        {
            opponents[i].GetComponent<Opponent>().giveYourPosition = true;
        }

        GameObject[] others = GameObject.FindGameObjectsWithTag("OtherOpponent");
        for (int i = 0; i < others.Length; i++)
        {
            others[i].GetComponent<OtherOpponent>().giveYourPosition = true;
        }

        yield return null;
    }

    IEnumerator getWinnerPositions()
    {
        yield return new WaitForSeconds(1f);

        foreach (int i in positionsList)
        {
            if (i == maxPosition)
            {
                maxPositionsList.Add(i);
            }
        }
        
        jackpotToWinner = jackpot / maxPositionsList.Count;

        Debug.Log(jackpot);
        Debug.Log(maxPositionsList.Count);
        Debug.Log(jackpotToWinner);
             

        GameObject players = GameObject.FindGameObjectWithTag("Player");
        players.GetComponent<Player>().takeJackpot = true;

        GameObject[] opponents = GameObject.FindGameObjectsWithTag("Opponent");
        for (int i = 0; i < opponents.Length; i++)
        {
            opponents[i].GetComponent<Opponent>().takeJackpot = true;
        }

        GameObject[] others = GameObject.FindGameObjectsWithTag("OtherOpponent");
        for (int i = 0; i < others.Length; i++)
        {
            others[i].GetComponent<OtherOpponent>().takeJackpot = true;
        }

        positionsList.Clear();
        maxPositionsList.Clear();
                
        yield return new WaitForSeconds(0.5f);  

        maxPosition = 0;
        StartCoroutine(getAllPositions());
        yield return new WaitForSeconds(0.5f);  
        positionsList.Clear();
        currentPosition = maxPosition;
        jackpot = 0;
        jackpotToWinner = 0;
        newPosition = 0;
    }

    private void plOnline()
    {
        GameObject[] opponents = GameObject.FindGameObjectsWithTag("Opponent");

        GameObject[] others = GameObject.FindGameObjectsWithTag("OtherOpponent");

        playersOnline = opponents.Length + others.Length + 1;
    }

    public float getTimeRemaining()
    {
        return seconds;
    }

    public void setNewPosition(int newPosition)
    {
        this.newPosition = newPosition;
    }

    public int getNewPosition()
    {
        return newPosition;
    }

    public float getJackpot()
    {
        return jackpot;
    }

    public void setJackpot(float jackpot)
    {
        this.jackpot = jackpot;
    }
}
