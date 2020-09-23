using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticleHandler : MonoBehaviour
{
    public ParticleSystem fire;
    // Start is called before the first frame update
    private void Awake() {
        fire.Stop(true);
    }

    private void OnCollisionEnter(Collision other) {
        fire.Play(true);
    }
    private void OnDisable() {
        fire.Stop(true);
    }
}
