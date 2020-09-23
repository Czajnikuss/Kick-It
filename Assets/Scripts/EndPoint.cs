using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    public bool shouldRotate = true;
    public Vector3 startPos;
    PlayManager playManager;
    public ParticleSystem boom;
    MeshRenderer meshRenderer;

    void Awake()
    {
        playManager = FindObjectOfType<PlayManager>();
        meshRenderer = GetComponent<MeshRenderer>();
        startPos = transform.position;
        if(shouldRotate)RotateSelf();
        
    }
    private void OnEnable() 
    {
        meshRenderer.enabled = true;
    }
    private void RotateSelf()
    {
        LeanTween.rotateAroundLocal(gameObject, Vector3.up, 360f, 3f).setLoopClamp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        
        meshRenderer.enabled = false;
        boom.gameObject.SetActive(true);
        boom.Play();
        
                
    }
    public void ParticleSystemComplete()
    {
        playManager.TargetHit();
        gameObject.SetActive(false);
    }

}
