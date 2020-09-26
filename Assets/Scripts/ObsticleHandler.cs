using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObsticleMovementType
{
    Ramdom, 
    OnlyY,
    OnlyZ,
    None
}

public class ObsticleHandler : MonoBehaviour
{
    public ObsticleMovementType thisObsticleMovementType;
    public ParticleSystem fire, toAshes, electricSparks;
    public bool isMoving = true;
    public float startx=9f;
    public Material standardMaterial, beginBurnMaterial, endingBurnMaterial;
    public float burnTime, timeToRebirth, electryficationTime;
    public MeshRenderer meshRenderer;
    PlayManager playManager;
    public BoxCollider boxCollider;
    public Rigidbody rigidbodyThis;
    bool collidingStill = false;
    Vector3 startPos;
    
    private void Start() {
        
        boxCollider = GetComponent<BoxCollider>();
        playManager = FindObjectOfType<PlayManager>();
        meshRenderer = GetComponent<MeshRenderer>();
        fire.Stop(true);
        toAshes.Stop(true);
        electricSparks.Stop(true);
        meshRenderer.material = standardMaterial;

    }
    private void OnEnable() 
    {
        startPos = transform.position;
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;
        fire.Stop(true);
        toAshes.Stop(true);
        electricSparks.Stop(true);
        meshRenderer.material = standardMaterial;
        rigidbodyThis.isKinematic = false;
        if(isMoving)
        {
            StartCoroutine("RandomizeMovement");
        }    
    }
    IEnumerator RandomizeMovement()
    {
        while (true)
        {   
            if(thisObsticleMovementType == ObsticleMovementType.Ramdom)RandomMove();
            else if(thisObsticleMovementType == ObsticleMovementType.OnlyY)OnlyY();
            else if(thisObsticleMovementType == ObsticleMovementType.OnlyZ)OnlyZ();
            else if(thisObsticleMovementType == ObsticleMovementType.None)rigidbodyThis.isKinematic = true;
            
            yield return new WaitForSeconds(1f);
        }
    }

    public void RandomMove()
    {

            rigidbodyThis.AddForce(new Vector3(0, Random.Range(-3f,2f), Random.Range(-2f,2f)),ForceMode.Impulse);
    }
    public void OnlyY()
    {
        rigidbodyThis.AddForce(new Vector3(0, Random.Range(-2f,2f),0),ForceMode.Impulse);
   
    }
    public void OnlyZ()
    {
        rigidbodyThis.AddForce(new Vector3(0,0, Random.Range(-2f,2f)),ForceMode.Impulse);
   
    }

    private void OnCollisionExit(Collision other) {
        collidingStill=false;
    }
    private void OnCollisionStay(Collision other) {
            if(!other.gameObject.GetComponent<BallHandler>())
            {        
                if(!collidingStill)
                {    
                    Vector3 opositeDirection = rigidbodyThis.velocity * -1f;
                    collidingStill = true;
                    StartCoroutine("StillCollision", opositeDirection);
                }
                collidingStill = true;
            }    
    }
    IEnumerator StillCollision(Vector3 opositDirection)
    {
        yield return new WaitForSeconds(0.5f);
        while(collidingStill)
        {
            rigidbodyThis.AddForce(opositDirection);
            yield return new WaitForSeconds(0.5f);        
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(RandomizeMovement());
    }
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.GetComponent<BallHandler>())
        {
            BallHandler ballHandler = other.gameObject.GetComponent<BallHandler>();
            if(ballHandler.setAblaze)StartCoroutine(SetOnFire());
            if(ballHandler.electryfy)StartCoroutine(ElectryfyObsticle());
        }
        else if(other.gameObject.GetComponent<ObsticleHandler>())
                {
 
                    rigidbodyThis.AddForce(rigidbodyThis.velocity * -1f);
                    StartCoroutine("StillCollision", rigidbodyThis.velocity * -1f);
  
                }
        else
        {
            GoBack();
        }
        
    }
    IEnumerator ElectryfyObsticle()
    {
        Debug.Log("electric");
        electricSparks.Play(true);
        rigidbodyThis.isKinematic = true;
        yield return new WaitForSeconds(electryficationTime);
        electricSparks.Stop(true);
        if(thisObsticleMovementType != ObsticleMovementType.None) rigidbodyThis.isKinematic = false;
        StartCoroutine(RandomizeMovement());
    }
    private void GoBack()
    {
        rigidbodyThis.AddForce(rigidbodyThis.velocity * -1f);
                   
    }
    IEnumerator SetOnFire()
    {
        fire.Play(true);
        yield return new WaitForSeconds(burnTime/3f);
        meshRenderer.material = beginBurnMaterial;
        yield return new WaitForSeconds(burnTime/3f);
        meshRenderer.material = endingBurnMaterial;
        
        yield return new WaitForSeconds(burnTime/3f);
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
        fire.Stop(true);
        StopCoroutine(RandomizeMovement());

        rigidbodyThis.angularVelocity = Vector3.zero;
        rigidbodyThis.velocity = Vector3.zero;
        rigidbodyThis.isKinematic = true;
        toAshes.gameObject.SetActive(true);
        toAshes.Play(true);
    }
    private void OnDisable() {
        StopCoroutine("RandomizeMovement");
        fire.Stop(true);
        boxCollider.enabled = true;
        meshRenderer.material = standardMaterial;
    }
    public void SmokeEnded()
    {
        
        StartCoroutine ("CooldownToRebith");
        
    }
    IEnumerator CooldownToRebith()
    {
        int thisLevel = playManager.currentLevel;
        
        yield return new WaitForSeconds(timeToRebirth);
        
        if(thisLevel == playManager.currentLevel)
        {
            
            meshRenderer.enabled = true;
            meshRenderer.material = standardMaterial;
            boxCollider.enabled = true;
            rigidbodyThis.isKinematic = false;
            gameObject.SetActive(true);
            transform.position = startPos;
            if(isMoving) StartCoroutine(RandomizeMovement());
                  
        }
        yield return null;

    }
}
