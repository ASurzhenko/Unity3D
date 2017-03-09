using UnityEngine;
using System.Collections;

public class Parachute : Obj
{
    public Player player;
    public float speed, tilt;
    private float target = 0.45f;

    void Awake()
    {
        StartParachut();
    }

    public override void StartOpponents()
    {
        base.StartParachut();
    }
    
    void Update()
    {
        if (!Pause.paused)
        {
            if (transform.rotation.z >= target)
            {
                target = 0;
                transform.Rotate(Vector3.back * tilt);
            }
            else
            {
                target = 0.45f;
                transform.Rotate(Vector3.forward * tilt);
            }

            if (transform.position.y < -3.7f)
            {
                StartCoroutine(creatingParachutees());
            }
        }
    }

    IEnumerator creatingParachutees()
    {
        yield return new WaitForSeconds(Random.Range(6, 10));
        Instantiate(gameObject);
        Destroy(gameObject);
    }

    void OnMouseDown()
    {
        if (!Pause.paused)
        {
            if (PlayerPrefs.GetString("Sound") != "no")
            {
                GetComponent<AudioSource>().Play();
            }
            transform.position = new Vector3(2.0f, -5.0f);
            player.score += 1f;
        }
    }
}
