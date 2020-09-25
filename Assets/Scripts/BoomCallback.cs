using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomCallback : MonoBehaviour
{
    EndPoint endPoint;
    private void Awake() {
        endPoint = GetComponentInParent<EndPoint>();
    }

    private void OnParticleSystemStopped() {
       // Debug.Log("target out");
        endPoint.ParticleSystemComplete();
        

    }
}
