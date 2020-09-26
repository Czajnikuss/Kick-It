﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[System.Serializable]
public class LevelConfig
{
    public static int xTarg = 5, yTarg = 4;
    public bool[,] targetPattern;
    public static int xObs = 4, yObs = 3;
    public bool[,] obsticlePattern;
    public ObsticleMovementType levelObsticlesMovementType; 
    public LevelConfig(bool[,] targetPatern, bool[,] obsticlePattern, ObsticleMovementType movementType)
    {
        this.targetPattern = targetPatern;
        this.obsticlePattern = obsticlePattern;
        this.levelObsticlesMovementType = movementType;
        xTarg = 5;
        yTarg = 4;
        xObs = 4;
        yObs = 3;
    }
}

public class PlayManager : MonoBehaviour
{
    public List<LevelConfig> allLevels = new List<LevelConfig>();
    public TextMeshProUGUI movesText, ballsText, targetCaunterText;
    public int moves, startMoves, currentLevel;
    private float timer, suspensionTime = 0.5f;
    public bool inputAllowed=false;
    
    public bool movesPossible = true;
    BallHandler ball;
    EndPoint endPoint;
    AllObsticles allObsticles;
    AllTargets allTargets;
    int ballNumber, targetNumber;
    public GameObject winPanel, losePanel, gamePanel,mainMenuPanel, ballPrefab;

    void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        gamePanel.SetActive(true);
        mainMenuPanel.SetActive(true);

        currentLevel = PlayerPrefs.GetInt("lastLevel",0);
        
        moves=startMoves;
        movesText.text = "Moves: " + moves.ToString();
        
        GameObject tempBall = Instantiate(ballPrefab, new Vector3(16f, 1f, 0.3f), Quaternion.identity);
        ball = tempBall.GetComponent<BallHandler>();
        
        endPoint = FindObjectOfType<EndPoint>();
        allObsticles = FindObjectOfType<AllObsticles>();
        allTargets = FindObjectOfType<AllTargets>();
        
