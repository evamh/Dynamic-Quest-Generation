using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Class to encapsulate the logic for one side of dialogue
**/

public class CharacterSpeech
{
    public string Tag;
    public string Speech;
    
    public CharacterSpeech(string tag, string speech)
    {
        Tag = tag;
        Speech = speech;
    }

    public override string ToString()
    {
        string result = "Character: " + Tag;
        result += ", Speech: " + Speech;
        return result;
    }


}
