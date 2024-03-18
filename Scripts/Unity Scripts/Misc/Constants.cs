using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Constants class to store all constants used in the code
**/

public class Constants
{
    public class Quest_Types
    {
        public const string COLLECT_COINS = "COLLECT_COINS";
        public const string TALK_NPC = "TALK_NPC";
        public const string GOTO_LOCATION = "LOCATION_GOTO";
        public const string FIND_OBJECT = "FIND_OBJECT";
        public const string UNDEFINED = "UNDEFINED";
    }

    public class JSON_Fields
    {
        // ALL
        public const string QUEST_TYPE = "QUEST_TYPE";
        public const string QUEST_TITLE = "QUEST_TITLE";
        public const string QUEST_DESC = "QUEST_DESC";

        // REWARDS
        public const string COIN_REWARD = "COIN_REWARD";

        // COLLECT COINS
        public const string NUM_COINS = "NUM_COINS";

        // NPC
        public const string NPC_NAME = "NPC_NAME";
        public const string NPC_RESPONSE = "NPC_RESPONSE";
        public const string PLAYER_PROMPT = "PLAYER_PROMPT";
        public const string DIALOGUE = "DIALOGUE";

        // FIND OBJECT
        public const string OBJECT_TYPE = "OBJECT";
        public const string LOCATION = "LOCATION";

        // QUEST SERIES
        public const string SERIES_TITLE = "SERIES_TITLE";
        public const string SERIES_DESC = "SERIES_DESC";
        public const string SERIES_STEPS = "SERIES_STEPS";
    }

    public class Character_Tags
    {
        public const string FOX_TAG = "FOX";
        public const string CHIP_TAG = "CHIP";
        public const string IVY_TAG = "IVY";
        public const string BEAN_TAG = "BEAN";
        public const string RANDOMLY_GENERATED = "RANDOMLY_GENERATED";

        public static readonly string[] CHARACTERS = { "FOX", "CHIP", "IVY", "BEAN"};
    }

    public class Instructions
    {
        public const string CONTINUE_SPEECH = "Press spacebar to continue";
        public const string CLOSE_SPEECH = "Press spacebar to close";
        public const string START_TALKING = "Press 't' to start talking";
    }

    public class Conversation_Keys
    {
        public const KeyCode CONTINUE_SPEECH = KeyCode.Space;
        public const KeyCode CLOSE_SPEECH = KeyCode.Space;
    }

    public class Description
    {
        public const string INVENTORY_ITEMS = "OBJECTS";
        public const string NPC = "NPC_NAMES";
        public const string LOCATIONS = "LOCATIONS";
        public const string NPC_GAMEOBJECT_TAG = "NPCs";
    }

    public class Locations
    {
        public const string BEACH = "BEACH";
        public const string FOREST = "FOREST";
        public const string VILLAGE = "VILLAGE";
        public const string DEFAULT = "VILLAGE";
    }

    public class Props
    {
        public const string PROPS = "PROPS";
    }

    public class Items
    {
        public const string COIN = "COIN";
    }
}
