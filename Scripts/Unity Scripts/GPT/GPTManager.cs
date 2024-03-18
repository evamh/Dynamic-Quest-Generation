using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using OpenAI.Chat;
using Unity.VisualScripting;
using System.IO;
using Newtonsoft.Json;
using System;
using UnityEditor;

/**
    The singleton class for the GPT manager. This manages the set up and API calls to the GPT model 

    This class uses the package OpenAI by RageAgainstThePixel for the GPT related logic (setting up the model, making calls, etc)
    The code was modified from the one provided in the Udemy course "ChatGPT x Unity: The Ultimate Integration Guide" by Tabsil Makes Games.        
**/

public class GPTManager : MonoBehaviour, IDataSave
{
    public static GPTManager current;

    [SerializeField] private string pathToKeys;
    [SerializeField] private string modelName = "ft:gpt-3.5-turbo-1106:eva::8Nj3TB0j";
    [SerializeField] bool makeCalls = true;

    //private OpenAI.OpenAIAuthentication apiInfo;
    private string apiKey;
    private string organisationID;
    private string defaultModel = OpenAI.Models.Model.GPT3_5_Turbo;

    private OpenAIClient api;
    private List<Message> chatPrompts;

    void Awake()
    {
        current = this;

        EventsManager.current.JournalEntryAddedEvent += MakeGPTCall;
    }

    void Start()
    {
        PopulateKeys();
        Authenticate();
    }

    // Read in file from defined path and set up key/IDs as needed 
    private void PopulateKeys()
    {
        var apiInfo = OpenAIAuthentication.Default.LoadFromPath(pathToKeys);
        apiKey = apiInfo.Info.ApiKey;
        organisationID = apiInfo.Info.OrganizationId;
    }

    // Authenticate the keys and create API client 
    private void Authenticate()
    {
        OpenAIAuthentication authentication = new OpenAIAuthentication(apiKey, organisationID);
        api = new OpenAIClient(authentication); 
    }

    // Given a role and string text, create a message object and store it in the chatPrompts list 
    private void CreateAndStoreMessage(Role role, string text)
    {
        Message newMessage = new Message(role, text);
        chatPrompts.Add(newMessage);
    }

    // Retrieve the system instructions and append the world description
    // Create the System role with these instructions and store accordingly
    private void InitialiseModel()
    {
        DescriptionGenerator worldDescriptionGenerator = GetComponent<DescriptionGenerator>();
        string initScript = Resources.Load<TextAsset>("GPT_Initialisation_Script").ToString();
        initScript += worldDescriptionGenerator.GetWorldDescription();

        CreateAndStoreMessage(Role.System, initScript);
    }

    // From RageAgainstThePixel OpenAI package, used for fine-tuning testing 
    private async void ListFineTuneJobs()
    {
        var list = await api.FineTuningEndpoint.ListFineTuneJobsAsync();
        foreach(var job in list)
        {
            Debug.Log("job: " + job);
        }
    }

    // Make and receive the GPT call
    private async void GPTCall()
    {      
        Debug.Log("model name is: " + modelName);
        
        // Create request
        ChatRequest request = new ChatRequest(
                    messages: chatPrompts,
                    model: modelName,
                    temperature: 1
        );
 
        // Send request and get result
        try
        {

            Debug.Log("request: " + request.ToString());
            Debug.Log("number of prompts: " + chatPrompts.Count);

            // send request
            var result = await api.ChatEndpoint.GetCompletionAsync(request);

            // store result 
            CreateAndStoreMessage(Role.Assistant, result.FirstChoice.ToString());

            Debug.Log("RESULT: \n" + result.FirstChoice.ToString());

            EventsManager.current.InvokeGPTResponseEvent(result.FirstChoice.ToString());
        }
        catch(Exception e)
        {   
            string message = "[GPTManager.GPTCall] Exception thrown when making call to ChatGPT: \n";
            message += e;
            Debug.Log(message);
        }
    }

    // Wrapper function that checks the bool makeCalls before calling GPT 
    // Helps with testing 
    public void MakeGPTCall(string newPrompt)
    {
        if(makeCalls)
        {
            CreateAndStoreMessage(Role.User, newPrompt);
            GPTCall();
        }
    }

    void OnDestroy()
    {
        EventsManager.current.JournalEntryAddedEvent -= MakeGPTCall;
    }

    // Persisting data
    public void LoadData(GameData gameData)
    {
        chatPrompts = gameData.GPTMessages;
        if(chatPrompts.Count == 0) InitialiseModel();
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.GPTMessages = chatPrompts;
    }

}
