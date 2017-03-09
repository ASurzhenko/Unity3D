using UnityEngine;
using System.Collections;

public class ParticleSystem : MonoBehaviour
{

    void LateUpdate()
    {
        if (!GetComponent<UnityEngine.ParticleSystem>().IsAlive())
        {
            Destroy(gameObject);﻿
        }
    }
}
