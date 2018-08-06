using System.Collections;
using UnityEngine;

public class particleSystemController : MonoBehaviour
{
    private ParticleSystem system;

    public void Start()
    {
        system = GetComponent<ParticleSystem>();
    }

    public void FixedUpdate()
    {
        if ((!global.ongoingGame) && (system != null) && (system.isPlaying))
            system.Pause(true);
        else if ((system != null) && (system.isPaused)) 
            system.Play(true);
    }
}
