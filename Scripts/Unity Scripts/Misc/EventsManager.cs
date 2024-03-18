using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Singleton class to manage all events in the game
**/

public class EventsManager : MonoBehaviour
{
    public static EventsManager current;

    // Actions
    public event Action<int> CoinChangeEvent;

    // TODO: remove quest specific actions
    public event Action<QuestObject> QuestChangeEvent;
    public event Action<QuestObject> QuestAddedEvent;

    public event Action<QuestSeriesObject> QuestSeriesAddedEvent;
    public event Action<QuestSeriesObject> QuestSeriesChangeEvent;
    public event Action<QuestSeriesObject> QuestSeriesCompletedEvent;

    public event Action<string> JournalEntryAddedEvent;

    // UI
    public event Action<string> DisplayInstructionEvent;
    public event Action<QuestSeriesObject> DisplaySeriesEvent;

    // For NPC
    public event Action<List<CharacterSpeech>> CommunicationEvent;

    // GPT
    public event Action<string> GPTResponseEvent;

    // New Game
    public event Action NewGameEvent;

    void Awake()
    {
        current = this;
    }

    // Coin change event
    public void InvokeCoinChangeEvent(int amount)
    {
        CoinChangeEvent?.Invoke(amount);
    }

    // Quest change event
    public void InvokeQuestChangeEvent(QuestObject quest)
    {
        QuestChangeEvent?.Invoke(quest);
    }

    // Quest added event
    public void InvokeQuestAddedEvent(QuestObject quest)
    {
        QuestAddedEvent?.Invoke(quest);
    }

    // Quest series added event 
    public void InvokeQuestSeriesAddedEvent(QuestSeriesObject series)
    {
        QuestSeriesAddedEvent?.Invoke(series);
    }

    // Quest series change event
    public void InvokeQuestSeriesChangeEvent(QuestSeriesObject series)
    {
        QuestSeriesChangeEvent?.Invoke(series);
    }

    // Quest series completed event
    public void InvokeQuestSeriesCompletedEvent(QuestSeriesObject series)
    {
        QuestSeriesCompletedEvent?.Invoke(series);
    }

    // Journal entry added event
    public void InvokeJournalEntryAddedEvent(string entry)
    {
        JournalEntryAddedEvent?.Invoke(entry);
    }

    // NPC communication event
    public void InvokeCommunicationEvent(List<CharacterSpeech> dialogue)
    {
        CommunicationEvent?.Invoke(dialogue);
    }

    // Instruction display event
    public void InvokeDisplayInstructionEvent(string instruction)
    {
        DisplayInstructionEvent?.Invoke(instruction);
    }

    // GPT response event
    public void InvokeGPTResponseEvent(string json)
    {
        GPTResponseEvent?.Invoke(json);
    }

    // New game event
    public void InvokeNewGameEvent()
    {
        NewGameEvent?.Invoke();
    }

    // Display series event 
    public void InvokeDisplaySeriesEvent(QuestSeriesObject series)
    {
        DisplaySeriesEvent?.Invoke(series);
    }
}
