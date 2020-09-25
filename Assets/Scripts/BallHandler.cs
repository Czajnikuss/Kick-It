using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TalionApps.Utils;

[RequireComponent(typeof (Rigidbody))]
public class BallHandler : MonoBehaviour
{
    public bool setAblaze, freeze;
    public PlayManager playManager;
    public Rigidbody rigidbody;
    float startDrag;
    bool beengDraged;
    public float magnitude;
    //public Camera camera;
    public Vector3 startPos;
    void Start()
    {
        playManager = FindObjectOfType<PlayManager>();
        rigidbody = GetComponent<Rigidbody>();
        startPos = transform.position;
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
            Vector3 force = (UtilsClass.GetMouseWorldPositionWithZ() -this.transform.position )*magnitude;
            Debug.Log(UtilsClass.GetMouseWorldPositionWithZ());
            rigidbody.AddForce(force);
            playManager.MoveMade();
        }

    }
}
