using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllTargets : MonoBehaviour
{
    public int xDim = LevelConfig.xTarg, yDim = LevelConfig.yTarg;
    public GameObject target;
    public GameObject[,] targetMatrix;
    public bool[,] targetPattern;
    public int targetNumber; 
    public bool targetsSet = false;
    void Start()
    {
        targetMatrix = new GameObject[xDim, yDim];
        targetPattern = new bool[xDim, yDim];
        InitializeTargets();
        //RandomizeTargets();
       // SetTargets();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitializeTargets()
    {
        GameObject tempTarget;
        for (int i = 0; i < xDim; i++)
        {
            for (int j = 0; j < yDim; j++)
            {
                tempTarget = Instantiate(target, new Vector3(1f, 1f+(j *2f), -5f + (i * 3f)),Quaternion.identity );
                tempTarget.SetActive(false);
                targetMatrix[i,j] = tempTarget;  
            }
        }
    }
    public void SetTargets()
    {
        targetNumber = 0;
        StartCoroutine("SetTargetAfterWait");
    }
    IEnumerator SetTargetAfterWait()
    {
        
        yield return null;
        
        //clear all targets
        foreach (var item in targetMatrix)
        {
            item.SetActive(false);
        }
        yield return null;
        //fill according to pattern
        for (int i = 0; i < xDim; i++)
        {
            for (int j = 0; j < yDim; j++)
            {
                if(targetPattern[i,j])
                {
                    targetNumber++;
                    targetMatrix[i,j].SetActive(true);
                    
                }
            }
        }
        targetsSet = true;
        yield return null;

    }
    public void RandomizeTargets()
    {
        targetPattern = new bool[xDim,yDim];
         for (int i = 0; i < xDim; i++)
        {
            for (int j = 0; j < yDim; j++)
            {
                if(Mathf.RoundToInt(Random.Range(0,1f)) == 1f) targetPattern[i,j] = true;
            }
        }
    }
    public void RandomizeTargets(int maxTarget)
    {
        if(maxTarget>xDim*yDim)maxTarget=xDim*yDim;
        int count =0;
        targetPattern = new bool[xDim,yDim];
         for (int i = 0; i < xDim; i++)
        {
            for (int j = 0; j < yDim; j++)
            {
                if(Mathf.RoundToInt(Random.Range(0,1f)) == 1f) 
                {
                    targetPattern[i,j] = true;
                    count++;
                    if(count>= maxTarget) return;
                }
            }
        }
    }
}
