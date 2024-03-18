using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using UnityEngine;

/**
    Singleton class to manage dummy quests
    This script is used for testing only. When 'WriteDummyQuests' is true, it'll add any dummyJSONs to the list of quest series
**/

public class DummyQuestManager : MonoBehaviour
{
    public static DummyQuestManager current;
    public bool WriteDummyQuests = true;

    void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(WriteDummyQuests)
        {

            string dummyJSON2 = DummyCoinJSON();
            //QuestManager.current.CreateQuestFromJSON(dummyJSON2);

            string dummyJson4 = Dummy3JSON();
            //QuestObject q4 = CreateQuestFromJSON(dummyJson4);
            //QuestManager.current.AddQuest(q4);

            string dummyJson5 = DummyObjectJSON();
            //QuestManager.current.CreateQuestFromJSON(dummyJson5);

            string dummyJson6 = DummyDialogueJSON();
            //QuestManager.current.CreateQuestFromJSON(dummyJson6);

            string dummyJson7 = DummySeriesJSON();
            //QuestSeriesManager.current.CreateQuestSeriesFromJSON(dummyJson7);

            string dummyJson8 = DummySeries2JSON();
            //QuestSeriesManager.current.CreateQuestSeriesFromJSON(dummyJson8);

            string dummyCoinJson = DummyCoinSeriesJSON();
            //QuestSeriesManager.current.CreateQuestSeriesFromJSON(dummyCoinJson);

            string dummyObjJSON = DummyObjectSeriesJSON();
            //QuestSeriesManager.current.CreateQuestSeriesFromJSON(dummyObjJSON);

            string dummyRandomNPCJSON = DummyRandomNPCSeriesJSON();
            //QuestSeriesManager.current.CreateQuestSeriesFromJSON(dummyRandomNPCJSON);

            string dummyUndefinedObjJSON = DummyUndefinedObjectSeriesJSON();
            QuestSeriesManager.current.CreateQuestSeriesFromJSON(dummyUndefinedObjJSON);
        }
       
    }

    // The following functions return dummy JSON objects for testing
    private string DummyNPCJSON()
    {
        string str = 
        @"{
            ""QUEST_TYPE"": ""TALK_NPC"",
            ""QUEST_TITLE"": ""Talk to Baker Chip"",
            ""QUEST_DESC"": ""Baker Chip is known for his delicious baked goods. Speak to him for culinary tips and a secret recipe."",
            ""COIN_REWARD"": 5,
            ""PLAYER_PROMPT"": ""Hi, Chip! I've heard your baking is amazing. Do you have any tips or a special recipe to share?"",
            ""NPC_NAME"": ""CHIP"",
            ""NPC_RESPONSE"": ""Ah, baking is an art! I'll share with you my secret doughnut recipe.""
        }";

        return str;
    }

    private string DummyCoinJSON()
    {

        string str = 
        @"{
            ""QUEST_TYPE"": ""COLLECT_COINS"",
            ""QUEST_TITLE"": ""Collect 2 Coins for Charity Drive"",
            ""QUEST_DESC"": ""Contribute to the town's charity drive by collecting 2 coins from various sources."",
            ""COIN_REWARD"": 10,
            ""NUM_COINS"": 2
        }";
        return str;
    }

    private string Dummy2JSON()
    {
        string str = 
        @"{
            ""QUEST_TYPE"": ""TALK_NPC"",
            ""QUEST_TITLE"": ""Talk to the Wise Old Writer to seek advice on starting a career as a fiction writer"",
            ""QUEST_DESC"": ""The Wise Old Writer, known for his wisdom and experience, may have some valuable advice and guidance for you on beginning your career as a fiction writer."",
            ""NPC_NAME"": ""Wise Old Writer"",
            ""PLAYER_PROMPT"": ""Hello Wise Old Writer, I would like to seek your advice on starting a career as a fiction writer. Can you share any tips or guidance?"",
            ""NPC_RESPONSE"": ""Ah, another aspiring writer! My advice for you is to start by immersing yourself in the genre you wish to write in. Read extensively, study the techniques of successful authors, and practice your writing every day. Remember, writing is not just about talent, but also dedication and perseverance. Good luck on your journey!""
        }";

        return str;
    }

    private string Dummy3JSON()
    {
        string str =
        @"{
            ""QUEST_TYPE"": ""COLLECT_COINS"",
            ""QUEST_TITLE"": ""Collect Coins for a Cup of Coffee"",
            ""QUEST_DESC"": ""You need some coins to buy a cup of coffee. Explore the forest and collect enough coins to satisfy your caffeine cravings."",
            ""NUM_COINS"": 5
        }";

        return str;
    }

     private string DummyObjectJSON()
    {
        string str =
        @"{
            ""QUEST_TYPE"": ""FIND_OBJECT"",
            ""QUEST_TITLE"": ""Find the Hidden Ancient Scroll (Cube_A)"",
            ""QUEST_DESC"": ""Legend speaks of an ancient scroll hidden deep in the forest. Your task is to locate the elusive Cube_A, which holds the scroll."",
            ""OBJECT_TYPE"": ""Cube_A""
        }";

        return str;
    }

    private string DummyDialogueJSON()
    {
        string str = 
        @"{
            ""QUEST_TYPE"": ""TALK_NPC"",
            ""QUEST_TITLE"": ""Enjoy Coffee with NPC_A"",
            ""QUEST_DESC"": ""Express your desire to have a cup of coffee with someone and engage in a pleasant conversation with NPC_CoffeeLover who shares your passion for coffee."",
            ""NPC_NAME"": ""NPC_A"",
            ""DIALOGUE"": [
                { ""Tag"": ""Fox"", ""Speech"": ""Hello, NPC_A. I would love to get a cup of coffee with someone. Would you like to join me for a coffee and a chat?""},
                { ""Tag"": ""NPC_A"", ""Speech"": ""Absolutely! I can't resist a good cup of coffee and a friendly conversation. Let's head to the local café.""},
                { ""Tag"": ""Fox"", ""Speech"": ""That sounds great. I've heard the café around here serves excellent coffee.""},
                { ""Tag"": ""NPC_A"", ""Speech"": ""Indeed, they do. We can share our love for coffee and maybe even exchange some coffee-related stories.""},
                { ""Tag"": ""Fox"", ""Speech"": ""I'm looking forward to it. Coffee and good company make for a perfect combination.""},
                { ""Tag"": ""NPC_A"", ""Speech"": ""I couldn't agree more. Let's enjoy our coffee and a delightful conversation.""},
                { ""Tag"": ""Fox"", ""Speech"": ""Thank you, NPC_A. This coffee break will be a highlight of my day.""}
            ]
        }";

        return str;
    }

    private string DummySeriesJSON()
    {
        string str =
        @"{
            ""SERIES_TITLE"": ""The Baker's Dream"",
            ""SERIES_DESC"": ""Embark on a journey to establish the Baker's Dream Bakery. From finding the perfect location to creating mouthwatering pastries, follow your passion for baking in this delightful quest series."",
            ""SERIES_STEPS"": [
            {
                ""QUEST_TYPE"": ""TALK_NPC"",
                ""QUEST_TITLE"": ""Consulting with Ivy"",
                ""QUEST_DESC"": ""Ivy offers her guidance on the ideal bakery location."",
                ""NPC_NAME"": ""IVY"",
                ""DIALOGUE"": [
                { ""Tag"": ""FOX"", ""Speech"": ""Hello, Ivy. I'm planning to open a bakery in town, and I'd appreciate your advice on the location."" },
                { ""Tag"": ""IVY"", ""Speech"": ""Oh, that's exciting! We have a great spot near the park. It's perfect for a bakery.""}
                ]
            },
            {
                ""QUEST_TYPE"": ""FIND_OBJECT"",
                ""QUEST_TITLE"": ""Scouting Bakery Locations"",
                ""QUEST_DESC"": ""Explore potential locations near the park for your future bakery."",
                ""OBJECT_TYPE"": ""CUBE_A"",
                ""LOCATION"": ""VILLAGE""
            },
            {
                ""QUEST_TYPE"": ""COLLECT_COINS"",
                ""QUEST_TITLE"": ""Gathering Building Resources"",
                ""QUEST_DESC"": ""Collect the necessary resources to construct your bakery, including wood, bricks, and more."",
                ""NUM_COINS"": 5
            }
        ]
        }";
        return str;
    }

    private string DummyCoinSeriesJSON()
    {
                string str =
        @"{
            ""SERIES_TITLE"": ""The Baker's Dream"",
            ""SERIES_DESC"": ""Embark on a journey to establish the Baker's Dream Bakery. From finding the perfect location to creating mouthwatering pastries, follow your passion for baking in this delightful quest series."",
            ""SERIES_STEPS"": [
            {
                ""QUEST_TYPE"": ""COLLECT_COINS"",
                ""QUEST_TITLE"": ""Gathering Building Resources"",
                ""QUEST_DESC"": ""Collect the necessary resources to construct your bakery, including wood, bricks, and more."",
                ""NUM_COINS"": 5
            }
        ]
        }";
        return str;
    }

     private string DummyRandomNPCSeriesJSON()
    {
        string str =
        @"{
            ""SERIES_TITLE"": ""Make New Friends"",
            ""SERIES_DESC"": ""Meet someone new!"",
            ""SERIES_STEPS"": [
            {
                ""QUEST_TYPE"": ""TALK_NPC"",
                ""QUEST_TITLE"": ""Meet your new friend"",
                ""QUEST_DESC"": ""Have a chat with your new friend, Cleo."",
                ""NPC_NAME"": ""CLEO"",
                ""DIALOGUE"": [
                    { ""Tag"": ""FOX"", ""Speech"": ""Hey Cleo, it's nice to meet you!"" },
                    { ""Tag"": ""CLEO"", ""Speech"": ""Fox, it's nice to meet you too!"" }
            ]
            }
        ]
        }";
        return str;
    }

    private string DummyObjectSeriesJSON()
    {
        string str =
        @"{
            ""SERIES_TITLE"": ""Treasure Hunt"",
            ""SERIES_DESC"": ""Look for different objects in the world!"",
            ""SERIES_STEPS"": [
            {
                ""QUEST_TYPE"": ""FIND_OBJECT"",
                ""QUEST_TITLE"": ""Find an object"",
                ""QUEST_DESC"": ""Go on a scavenger hunt in the world!"",
                ""OBJECT"": ""CHERRY"",
                ""LOCATION"": ""FOREST""
            }
        ]
        }";
        return str;
    }

    private string DummyUndefinedObjectSeriesJSON()
    {
        string str =
        @"{
            ""SERIES_TITLE"": ""Treasure Hunt"",
            ""SERIES_DESC"": ""Look for different objects in the world!"",
            ""SERIES_STEPS"": [
            {
                ""QUEST_TYPE"": ""FIND_OBJECT"",
                ""QUEST_TITLE"": ""Find an object"",
                ""QUEST_DESC"": ""Go on a scavenger hunt in the world!"",
                ""OBJECT"": ""PEN"",
                ""LOCATION"": ""FOREST""
            }
        ]
        }";
        return str;
    }

    private string DummySeries2JSON()
    {
        string str =
        @"
        {
            ""SERIES_TITLE"": ""Bean's Coffee Quest"",
            ""SERIES_DESC"": ""Join Bean, the coffee aficionado, on a delightful coffee quest as FOX explores the art of brewing, uncovers coffee secrets, and enjoys the perfect cup of joe."",
            ""SERIES_STEPS"": [
            {
                ""QUEST_TYPE"": ""TALK_NPC"",
                ""QUEST_TITLE"": ""Chat with Bean for Coffee"",
                ""QUEST_DESC"": ""Begin your coffee journey with a chat with Bean to discover the secrets of a perfect morning brew."",
                ""NPC_NAME"": ""BEAN"",
                ""DIALOGUE"": [
                    { ""Tag"": ""FOX"", ""Speech"": ""Hey Bean, I'm in the mood for a cup of coffee. Any recommendations?"" },
                    { ""Tag"": ""BEAN"", ""Speech"": ""Ah, coffee lover! Let's start with the basics. The key to a great brew is freshly ground beans. Would you like to know more?"" }
            ]
            },
            {
                ""QUEST_TYPE"": ""FIND_OBJECT"",
                ""QUEST_TITLE"": ""Hunting for Coffee Beans"",
                ""QUEST_DESC"": ""Embark on a coffee bean hunt in the forest to find the finest coffee beans for your brew."",
                ""OBJECT_TYPE"": ""CUBE_A"",
                ""LOCATION"": ""FOREST""
            },
            {   
                ""QUEST_TYPE"": ""TALK_NPC"",
                ""QUEST_TITLE"": ""Coffee Art at the Village Café"",
                ""QUEST_DESC"": ""Visit the village café and learn the art of coffee decoration from the skilled baristas."",
                ""NPC_NAME"": ""BEAN"",
                ""DIALOGUE"": [
                { ""Tag"": ""FOX"", ""Speech"": ""Bean, I've got the beans. Now, how can I make my coffee look as good as it tastes?"" },
                { ""Tag"": ""BEAN"", ""Speech"": ""Great! Coffee art is next. Let's get creative with latte designs. Oh, and here's a little tip - serve it with a smile."" }
            ]
            },
            {
                ""QUEST_TYPE"": ""COLLECT_COINS"",
                ""QUEST_TITLE"": ""Bean's Coffee Showdown"",
                ""QUEST_DESC"": ""Challenge Bean to a coffee brewing showdown and impress with your newfound coffee skills."",
                ""NUM_COINS"": 5
            }
        ]
        }";
        return str;
    }

    
}
