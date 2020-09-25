using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticleHandler : MonoBehaviour
{
    public ParticleSystem fire, toAshes;
    public bool isMoving = true;
    public float startx=9f;
    public Material standardMaterial, beginBurnMaterial, endingBurnMaterial;
    public float burnTime, timeToRebirth;
    public MeshRenderer meshRenderer;
    PlayManager playManager;
    public BoxCollider boxCollider;
    
    private void Start() {
        boxCollider = GetComponent<BoxCollider>();
        playManager = FindObjectOfType<PlayManager>();
        meshRenderer = GetComponent<MeshRenderer>();
        fire.Stop(true);
        toAshes.Stop(true);
        meshRenderer.material = standardMaterial;

    }
    private void OnEnable() 
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;
        fire.Stop(true);
        toAshes.Stop(true);
        meshRenderer.material = standardMaterial;
        if(isMoving)
        {
            RandomMove();
        }    
    }

    public void RandomMove()
    {
        LeanTween.cancel(gameObject);
        Vector3 randomDirection = new Vector3(startx, Random.Range(1f, 7f), Random.Range(-1f, 7));
        LeanTween.moveZ(this.gameObject, Random.Range(1f, 7f), Random.Range(1f, 3f));
        LeanTween.moveY(this.gameObject, Random.Range(1f, 7f), Random.Range(1f, 3f) ).setOnComplete(()=> RandomMove() );
    }
    private void OnCollisionStay(Collision other) {
        
    }
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.GetComponent<BallHandler>())
        {
            BallHandler ballHandler = other.gameObject.GetComponent<BallHandler>();
            if(ballHandler.setAblaze)StartCoroutine("SetOnFire");
        }
        else 
        {
            if(other.gameObject.GetComponent<ObsticleHandler>())
                {
                    
                    Vector3 opositeDir = transform.position - other.gameObject.transform.position ;
                    Debug.DrawRay(transform.position, opositeDir, Color.red, 2f);
                    LeanTween.moveY(this.gameObject,Mathf.Clamp(opositeDir.y, 1f, 7f), Random.Range(1f, 3f) );
                    LeanTween.moveZ(this.gameObject,Mathf.Clamp(opositeDir.z, 1f, 7f), Random.Range(1f, 3f) );
                    
                    transform.position = new Vector3(startx, transform.position.y, transform.position.z );
                }
        }
        
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
        LeanTween.cancel(gameObject);
        toAshes.gameObject.SetActive(true);
        toAshes.Play(true);
    }
    private void OnDisable() {
        fire.Stop(true);
        boxCollider.enabled = true;
        meshRenderer.material = standardMaterial;
    }
    public void SmokeEnded()
    {
        
        StartCoroutine ("cooldownToRebith");
        
    }
    IEnumerator cooldownToRebith()
    {
        int thisLevel = playManager.currentLevel;
        
        yield return new WaitForSeconds(timeToRebirth);
        
        if(thisLevel == playManager.currentLevel)
        {
            
            meshRenderer.enabled = true;
            meshRenderer.material = standardMaterial;
            boxCollider.enabled = true;
            gameObject.SetActive(true);
            if(isMoving) RandomMove();
                  
        }
        yield return null;

    }
}
