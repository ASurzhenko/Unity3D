using UnityEngine;
using System.Collections;

public class Opponent : Obj
{

    public OtherOpponent other;
    public Player player;
    public MouseReaction reaction;
    Vector3 currentPosition;
    public bool start;
    public Transform particleSys;
    public bool youLose;
    private Vector3 otherPosition;
    private float bank;
    public float score;
    private bool firstMove;
    public int position;
    public bool takeJackpot;
    public bool giveYourPosition;
    
    void Start()
    {
        //score = Random.Range(50, 200);
        score = 100;
        position = 0;
    }

    void Awake()
    {
        StartOpponents();
    }

    public override void StartOpponents()
    {
        base.StartOpponents();
    }

    void Update()
    {
        firstMove = (bank == 0) ? true : false;

        if (giveYourPosition && position > 0)
        {
            GameObject setPos = GameObject.FindGameObjectWithTag("MainCamera");
            setPos.GetComponent<MouseReaction>().positionsList.Add(position);
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

        if (start)
        {
            start = false;
            StartCoroutine(Go());
        }

        if (youLose == true && otherPosition == transform.position)
        {
            youLose = false;
            otherPosition = new Vector3(0, 0, 0);
            StartCoroutine(LoseMove());
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (firstMove)
        {
            bank = 20;
            score -= 10;
        }
        else
        {
            bank *= 2;
        }

        start = true;

        Instantiate(particleSys, transform.position, transform.rotation);

        currentPosition = transform.position;

        GameObject[] others = GameObject.FindGameObjectsWithTag("OtherOpponent");

        for (int i = 0; i < others.Length; i++)
        {
            others[i].GetComponent<OtherOpponent>().busyPositions.Add(currentPosition);
        }
        
        StartCoroutine(RemoveFromBusyPositions());
    }

    public bool IsEmpty()
    {
        return !Physics.Raycast(transform.position, transform.up, 0.5f);
    }

    public bool IsStartEmpty()
    {
        return !Physics.Raycast(transform.position, -transform.up, 0.5f);
    }

    IEnumerator RemoveFromBusyPositions()
    {
        yield return new WaitForSeconds(5);

        GameObject[] others = GameObject.FindGameObjectsWithTag("OtherOpponent");

        for (int i = 0; i < others.Length; i++)
        {
            others[i].GetComponent<OtherOpponent>().busyPositions.Remove(currentPosition);
        }
    }

    public IEnumerator Go()
    {
        if ((player.mode == "on" && transform.position != player.transform.position) || player.mode == "off")
        {
            int winner = (int)Random.RandomRange(0.0f, 101.0f);

            yield return new WaitForSeconds(5.0f);

            if (winner % 2 == 0) // otherOpponent wins
            {
                GameObject[] others = GameObject.FindGameObjectsWithTag("OtherOpponent");
                for (int i = 0; i < others.Length; i++)
                {
                    others[i].GetComponent<OtherOpponent>().move = true;
                    others[i].GetComponent<OtherOpponent>().setOppPosition(transform.position);
                }
                GameObject pl = GameObject.FindGameObjectWithTag("Player");
                pl.GetComponent<Player>().move = true;
                pl.GetComponent<Player>().setOppPosition(transform.position);
            }
            else // opponent wins
            {
                if (PlayerPrefs.GetString("Sound") != "no" && transform.position == player.transform.position)
                {
                    GetComponent<AudioSource>().Play();
                }

                GameObject[] others = GameObject.FindGameObjectsWithTag("OtherOpponent");
                for (int i = 0; i < others.Length; i++)
                {
                    others[i].GetComponent<OtherOpponent>().youLose = true;
                    others[i].GetComponent<OtherOpponent>().setOppPosition(transform.position);
                }
                GameObject pl = GameObject.FindGameObjectWithTag("Player");
                pl.GetComponent<Player>().youLose = true;
                pl.GetComponent<Player>().setOppPosition(transform.position);

                while (!IsEmpty())
                {
                    transform.position += new Vector3(1.0f, 0, 0);
                    IsEmpty();
                }
                transform.position += new Vector3(0, 0.6f, 0);

                float toJackpot = bank * 0.01f;
                bank -= toJackpot;
                position++;

                GameObject setJP = GameObject.FindGameObjectWithTag("MainCamera");
                setJP.GetComponent<MouseReaction>().setJackpot(setJP.GetComponent<MouseReaction>().getJackpot() + toJackpot);
                setJP.GetComponent<MouseReaction>().setNewPosition(position);
            }
        }
        else
        {
            yield return new WaitForSeconds(5.0f);
            GameObject pl = GameObject.FindGameObjectWithTag("Player");
            pl.GetComponent<Player>().move = true;
            pl.GetComponent<Player>().setOppPosition(transform.position);
        }
    }

    public void setOtherPosition(Vector3 otherPosition)
    {
        this.otherPosition = otherPosition;
    }

    IEnumerator LoseMove()
    {
        transform.position = new Vector3(Mathf.Round(Random.Range(-3.0f, 4.0f)), -2.1f);

        while (!IsStartEmpty())
        {
            transform.position += new Vector3(1.0f, 0, 0);
            IsStartEmpty();
        }
        transform.position -= new Vector3(0, 0.6f, 0);

        bank = 0;
        position = 0;

        yield return null;
    }

    public float getBank()
    {
        return bank;
    }

    public void setBank(float bank)
    {
        this.bank = bank;
    }

    public float getScore()
    {
        return score;
    }

    public void setScore(float score)
    {
        this.score = score;
    }
}