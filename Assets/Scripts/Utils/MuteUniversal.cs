using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class MuteUniversal : MonoBehaviour
{
    public Sprite muteSprite, soundSprite;
    bool isMute = false;
    
    public Button thisObjectButton;
    

    private void Start() 
    { 
        if(thisObjectButton == null) thisObjectButton = GetComponent<Button>();     
    }
    public void SoundTougle()
    {
        if(isMute)
        {
            //turn listener on
            AudioListener.pause =false;
            //show sound icon
            thisObjectButton.image.sprite = soundSprite;
            isMute = false;
        }
        else
        {
            //turn listener off
            AudioListener.pause = true;
            //show sound icon
            thisObjectButton.image.sprite = muteSprite;
            isMute = true;
        }
    }

}
