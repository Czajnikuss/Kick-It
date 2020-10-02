using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class MuteUniversal : MonoBehaviour
{
    public Sprite muteSprite, soundSprite;
    bool isMute = false;
    public AudioListener audioListener;
    public Button thisObjectButton;
    

    private void Start() 
    {
        if(audioListener == null) audioListener = FindObjectOfType<AudioListener>(); 
        if(thisObjectButton == null) thisObjectButton = GetComponent<Button>();     
    }
    public void SoundTougle()
    {
        if(isMute)
        {
            //turn listener on
            audioListener.enabled = true;
            //show sound icon
            thisObjectButton.image.sprite = soundSprite;
            isMute = false;
        }
        else
        {
            //turn listener off
            audioListener.enabled = false;
            //show sound icon
            thisObjectButton.image.sprite = muteSprite;
            isMute = true;
        }
    }

}
