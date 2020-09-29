using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public enum TypeOfItemToBuy
{
    Ball,
    Obsticle,
    Taget,
    Envirement
}


public class ItemToBuy : MonoBehaviour
{
    public bool inInventory, equiped;
    
    public TypeOfItemToBuy thisItemType;
    VideoPlayer videoPlayer;
    public VideoClip itemClip;
    public RenderTexture itemRendererTexture;

    public string infoToDisplay;
    public int priceCoins;
    
    public string nameInOnLineStore;


}
