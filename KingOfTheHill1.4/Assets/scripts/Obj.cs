using UnityEngine;
using System.Collections;

public class Obj : MonoBehaviour {

    public virtual void StartOpponents()
    {
        transform.position = new Vector3(Mathf.Round(Random.Range(-3.0f, 4.0f)), -2.1f);

        while (!IsEmpty())
        {
            transform.position += new Vector3(1.0f, 0, 0);
            IsEmpty();
        }
        transform.position -= new Vector3(0, 0.6f, 0);
    }

    public virtual void StartPlayer()
    {
        transform.position = new Vector3(-4.0f, -2.7f);
    }

    public bool IsEmpty()
    {
        return !Physics.Raycast(transform.position, -transform.up , 0.5f);
    }

    public virtual void StartParachut()
    {
        transform.position = new Vector3(Mathf.Round(Random.Range(-4.0f, 14.0f)), 10.0f, -0.4f);
    }
}
