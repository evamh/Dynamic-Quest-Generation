using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/**
    Class responsible for speech/UI communication, such as updating the UI accordingly when conversing with a NPC 
**/

public class CommunicationUIManager : MonoBehaviour
{
    [SerializeField] GameObject speechUI;
    private bool showNextPrompt;
    private bool waitingForClose;

    void Awake()
    {
        EventsManager.current.CommunicationEvent += StartConversation;

        showNextPrompt = true;
        waitingForClose = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(Constants.Conversation_Keys.CLOSE_SPEECH) && waitingForClose)
        {
            UIManager.current.ToggleUIElement(speechUI);
            waitingForClose = false;
        }
    }

    // Populate speech in the UI 
    private void PopulateSpeech(CharacterSpeech entry)
    {
        string speechText = entry.Tag + ":\n \"<i>" 
                            + entry.Speech +"\"</i>";

        speechUI.transform.Find("SpeechText").GetComponent<TextMeshProUGUI>().text = speechText;
    }

    // Start conversation and toggle UI element 
    public void StartConversation(List<CharacterSpeech> dialogue)
    {
        UIManager.current.ToggleUIElement(speechUI);

        EventsManager.current.InvokeDisplayInstructionEvent(Constants.Instructions.CONTINUE_SPEECH);;

        StartCoroutine(Conversation(dialogue));
    }

    // Coroutine for conversation between player and NPC 
    IEnumerator Conversation(List<CharacterSpeech> dialogue)
    {
        int counter = 0;
        foreach(CharacterSpeech speech in dialogue)
        {
            PopulateSpeech(speech);
            showNextPrompt = false;

            while(!showNextPrompt && counter < dialogue.Count - 1)
            {
                if(Input.GetKeyDown(Constants.Conversation_Keys.CONTINUE_SPEECH)) {
                    showNextPrompt = true;
                    counter++;
                }

                yield return null;
            }
        }

        waitingForClose = true;

    }

    void OnDestroy()
    {
        EventsManager.current.CommunicationEvent -= StartConversation;
    }
}
