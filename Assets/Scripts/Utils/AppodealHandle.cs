using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using UnityEngine.UI;
using System;


public class AppodealHandle : MonoBehaviour, IRewardedVideoAdListener
{
    [Header("Reward Dispatch Variables")]
    public PlayManager playManager;   
    public Stores store = Stores.GooglePlay;
    public bool isRewardedVideoLoaded
    { 
        private set; 
        get;
    }
    public enum Stores
    {
        GooglePlay,
        Amazon
    }

    string thisStore;
    const string GooglePlayAppKey ="5357145b6d2bf5264b19d4ef1ee27efbae1d9ca308eea3a2";
    const string AmazonAppKey = "42462e889282c8dd2a98bdf883d82461f3dce2a7c4a234a5";

    #region SINGLETON PATTERN
 public static AppodealHandle _instance;
 public static AppodealHandle Instance
 {
     get {
         if (_instance == null)
         {
             _instance = GameObject.FindObjectOfType<AppodealHandle>();
             
             if (_instance == null)
             {
                 GameObject container = new GameObject("AppodealHandle");
                 _instance = container.AddComponent<AppodealHandle>();
             }
         }
     
         return _instance;
     }
 }
 #endregion

    [Header("Conscent elements")]
    
    public GameObject conscentWindow;
    public GameObject mainPanel;
    public bool consentValue {get; private set;}
    public Toggle conscenToggle;

    
    [Header("Ads Problems")]
    public GameObject notShowingTryAgainPanel;
    
    void Start()
    {
        isRewardedVideoLoaded = false;
        if(store == Stores.Amazon) thisStore = AmazonAppKey; 
        else thisStore = GooglePlayAppKey;
        
        if(PlayerPrefs.GetInt("result_gdpr",0) != 0)
        {
            consentValue = PlayerPrefs.GetInt("result_gdpr_sdk") > 0 ? true : false;
            Debug.Log(consentValue);
            Appodeal.initialize(thisStore, Appodeal.REWARDED_VIDEO, consentValue);
        }
        else
        {
            Appodeal.initialize(thisStore, Appodeal.REWARDED_VIDEO, false);
            ShowConscentWindow();
        }
        Appodeal.setRewardedVideoCallbacks(this);
        Appodeal.setLogLevel(Appodeal.LogLevel.Verbose);
        
//Testing Mode        
        Appodeal.setTesting(false);

    }
    private void  ShowConscentWindow()
    {
        conscentWindow.SetActive(true);
        mainPanel.SetActive(false);
    }
    public void YesPressed()
    {
        PlayerPrefs.SetInt("result_gdpr", 1);
        PlayerPrefs.SetInt("result_gdpr_sdk", 1);
        consentValue = true;
        
        Appodeal.updateConsent(consentValue);
        
        mainPanel.SetActive(true);
        conscentWindow.SetActive(false);
    }
    public void NoPressed()
    {
        PlayerPrefs.SetInt("result_gdpr", 1);
        PlayerPrefs.SetInt("result_gdpr_sdk", 0);
        consentValue = false;
        
        Appodeal.updateConsent(consentValue);
        
        mainPanel.SetActive(true);
        conscentWindow.SetActive(false);
    }
     /* "personalizes your advertising experience using Appodeal. " +
        Appodeal and its partners may collect and process personal data such as device identifiers, 
        location data, and other demographic and interest data to provide advertising experience tailored to you. 
        By consenting to this improved ad experience, you'll see ads that Appodeal and 
        its partners believe are more relevant to you. Learn more.
        By agreeing, you confirm that you are over the age of 16 and would like a personalized ad experience.
    
    
    
    
Appodeal and its partners may collect and process personal data such as device identifiers, location data, and other demographic and interest data to provide advertising experience tailored to you. 
By consenting to this improved ad experience, you'll see ads that Appodeal and its partners believe are more relevant to you.
By agreeing, you confirm that you are over the age of 16 and would like a personalized ad experience.
    
    
    
    
    */
    public void PrivacyAppoLinkOpen()
    {
        Application.OpenURL("https://www.appodeal.com/privacy-policy");
    }

    #region Rewarded Video callback handlers
    public void onRewardedVideoLoaded(bool isPrecache) //Called when rewarded video was loaded (precache flag shows if the loaded ad is precache).
    { 
        print("Video loaded"); 
        isRewardedVideoLoaded = isPrecache;
    }
    public void onRewardedVideoFailedToLoad() { print("Video failed"); } // Called when rewarded video failed to load
    public void onRewardedVideoShowFailed() // Called when rewarded video was loaded, but cannot be shown (internal network errors, placement settings, or incorrect creative)
    {
         print ("Video show failed");
         notShowingTryAgainPanel.SetActive(true); 
    } 
    public void onRewardedVideoShown() { print("Video shown"); } // Called when rewarded video is shown
    public void onRewardedVideoClicked() { print("Video clicked"); } // Called when reward video is clicked
    public void onRewardedVideoClosed(bool finished) // Called when rewarded video is closed
    { 
        print("Video closed"); 
    } 
    public void onRewardedVideoFinished(double amount, string name) // Called when rewarded video is viewed until the end
    { 
        //Debug.Log("Reward: " + amount + " " + name);
        if(name == "NextLevel")
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    GiveNextLevel();
                }
            );
        }
        else if(name == "Restart")
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    RestartLevel();
                }
            );
        }
        else if(name == "Coins")
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    AddCoinsAfterAdd((int)amount);
                }
            );
        }

    } 
    public void onRewardedVideoExpired() { print("Video expired"); } //Called when rewarded video is expired and can not be shown
    
    
    #endregion

    public void ShowVideoForNextLevel()
    {
        if(Appodeal.canShow(Appodeal.REWARDED_VIDEO))
        {
            Appodeal.show(Appodeal.REWARDED_VIDEO, "NextLevelButtonPressed");
        }
        else GiveNextLevel();
    }
    public void GiveNextLevel()
    {
        playManager.NextLevel();
    }
    public void ShowVideoForRestart()
    {
        if(Appodeal.canShow(Appodeal.REWARDED_VIDEO))
        {
            Appodeal.show(Appodeal.REWARDED_VIDEO, "RestartLevel");
        }
        else RestartLevel();
    }
    public void RestartLevel()
    {
        playManager.ResetThisLevel();
    }
    public void ShowVideoForCoins()
    {
        if(Appodeal.canShow(Appodeal.REWARDED_VIDEO))
        {
            Appodeal.show(Appodeal.REWARDED_VIDEO, "AddCoins");
        }
        
    }
    public void AddCoinsAfterAdd(int amount)
    {
        playManager.AddCoins(amount);
    }
   
   
}
