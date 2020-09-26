using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TalionApps.Utils;

[RequireComponent(typeof (Rigidbody))]
public class BallHandler : MonoBehaviour
{
    public bool setAblaze, electryfy;
    public ParticleSystem fire, electricSparks;
    public PlayManager playManager;
    public Rigidbody rigidbodyThis;
    float startDrag;
    bool beengDraged;
    public float magnitude;
    
    public Vector3 startPos;
    private float shootEffectTimer=0, shootEffectCooldown = 1f;
    void Start()
    {
        playManager = FindObjectOfType<PlayManager>();
        rigidbodyThis = GetComponent<Rigidbody>();
        startPos = transform.position;
        fire.gameObject.SetActive(setAblaze);
        fire.Stop(true);
        electricSparks.gameObject.SetActive(electryfy);
        electricSparks.Stop(true);
    }
    private void Update() {
        if(beengDraged && Input.GetMouseButtonUp(0)&& playManager.inputAllowed)
        {
            Shoot();
        }

        //debug
        if(Input.GetMouseButtonDown(0))
        {
            // Debug.Log(UtilsClass.GetMouseWorldPositionWithZ(camera));
        }
        shootEffectTimer+=Time.deltaTime;
        if(shootEffectTimer >= shootEffectCooldown)
        {
            
            if(setAblaze && rigidbodyThis.velocity.magnitude <=3)
            {
                fire.Stop(true);
                shootEffectTimer = 0;
            }
            if(electryfy && rigidbodyThis.velocity.magnitude <=3)
            {
                electricSparks.Stop(true);
                shootEffectTimer = 0;
            }

        }
        
    }
    
    private void OnMouseDown() {
        
        startDragCalcultion();
        beengDraged = true;
        
    }
    private void OnDisable() {
        
        playManager.OnBallDisabled();
    }
    

    public void startDragCalcultion()
    {
        Debug.Log("start");
        startDrag = Time.realtimeSinceStartup;
        
    }

    public void Shoot()
    {   
        if(playManager.movesPossible)
        {
            if(setAblaze) fire.Play(true);
            if(electricSparks) electricSparks.Play(true);
            Vector3 force = (UtilsClass.GetMouseWorldPositionWithZ() -this.transform.position )*magnitude;
           // Debug.Log(UtilsClass.GetMouseWorldPositionWithZ());
            rigidbodyThis.AddForce(force);
            playManager.MoveMade();

        }

    }
}
