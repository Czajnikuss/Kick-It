using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using EasyMobile;

public class FacebookHandler : MonoBehaviour
{
    public GameObject facebookLoginButton;


// Awake function from Unity's MonoBehavior
void Awake ()
{
    if (!FB.IsInitialized) {
        // Initialize the Facebook SDK
        FB.Init(InitCallback, OnHideUnity);
    } else {
        // Already initialized, signal an app activation App Event
        FB.ActivateApp();
    }
}

private void InitCallback ()
{
    if (FB.IsInitialized) {
        // Signal an app activation App Event
        FB.ActivateApp();
        // Reques login status (uer may be loged to facgook prevoiusly on this device)
        FB.Android.RetrieveLoginStatus(LoginStatusCallback);
        
        // ...
    } else {
        Debug.Log("Failed to Initialize the Facebook SDK");
    }
}

private void OnHideUnity (bool isGameShown)
{
    if (!isGameShown) {
        // Pause the game - we will need to hide
        Time.timeScale = 0;
    } else {
        // Resume the game - we're getting focus again
        Time.timeScale = 1;
    }
}


private void AuthCallback (ILoginResult result) {
    if (FB.IsLoggedIn) {
        // AccessToken class will have session details
        var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
        // Print current access token's User ID
        Debug.Log(aToken.UserId);
        // Print current access token's granted permissions
        foreach (string perm in aToken.Permissions) {
            Debug.Log(perm);
        }
    } else {
        Debug.Log("User cancelled login");
    }
}


private void LoginStatusCallback(ILoginStatusResult result) {
    if (!string.IsNullOrEmpty(result.Error)) {
        //Some error ocured during asking is user loged allready, so ask to login
        Debug.Log("Error: " + result.Error);
        facebookLoginButton.SetActive(true);
        
    } else if (result.Failed) {
        //Some error ocured during asking is user loged allready, so ask to login
       facebookLoginButton.SetActive(true);
        
        Debug.Log("Failure: Access Token could not be retrieved");
    } else {
        // Successfully logged user in
        // A popup notification will appear that says "Logged in as <User Name>"
        Debug.Log("Success: " + result.AccessToken.UserId);
    }
}
public void LoginToFacebook()
{
    var perms = new List<string>(){"public_profile", "email"};
    FB.LogInWithReadPermissions(perms, AuthCallback);
}

    
}
