using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllObsticles : MonoBehaviour
{
    public int xDim = LevelConfig.xObs, yDim = LevelConfig.yObs;
    public GameObject obsticle;
    public GameObject[,] obsticleMatrix;
    public bool[,] obsticlePattern;
    public bool obsticlesSet = false; 
    void Start()
    {
        obsticleMatrix = new GameObject[xDim, yDim];
        obsticlePattern = new bool[xDim, yDim];
        InitializeObsticles();
        //RandomizeObsticles();
        //SetObsticles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitializeObsticles()
    {
        GameObject tempObst;
        for (int i = 0; i < xDim; i++)
        {
            for (int j = 0; j < yDim; j++)
            {
                tempObst = Instantiate(obsticle, new Vector3(9f, 1f+(j *3f), -5f + (i * 4f)),Quaternion.identity );
                
                obsticleMatrix[i,j] = tempObst;
                tempObst.SetActive(false);
            }
        }
    }
    public void SetObsticles()
    {
        //clear all targets
        foreach (var item in obsticleMatrix)
        {
            item.SetActive(false);
        }
        //fill according to pattern
        for (int i = 0; i < xDim; i++)
        {
            for (int j = 0; j < yDim; j++)
            {
                if(obsticlePattern[i,j])obsticleMatrix[i,j].SetActive(true);
            }
        }
        obsticlesSet = true;
    }
    public void RandomizeObsticles()
    {
        obsticlePattern = new bool[xDim,yDim];
         for (int i = 0; i < xDim; i++)
        {
            for (int j = 0; j < yDim; j++)
            {
                if(Mathf.RoundToInt(Random.Range(0,1f)) == 1f) obsticlePattern[i,j] = true;
            }
        }
    }
    public void RandomizeObsticles(int maxObsticles)
    {
        if(maxObsticles>xDim*yDim)maxObsticles=xDim*yDim;
        int count =0;
        obsticlePattern = new bool[xDim,yDim];
        while(count<maxObsticles)
        {
            ramdomAgain:
            int i = Mathf.RoundToInt(Random.Range(0,xDim));
            int j = Mathf.RoundToInt(Random.Range(0,yDim));
            if(obsticlePattern[i,j]) goto ramdomAgain;
            else
            {
                obsticlePattern[i,j] = true;
                count++;
            }
        }
    }
}
