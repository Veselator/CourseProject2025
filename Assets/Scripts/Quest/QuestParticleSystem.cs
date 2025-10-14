using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestParticleSystem : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<ParticleSystem>().Play();
    }
}
