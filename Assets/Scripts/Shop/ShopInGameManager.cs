using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

public class ShopInGameManager : MonoBehaviour
{
    public ItemToBuy[] allItemsToBuy;
    ItemToBuy itemAboutToBeBought;
    public List<string> availableItemsNames;
    public PlayManager playManager;
    public GameObject displayInStoreButtonPrefab;
    public GameObject displayBallsPanel, displayObsticlesPanel, displayTargetPanel, displayEnvirementPanel;

    public GameObject shopDialogPanel, listingPanel, confirmationPanel;
    public RawImage listingImage;
    public VideoPlayer listingPlayer;
    public TextMeshProUGUI listingInfoText, priceCoinsText, priceInMoney, coinsAmountText, thankYouText;
    public Button buyforCoinButton, buyForCashButton;
    void Start()
    {
        playManager = GetComponent<PlayManager>();
        shopDialogPanel.SetActive(false);
        DisplayBuyButtons();
        
    }

    private void DisplayBuyButtons()
    {    
        float displayHight = displayInStoreButtonPrefab.GetComponent<RectTransform>().sizeDelta.y;
        foreach (var item in allItemsToBuy)
        {   
            GameObject tempDisplayPanel = displayBallsPanel;
            if(item.thisItemType == TypeOfItemToBuy.Ball)tempDisplayPanel = displayBallsPanel;
            else if(item.thisItemType == TypeOfItemToBuy.Envirement)tempDisplayPanel = displayEnvirementPanel;
            else if(item.thisItemType == TypeOfItemToBuy.Obsticle)tempDisplayPanel = displayObsticlesPanel;
            else if(item.thisItemType == TypeOfItemToBuy.Taget)tempDisplayPanel = displayTargetPanel;
            
            
            GameObject tempDisplay = Instantiate(displayInStoreButtonPrefab, tempDisplayPanel.transform,false );
            VideoPlayer videoPlayerInDisplay = tempDisplay.GetComponent<VideoPlayer>();
            videoPlayerInDisplay.clip = item.itemClip;
            videoPlayerInDisplay.targetTexture = item.itemRendererTexture;
            tempDisplay.GetComponent<RawImage>().texture = item.itemRendererTexture;
            tempDisplay.transform.Find("BuyIndicator").gameObject.SetActive(!item.inInventory);
            tempDisplay.GetComponent<DisplayPanelHandler>().itemOnDisplay = item;
        }
        //set width of displays
        RectTransform rt = displayBallsPanel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, (displayBallsPanel.transform.childCount * displayHight) + 10f);
        rt = displayObsticlesPanel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, (displayObsticlesPanel.transform.childCount * displayHight) + 10f);
        rt = displayTargetPanel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, (displayTargetPanel.transform.childCount * displayHight) + 10f);
        rt = displayEnvirementPanel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, (displayEnvirementPanel.transform.childCount * displayHight) + 10f);
        
    }
    public void ShowShopDialogPanel(ItemToBuy itemToBuy)
    {
        shopDialogPanel.SetActive(true);
        listingPanel.SetActive(true);
        listingPlayer.clip = itemToBuy.itemClip;
        listingPlayer.targetTexture = itemToBuy.itemRendererTexture;
        listingImage.texture = itemToBuy.itemRendererTexture;
        listingInfoText.text = itemToBuy.infoToDisplay;
        priceCoinsText.text = itemToBuy.priceCoins.ToString();
        coinsAmountText.text = playManager.coinsAmount.ToString();
        itemAboutToBeBought = itemToBuy;

        if(playManager.coinsAmount>= itemToBuy.priceCoins) buyforCoinButton.interactable =true;
        else buyforCoinButton.interactable = false;
        //handle external shop dialog and praices.
        

    }
    public void SaveAvaliableItemsList()
    {
        PlayerPrefs.SetInt("availableItemsCount", availableItemsNames.Count);
        for (int i = 0; i < availableItemsNames.Count; i++)
        {
            PlayerPrefs.SetString("itemName" + i, availableItemsNames[i]);
        }
    }
    public void LoadAvailableItemsList()
    {
        if(PlayerPrefs.GetInt("availableItemsCount",0) > 0)
        {
            Debug.Log("we got some items");
            availableItemsNames = new List<string>();
            for (int i = 0; i < PlayerPrefs.GetInt("availableItemsCount",0); i++)
            {
                availableItemsNames.Add(PlayerPrefs.GetString("itemName" + i));
            }

            SetItemsAvailabilityAcordingToList();
        }
        else Debug.Log("no items in player prefs");
    }
    public void SetItemsAvailabilityAcordingToList()
    {
        foreach (var item in availableItemsNames)
        {
            for (int i = 0; i < allItemsToBuy.Length; i++)
            {
                if(allItemsToBuy[i].gameObject.name == item)
                {
                    allItemsToBuy[i].inInventory = true;
                    continue;
                }
            }
        }
    }
    public void BuySuccesfull()
    {
        for (int i = 0; i < allItemsToBuy.Length; i++)
        {
            if(allItemsToBuy[i]==itemAboutToBeBought)
            {
                allItemsToBuy[i].inInventory = true;
                availableItemsNames.Add(allItemsToBuy[i].gameObject.name);
                SaveAvaliableItemsList();
                playManager.coinsAmount -= itemAboutToBeBought.priceCoins;
                ShowConfirmationPanel();
                continue;
            }
        }
    }
    public void ShowConfirmationPanel()
    {
        confirmationPanel.SetActive(true);
        DisplayPanelHandler[] allDisplays = GameObject.FindObjectsOfType<DisplayPanelHandler>();
        foreach (var item in allDisplays)
        {   
            if(item.itemOnDisplay == itemAboutToBeBought)
            {
                item.buyIndicator.SetActive(false);
                continue;
            }
        }     

        thankYouText.text = "Thank You for buying " + itemAboutToBeBought.gameObject.name + " !\n It is allready avaialble for equiping\n Have fun with it! \n \n You got still "+ playManager.coinsAmount.ToString() + " Coins left!";
    }
    
}
