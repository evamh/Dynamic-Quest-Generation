using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
    Singleton class for Instruction UI 
    This is a TODO item 
**/

public class InstructionUIManager : MonoBehaviour
{
    public static InstructionUIManager current;
    [SerializeField] GameObject instructionUI;

    void Awake()
    {
        current = this;
        EventsManager.current.DisplayInstructionEvent += DisplayInstruction;
    }

    void OnDestroy()
    {
        EventsManager.current.DisplayInstructionEvent -= DisplayInstruction;
    }

    // Display instruction 
    public void DisplayInstruction(string instruction)
    {
        UIManager.current.ToggleUIElement(instructionUI);
        instructionUI.transform.Find("InstructionText").GetComponent<TextMeshProUGUI>().text = instruction;
    }
}
