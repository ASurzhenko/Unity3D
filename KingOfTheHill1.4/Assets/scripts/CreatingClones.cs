using UnityEngine;
using System.Collections;

public class CreatingClones : MonoBehaviour {

    public OtherOpponent other;
    public Opponent opponent;
    public int otherCounter;
    public int opponentCounter;
    
    void Start()
    {
        StartCoroutine(CreatingOther());
        StartCoroutine(CreatingOpponent());
    }

    IEnumerator CreatingOther()
    {
        while (otherCounter < 10)
        {
            yield return new WaitForSeconds(Random.Range(6, 10));
            Instantiate(other);
            otherCounter++;
        }
    }

    IEnumerator CreatingOpponent()
    {
        while (opponentCounter < 10)
        {
            yield return new WaitForSeconds(Random.Range(6, 10));
            Instantiate(opponent);
            opponentCounter++;
        }
    }
}