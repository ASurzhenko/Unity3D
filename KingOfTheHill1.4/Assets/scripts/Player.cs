using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System;

public class Player : Obj
{
    public Opponent opponent;
    public OtherOpponent other;
    public MouseReaction reaction;
    public Transform particleSys;
    private Vector3 oppPosition;
    private Vector3 initialCameraPosition;
    public float score;
    private float bank;
    private bool inProcess;
    private bool button;
    private bool firstMove;
    public bool move;
    public bool youLose;
    public int position;
    public bool takeJackpot;
    public bool giveYourPosition;
    private int bestPos;
    private int addMoneyCount;
    private bool add;
    public bool modeBool;
    public string mode;
    public WWW www;
    public JSONObject JO;
    
    void Awake()
    {
        StartPlayer();
        //score = 100;
        score = PlayerPrefs.GetFloat("Score");
        addMoneyCount = PlayerPrefs.GetInt("Add Money Count");
    }

    public override void StartPlayer()
    {
        initialCameraPosition = Camera.main.transform.position;
        base.StartPlayer();
    }

    void Update()
    {
        mode = (!modeBool) ? "off" : "on";

        if (score >= 10)
        {
            add = false;
        }

        if (position > bestPos)
        {
            bestPos = position;
        }

        if (PlayerPrefs.GetInt("Best Position") < bestPos)
        {
            PlayerPrefs.SetInt("Best Position", bestPos);
        }

        PlayerPrefs.SetFloat("Score", score);

        if (PlayerPrefs.GetFloat("Max Score") < score)
        {
            PlayerPrefs.SetFloat("Max Score", score);
        }

        if (PlayerPrefs.GetFloat("Max Jackpot") < reaction.getJackpot())
        {
            PlayerPrefs.SetFloat("Max Jackpot", reaction.getJackpot());
        }

        PlayerPrefs.SetInt("Add Money Count", addMoneyCount);

        firstMove = (bank == 0) ? true : false;

        if (giveYourPosition && position > 0)
        {
            reaction.positionsList.Add(position);
        }
        giveYourPosition = false;

        if (takeJackpot && position == reaction.maxPosition)
        {
            Instantiate(particleSys, transform.position, transform.rotation);
            score += reaction.jackpotToWinner;
            score += bank;
            StartCoroutine(LoseMove());
        }
        takeJackpot = false;

        if (move && oppPosition == transform.position)  // if (Vector3.Distance(oppPosition, transform.position) < 0.001f) - it must be more precisely
        {
            oppPosition = new Vector3(0, 0, 0);
            StartCoroutine(Move());
        }
        move = false;

        if (youLose && oppPosition == transform.position)
        {
            youLose = false;
            oppPosition = new Vector3(0, 0, 0);
            StartCoroutine(LoseMove());
        }

        if (Input.GetMouseButtonDown(0) && !inProcess && !Pause.paused)
        {
            if (score < 10 && position == 0)
            {
                add = true;
            }
            else
            {
                RaycastHit hit;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (hit.transform.tag == "Opponent" && (!other.busyPositions.Contains(hit.transform.position))
                        && Mathf.Approximately(transform.position.y, hit.transform.position.y))
                    {
                        if (PlayerPrefs.GetString("Sound") != "no")
                        {
                            GameObject.Find("Move Audio").GetComponent<AudioSource>().Play();
                        }

                        button = false;
                        transform.position = hit.transform.position;
                        StartCoroutine(wait());

                        if (firstMove)
                        {
                            bank = 20;
                            score -= 10;
                        }
                        else
                        {
                            bank *= 2;
                        }
                    }
                }
            }
        }
    }

    public bool IsEmpty()
    {
        return !Physics.Raycast(transform.position, transform.up, 0.5f);
    }

    IEnumerator Move()
    {
        if (PlayerPrefs.GetString("Sound") != "no")
        {
            GetComponent<AudioSource>().Play();
        }

        GameObject[] opponents = GameObject.FindGameObjectsWithTag("Opponent");
        for (int i = 0; i < opponents.Length; i++)
        {
            opponents[i].GetComponent<Opponent>().setOtherPosition(transform.position);
            opponents[i].GetComponent<Opponent>().youLose = true;
        }

        while (!IsEmpty()) //condition to go up if the up field if empty
        {
            transform.position += new Vector3(1.0f, 0, 0);
            IsEmpty();
        }

        transform.position += new Vector3(0, 0.6f, 0);

        float toJackpot = bank * 0.01f;
        bank -= toJackpot;
        reaction.setJackpot(reaction.getJackpot() + toJackpot);
        position++;
        reaction.newPosition = position;

        button = true;

        yield return null;
    }

    IEnumerator LoseMove()
    {
        Camera.main.transform.position = initialCameraPosition;
        StartPlayer();

        bank = 0;
        position = 0;

        yield return null;
    }

    IEnumerator wait()
    {
        inProcess = true;
        yield return new WaitForSeconds(6);
        inProcess = false;
    }

    private void OnGUI()
    {
        GUIStyle styleTime = new GUIStyle();

        styleTime.fontSize = 16;

        var strScore = string.Format("{0:0.##}", score);
        var strBank = string.Format("{0:0.##}", bank);
        var strJackpot = string.Format("{0:0.##}", reaction.getJackpot());

        styleTime.normal.textColor = Color.yellow;
        GUI.Label(new Rect(10, 10, 150, 50), "Player Score: " + strScore, styleTime);
        styleTime.normal.textColor = Color.white;
        GUI.Label(new Rect(10, 35, 100, 50), "Bank: " + strBank, styleTime);
        styleTime.normal.textColor = Color.cyan;
        GUI.Label(new Rect(170, 10, 100, 50), "Jackpot: " + strJackpot, styleTime);

        styleTime.normal.textColor = Color.cyan;
        GUI.Label(new Rect(490, 10, 100, 50), "Players on-line: " + reaction.playersOnline, styleTime);

        styleTime.normal.textColor = Color.white;
        GUI.Label(new Rect(490, 35, 100, 50), "Max Score: " + string.Format("{0:0.##}", PlayerPrefs.GetFloat("Max Score")), styleTime);

        if (position >= reaction.currentPosition && position > 0)
        {
            styleTime.normal.textColor = Color.red;
            GUI.Label(new Rect(170, 35, 100, 50), "Your are on TOP!", styleTime);
        }
        else
        {
            styleTime.normal.textColor = Color.white;
            GUI.Label(new Rect(170, 35, 100, 50), "Your position: " + string.Format("{0:0.##}", position), styleTime);
        }

        styleTime.normal.textColor = Color.white;
        GUI.Label(new Rect(170, 60, 100, 50), "Best position: " + string.Format("{0:0.##}", reaction.currentPosition), styleTime);

        styleTime.normal.textColor = Color.white;
        GUI.Label(new Rect(490, 60, 100, 50), "Max Jackpot: " + string.Format("{0:0.##}", PlayerPrefs.GetFloat("Max Jackpot")), styleTime);

        styleTime.normal.textColor = Color.red;
        GUI.Label(new Rect(300, 10, 150, 50), "Hit the Jackpot in: " + string.Format("{0:00}", reaction.minutes) + " : " +
            string.Format("{0:00}", (int)reaction.getTimeRemaining()), styleTime);

        GUI.color = Color.white;
        GUI.enabled = false;
        if (bank != 0 && button)
        {
            GUI.enabled = true;
        }
        if (GUI.Button(new Rect(10, 65, 90, 60), "Get the bank") && !Pause.paused)
        {
            if (PlayerPrefs.GetString("Sound") != "no")
            {
                GameObject.Find("Bank Audio").GetComponent<AudioSource>().Play();
            }
            score += bank;
            bank = 0;
            position = 0;
            Camera.main.transform.position = initialCameraPosition;
            StartPlayer();
        }

        GUI.color = Color.white;
        GUI.enabled = false;
        if (!inProcess)
        {
            GUI.enabled = true;
        }
        if (GUI.Button(new Rect(690, 10, 90, 60), "Quit") && !Pause.paused)
        {
            if (PlayerPrefs.GetString("Sound") != "no")
            {
                GameObject.Find("Click Audio").GetComponent<AudioSource>().Play();
            }
            if (bank != 0)
            {
                score += bank;
            }
            Application.LoadLevel("main");
        }

        GUI.color = Color.white;
        if (GUI.Button(new Rect(690, 85, 90, 20), "Add Funds") && !Pause.paused)
        {
            score += 100;
            addMoneyCount++;
            if (PlayerPrefs.GetString("Sound") != "no")
            {
                GameObject.Find("Click Audio").GetComponent<AudioSource>().Play();
            }
        }
        if (add)
        {
            styleTime.fontSize = 20;
            styleTime.normal.textColor = Color.red;
            GUI.Label(new Rect(Screen.width * 0.5f - 100, Screen.height * 0.5f - 12.5f, 200, 25), "Пожалуйста, пополните счет", styleTime);
        }

        GUI.Label(new Rect(15, 145, 90, 40), "Chit mode " + mode);
        if (GUI.Button(new Rect(10, 165, 90, 40), "Chit mode") && !Pause.paused)
        {
            if (PlayerPrefs.GetString("Sound") != "no")
            {
                GameObject.Find("Click Audio").GetComponent<AudioSource>().Play();
            }

            modeBool = !modeBool;
        }
    }

    public double getScore()
    {
        return score;
    }

    public double getBank()
    {
        return bank;
    }

    public void setBank(float bank)
    {
        this.bank = bank;
    }

    public void setOppPosition(Vector3 oppPosition)
    {
        this.oppPosition = oppPosition;
    }
}
