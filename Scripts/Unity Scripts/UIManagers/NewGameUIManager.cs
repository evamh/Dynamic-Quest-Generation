using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Manager for the New Game button 
**/

public class NewGameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject newGameButton;

    public void NewGameButtonClick()
    {
        EventsManager.current.InvokeNewGameEvent();
    } 
}