        CreateLevelsList(50);
        
    }
    IEnumerator ResetFrameAfter()
    {
        yield return new WaitForSeconds(0.5f);
        
        ResetThisLevel();
    }
    public void MoveMade()
    {
        moves--;
        movesText.text = "Moves: " + moves.ToString();
        if(moves <= 0)
        {
            movesPossible = false;
            StartCoroutine("CheckVelocityToDisable");
        }
    }
    IEnumerator CheckVelocityToDisable()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            if(ball.rigidbodyThis.velocity.magnitude<=1.5f)
            {
                ball.gameObject.SetActive(false);
            }
            
        }
    }
    public void NewBall()
    {
        StopCoroutine("CheckVelocityToDisable");
        ballNumber--;
        ShowBalls();
        moves=startMoves;
        movesText.text = "Moves: " + moves.ToString();
        movesPossible = true;
        ball.gameObject.SetActive(true);
        ball.transform.position = ball.startPos;
        ball.rigidbodyThis.velocity =  Vector3.zero;
        ball.rigidbodyThis.angularVelocity = Vector3.zero;
    }
    public void ResetToRandom()
    {
        moves=startMoves;
        movesText.text = "Moves: " + moves.ToString();
        movesPossible = true;
        ball.gameObject.SetActive(true);
        ball.transform.position = ball.startPos;

        ball.rigidbodyThis.velocity =  Vector3.zero;
        ball.rigidbodyThis.angularVelocity = Vector3.zero;
        allObsticles.RandomizeObsticles();
        allObsticles.levelObsticleMovmentType = (ObsticleMovementType)(int)Random.Range(0, 3);
        allObsticles.SetObsticles();
        allTargets.RandomizeTargets();
        allTargets.SetTargets();
       
        targetNumber = allTargets.targetNumber;
        ShowTargetsAmaunt();
        gamePanel.SetActive(true);
        losePanel.SetActive(false);
        winPanel.SetActive(false);
        
        ballNumber = allTargets.targetNumber + 1;
        ShowBalls();
        timer = 0;
        inputAllowed = false;
    }
    public void ResetThisLevel(int levelIndex)
    {
        if(levelIndex>allLevels.Count)ResetToRandom();

        moves=startMoves;
        movesText.text = "Moves: " + moves.ToString();
        movesPossible = true;
        ball.gameObject.SetActive(true);
        ball.transform.position = ball.startPos;

        ball.rigidbodyThis.velocity =  Vector3.zero;
        ball.rigidbodyThis.angularVelocity = Vector3.zero;
        
        allObsticles.obsticlePattern = allLevels[levelIndex].obsticlePattern;
        allObsticles.levelObsticleMovmentType = allLevels[levelIndex].levelObsticlesMovementType;
        allObsticles.SetObsticles();
        allTargets.targetPattern = allLevels[levelIndex].targetPattern;
        allTargets.targetsSet = false;
        allTargets.SetTargets();
        
        gamePanel.SetActive(true);
        losePanel.SetActive(false);
        winPanel.SetActive(false);
        Time.timeScale = 1;
        StartCoroutine("TillTargetsSet");
        timer = 0;
        inputAllowed = false;
        
    }
    public void ResetThisLevel()
    {
        if(currentLevel>allLevels.Count)ResetToRandom();

        moves=startMoves;
        movesText.text = "Moves: " + moves.ToString();
        movesPossible = true;
        ball.gameObject.SetActive(true);
        ball.transform.position = ball.startPos;

        ball.rigidbodyThis.velocity =  Vector3.zero;
        ball.rigidbodyThis.angularVelocity = Vector3.zero;
        
        allObsticles.obsticlePattern = allLevels[currentLevel].obsticlePattern;
        allObsticles.levelObsticleMovmentType = allLevels[currentLevel].levelObsticlesMovementType;
        allObsticles.SetObsticles();
        allTargets.targetPattern = allLevels[currentLevel].targetPattern;
        allTargets.targetsSet = false;
        allTargets.SetTargets();
        
        gamePanel.SetActive(true);
        losePanel.SetActive(false);
        winPanel.SetActive(false);
        Time.timeScale = 1;
        StartCoroutine("TillTargetsSet");
        timer = 0;
        inputAllowed = false;
        
    }
    IEnumerator TillTargetsSet()
    {
        while(true)
        {
            if(allTargets.targetsSet)
            {
                targetNumber = allTargets.targetNumber;
                ShowTargetsAmaunt();
                ballNumber = allTargets.targetNumber + 1;
                ShowBalls();
                yield return null;
                break;
            }
            else yield return null;

        }
    }    

    
    
    void Update()
    {
        timer+=Time.deltaTime;
        if(timer>=suspensionTime)inputAllowed = true;

        if(Input.GetKeyDown(KeyCode.F1))
        {
            ResetToRandom();
        }
        else if(Input.GetKeyDown(KeyCode.F2))
        {
            ResetThisLevel(currentLevel);
        }
        else if(Input.GetKeyDown(KeyCode.F3))
        {
            currentLevel++;
            ResetThisLevel(currentLevel);
        }
    }

    public void ShowBalls()
    {
        ballsText.text = "Balls :" + ballNumber.ToString();
    }
    public void OnBallDisabled()
    {
        if(ballNumber>0)
        {
            StartCoroutine("NewBallAfterFrame");
        }
        else
        {
            //LOST
            Debug.Log("LOST");
            Time.timeScale = 0;
            gamePanel.SetActive(false);
            losePanel.SetActive(true);
        }
    }
    IEnumerator NewBallAfterFrame()
    {
        yield return null;
        NewBall();
    }
    public void ShowTargetsAmaunt()
    {
        targetCaunterText.text = "Targets: " + targetNumber.ToString();
    }
    public void TargetHit()
    {
        targetNumber--;
        ShowTargetsAmaunt();
        if(targetNumber<=0)
        {
            //WIN
            Debug.Log("WIN level: " + currentLevel);
            PlayerPrefs.SetInt("lastLevel", currentLevel);
            Time.timeScale = 0;
            gamePanel.SetActive(false);
            winPanel.SetActive(true);

            
        }

    }
    public void NextLevelButtonPressed()
    {
        if(currentLevel%2==0)
        {
            AppodealHandle.Instance.ShowVideoForNextLevel();
        }
        else NextLevel();
    }
    
    public void NextLevel()
    {
        currentLevel++;
        Time.timeScale = 1;
        
        StartCoroutine("ResetFrameAfter");
    }
    public void CreateLevelsList(int numerOfLevels)
    {
        bool[,] tempObs = new bool[LevelConfig.xObs, LevelConfig.yObs], tempTar = new bool[LevelConfig.xTarg, LevelConfig.yTarg];
        ObsticleMovementType levelObsticleMovementType = ObsticleMovementType.None;
        for (int i = 0; i < numerOfLevels; i++)
        {
            allTargets.RandomizeTargets(Mathf.RoundToInt(i/10f)+1);
            tempTar = allTargets.targetPattern;
            allObsticles.RandomizeObsticles((int)Mathf.Floor(i/10f));
            tempObs = allObsticles.obsticlePattern;
            if(i<= 10) levelObsticleMovementType = ObsticleMovementType.None;
            else if(i>10) levelObsticleMovementType = ObsticleMovementType.OnlyY;
            
            else if(i>20) levelObsticleMovementType = ObsticleMovementType.OnlyZ;
            else if(i>30) levelObsticleMovementType = ObsticleMovementType.Ramdom;
            allLevels.Add(new LevelConfig(tempTar, tempObs, levelObsticleMovementType));
        }
    }
    public void NewGame()
    {
        currentLevel = 0;
        StartCoroutine(ResetFrameAfter());
        mainMenuPanel.SetActive(false);
    }
    public void Continue()
    {
        StartCoroutine("ResetFrameAfter");
        mainMenuPanel.SetActive(false);
    }
}
