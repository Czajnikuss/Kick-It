using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPanelHandler : MonoBehaviour
{
    ShopInGameManager shopInGameManager;

    public GameObject selectionIndicator, buyIndicator;
    private void Start() {
        if(itemOnDisplay.equiped) selectionIndicator.SetActive(true); 
        else selectionIndicator.SetActive(false);
        shopInGameManager= GameObject.FindObjectOfType<ShopInGameManager>();
    }
    public ItemToBuy itemOnDisplay;

    public void BuyItemOnDisplay()
    {
        if(!itemOnDisplay.inInventory)
        {
            shopInGameManager.ShowShopDialogPanel(itemOnDisplay);
        }
        else EquipThis();
    }
    public void EquipThis()
    {
        this.transform.parent.gameObject.BroadcastMessage("Unequip");
        itemOnDisplay.equiped=true;
        selectionIndicator.SetActive(true);
        if(itemOnDisplay.thisItemType == TypeOfItemToBuy.Ball)
        {
            shopInGameManager.playManager.ballPrefab = itemOnDisplay.gameObject;
            shopInGameManager.playManager.ChangeBall();
        }
        else if(itemOnDisplay.thisItemType == TypeOfItemToBuy.Obsticle)
        {
            shopInGameManager.playManager.allObsticles.obsticleToChangeTo = itemOnDisplay.gameObject;
            shopInGameManager.playManager.allObsticles.ChangeObsticles();
        }
        else if(itemOnDisplay.thisItemType == TypeOfItemToBuy.Taget)
        {
            shopInGameManager.playManager.allTargets.targetToChangeTo = itemOnDisplay.gameObject;
            shopInGameManager.playManager.allTargets.ChangeTargets();
        }
        else if(itemOnDisplay.thisItemType == TypeOfItemToBuy.Envirement)
        {
            EnvirementSettingHolder tempEnv = itemOnDisplay.GetComponent<EnvirementSettingHolder>();
            shopInGameManager.playManager.SetEnvirement(tempEnv);
        }
    }
    public void Unequip()
    {
        selectionIndicator.SetActive(false);
        itemOnDisplay.equiped = false;
    }
}
