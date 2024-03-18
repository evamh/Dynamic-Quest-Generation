using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

/**
    Manager for general UI 
**/

public class UIManager : MonoBehaviour, IUpdateUI
{
    public static UIManager current;
    [SerializeField] TextMeshProUGUI coinText; 

    public bool IsActiveUIElement {get; set;}
    private GameObject activeUIElement {get; set;}

    void Awake()
    {
        current = this;
        EventsManager.current.CoinChangeEvent += UpdateCoinText;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateCoinText(CoinManager.current.CurrentCoins);

        IsActiveUIElement = false;
        activeUIElement = null;
    }
    
    void OnDestroy()
    {
        EventsManager.current.CoinChangeEvent -= UpdateCoinText;
    }

    // Update coin text accordingly 
    void UpdateCoinText(int currentCoins)
    {
        coinText.text = "Coins: " + currentCoins.ToString();
    }

    // Toggle the given UI element. Make it active if it's inactive, and vice versa. 
    // Make the current active element inactive if it's different, otherwise UI elements will display on top of one another 
    public void ToggleUIElement(GameObject uiElement)
    {
        if(!IsActiveUIElement)
        {
            uiElement.SetActive(true);
            IsActiveUIElement = true;
            activeUIElement = uiElement;
        } else if(IsActiveUIElement && activeUIElement == uiElement) {
            uiElement.SetActive(false);
            IsActiveUIElement = false;
            activeUIElement = null;
        }
    }

    // Update UI 
    public void UpdateDataUI()
    {
        UpdateCoinText(CoinManager.current.CurrentCoins);
    }

    // Reset UI 
    public void ResetUI()
    {
        UpdateCoinText(CoinManager.current.startCoins); // TODO - might need to change this 
    }
}
