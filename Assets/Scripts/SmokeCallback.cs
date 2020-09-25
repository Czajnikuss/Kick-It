using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeCallback : MonoBehaviour
{
    private void OnParticleSystemStopped() 
    {
        Debug.Log("particle stoped");
        ObsticleHandler obsticle = GetComponentInParent<ObsticleHandler>();
        obsticle.SmokeEnded();
    }
}
