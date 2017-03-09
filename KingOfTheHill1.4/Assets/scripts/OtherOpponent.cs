using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OtherOpponent : Obj
{
    public Opponent opponent;
    public MouseReaction reaction;
    public Transform particleSys;
    public List<Vector3> busyPositions;
    private Vector3 oppPosition;
    public bool move;
    public bool youLose;
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
        StartCoroutine(Jump());
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
            GameObject setJP = GameObject.FindGameObjectWithTag("MainCamera");
            setJP.GetComponent<MouseReaction>().positionsList.Add(position);
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
       
        if(move == true && oppPosition == transform.position)
        {
            oppPosition = new Vector3(0, 0, 0);
            StartCoroutine(Move());
        }
        move = false;

        if(youLose == true && oppPosition == transform.position)
        {
            youLose = false;
            oppPosition = new Vector3(0, 0, 0);
            StartCoroutine(LoseMove());
        }
    }

    IEnumerator Jump()
    {
        GameObject opponentToJump;
        GameObject[] opponentTagList;
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(7, 7));

            opponentTagList = GameObject.FindGameObjectsWithTag("Opponent");

            opponentToJump = opponentTagList[Random.Range(0, opponentTagList.Length)];

            if (Mathf.Approximately(transform.position.y, opponentToJump.transform.position.y) && (!busyPositions.Contains(opponentToJump.transform.position)))
            {
                transform.position = opponentToJump.transform.position;

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

    public bool IsEmpty()
    {
        return !Physics.Raycast(transform.position, transform.up, 0.5f);
    }

    public bool IsStartEmpty()
    {
        return !Physics.Raycast(transform.position, -transform.up, 0.5f);
    }

    public void setOppPosition(Vector3 oppPosition)
    {
        this.oppPosition = oppPosition;
    }

    IEnumerator Move()
    {
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
        position++;

        GameObject setJP = GameObject.FindGameObjectWithTag("MainCamera");
        setJP.GetComponent<MouseReaction>().setJackpot(setJP.GetComponent<MouseReaction>().getJackpot() + toJackpot);
        setJP.GetComponent<MouseReaction>().setNewPosition(position);

        yield return null;
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
}