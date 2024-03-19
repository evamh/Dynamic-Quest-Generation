# Research Weblog - Dynamic Quest Generation Architecture

## Introduction
This research weblog outlines the development and research process for this project. I've chosen to separate this documentation into separate sections, each encompassing one area of development. There are two main categories: 

1. Development of the Unity game 
2. The research process for the GPT model integration (including fine-tuning and testing)

**Note**: all links and Unity Asset Store links used in the development of this project can be found at the end of this document. 

## Inspiration 

I've always loved playing open-world games, such as *The Elder Scrolls V: Skyrim* (Betheseda), *The Witcher 3: Wild Hunt* (CD Projekt Red) and *The Legend of Zelda: Breath of the Wild* (Nintendo). When playing these games, one thing that stands out to me the most is the agency I have as a player in the world. I'm always curious how my actions affect the environment. In addition, it's exciting when there are spontaneous events that occur during game play - for instance, you come across someone in your travels or you witness a conversation between two non-player characters (NPCs). These can become repetitive though, which is where the use of a large-language model (LLM) such as GPT could potentially have a powerful impact. I am fascinated by the use of LLMs in spinning off emergent narratives from these interactions in the game world, and how it can tailor gameplay to player behaviour. How can LLMs be used in video games to create worlds that feel alive and that respond to the behaviour of the agents that inhabit it?

**Aim: a fully dynamic, self-contained world that organically evolves using generative A.I. - used to dynamically generate quests to create a cohesive world that is shaped by player actions** 


## Brainstorming

I knew I wanted to explore the use of a LLM (such as GPT) in video games; more specifically, my primary goal was to investigate how to use GPT to generate quests and events dynamically in a game world. However, I wasn't sure exactly what that would look like or how to go about implementing this. 

I started brainstorming ideas on a Freeform board (image below). With this board, I began mapping out the key components of the game I wanted to make, including the game's main concept, technical details and overall gameplay/A.I. flow.

<img src="Images/freeform board.png" alt="Main gameplay flow" width="80%"/>

<br> I also deliberated on the game style and theme. I knew I wanted to explore LLMs in open-world RPG, but what would be the premise of the game? What kinds of games do I like to play, and how can GPT be integrated into the main gameplay flow to create emergent stories?

The idea came to me in two parts. Over the last year I've been playing *Stardew Valley* (Eric Barone), a farming based simulator game. I've always loved the cozy, relaxing tone of the game where the player can accomplish several goals: building their farm, performing side quests, interacting with villagers, etc. I knew I wanted to create a game that was similar in its relaxing, nature atmosphere. Additionally, I had been toying around with the idea of a journal-based game for a while, where the player can input thoughts into a diary which subsequently alters the game.

As a result, I decided to create a small open-world game, in a forest environment, where the quest and story is entirely decided by the player. The game would have a journal mechanism, with an intuitive interface for the player, that bridges the game to the GPT model. The journal entries made by the player would prompt GPT to generate quests based off the players thoughts using the OpenAI API.

One important consideration was the the main gameplay loop and the relationship between the GPT model and the game environment:

<img src="Images/Main gameplay flow.png" width="80%"/>

<br> As is highlighted in the diagram above, the game world would naturally evolve through the collaboration between player and the GPT model. 

# Game Project and Code
The game was developed in Unity, and all of the code for the game was written in C#. ChatGPT helped me with the game's code, especially for debugging purposes. 

## Game Characters 

### Design 
Another source of inspiration for this game is the popular video game series *Animal Crossing* (Nintendo). I wanted to create a village of various animals and as the game takes place in a woodland-type environment, the specific species followed naturally. I decided to model the main character (the player) as a Fox.

For the NPCs, I designed three such characters to populate the world with. I listed their main characteristics and had ChatGPT summarise their personality accordingly:

**Baker Chip**: A friendly baker who enjoys making people laugh with jokes and sharing his baked goods. He also likes experimenting with new doughnut flavors. Tag: “CHIP”

**Explorer Ivy**: Nature enthusiast who explores forests and beaches, passionate about uncovering natural mysteries and eager to share her discoveries with others. Tag: “IVY”

**Bean**: Coffee aficionado and barista at the local coffee shop, passionate about coffee and coffee art. Enjoys writing a novel during off-hours at the same coffee shop. Tag: “BEAN”

### Modelling
To model the fox, I followed a Blender tutorial on YouTube (Ksenia Starkova's *3D Fox Character Modeling | Blender Tutorial for Beginners [RealTime]*, link below) and altered it slightly:

<img src="Images/Fox blender.png" width="25%"/> <img src="Images/Fox blender 2.png" width="26%"/>

For Ivy, I used the same fox mesh and changed the materials:

<img src="Images/Ivy blender 1.png" width="25%"/> <img src="Images/Ivy blender 2.png" width="20%"/>

I decided to model Baker Chip as a bear. I started with the same base of the fox model, but altered it accordingly to model a bear with an apron and a chef's hat:

<img src="Images/chip blender.png" width="30%"/>

<br> Bean was modelled as a vest-wearing rabbit, in a similar process:

<img src="Images/Bean blender 1.png" width="25%"/> <img src="Images/Bean blender 2.png" width="25%"/>

### Default NPC
I also created a default NPC which is also modelled after the fox. This to be used when the model returns an undefined NPC in its output (more on this below). 

<img src="Images/randomly generated 1.png" width="20%"/> <img src="Images/randomly generated 2.png" width="24%"/>

### Unity Versions
And here are the characters in the game!

<img src="Images/Character images.png" width="50%"/>

### Rigging and animations
I used Adobe's Mixamo software (link below) to rig and animate the characters. I first set them up in a T-Pose in Blender, then imported this into Mixamo for the appropriate rig and animations. Finally, the models, rigs and animations were imported into Unity for the final round of processing. 

There are two animations for the Fox: walking and idle. When the player is moving in the world, the Fox enters the walking animation and is idle otherwise. I use an Animator Controller in Unity for this logic.

The NPCs are all in an idle state, with their own animator controllers. 

## World Design

### Environment
I knew I wanted to create an open-world type game, where the player can explore at their leisure. However, I kept the size relatively small in order to focus on the quest generation architecture. 

<img src="Images/Environment sketch.png" width="50%"/>

<br> First, I sketched out what I roughly envisioned the world to look like, and separated it into three locations: the forest, the town and the beach. 

In Unity, I started by using the terrain tools to create the world and sized it accordingly. I used various textures to paint the ground (such as gravel and sand), and downloaded 3D assets of trees and plants to enrich the world with vegetation.

<img src="Images/env 3.png" width="45%"/> <img src="Images/env 4.png" width="45%"/>
<img src="Images/beach 1.png" width="45%"/> <img src="Images/beach 3.png" width="45%"/>
<img src="Images/forest 1.png" width="45%"/> <img src="Images/forest 2.png" width="45%"/>
<img src="Images/cave image.png" width="45%"/> <img src="Images/view from rock.png" width="45%"/>

For the beach, I added two rectangular planes and applied a water material to each. This material has movement to give the appearance of gentle waves. The material was a part of the Fantastic Village Pack downloaded from the Unity Asset Store (link below).

<img src="Images/water 1.png" width="45%"/> <img src="Images/water 2.png" width="45%"/>

### Locations

Using 3D assets from this same asset pack, I populated the world accordingly. I created a town square and neighborhood, where I placed the NPCs. 

<img src="Images/market 1.png" width="45%"/> <img src="Images/market 2.png" width="45%"/>
<img src="Images/market 3.png" width="45%"/> <img src="Images/market 4.png" width="45%"/>
<img src="Images/town square 1.png" width="45%"/> <img src="Images/town square 2.png" width="45%"/>
<img src="Images/town square 3.png" width="45%"/> <img src="Images/nature 1.png" width="45%"/>
<img src="Images/village 1.png" width="45%"/> <img src="Images/village 2.png" width="45%"/>

## Other Game Assets 
The majority of assets used in this project, for both the environment design and the objects to find, are part of packages downloaded from the Unity Asset Store. All links for these assets are below.

### Item Scriptable Object 

I created a specific ScriptableObject class to contain the logic for each potential object to find in FIND_OBJECT quests. This makes it easier to encapsulate each object, assign it a prefab and tag, and use a manager to maintain a list of all items available to the GPT model. Logic such as updating the system instructions with the list of available objects and creating object pools (explained below) is done automatically as it sources the list directly from the manager. 

<img src="Images/bottle inventory item.png" width="500"/> 


### Object Pooling 
To instantiate these items in the game, I created object pools for each Inventory Item SO. When the game launches in Unity, each pool creates 5 inactive clones (more for coins). If a FIND_OBJECT quest needs one of these instances, the quest logic will make the instance active. One of the main benefits of object pooling is that it's more efficient than instantiating and destroying objects on the fly. 

## Quest System Design

The quest system was one of the more complicated parts of the project to implement. As it was my first time creating a fully-fledged quest system, I did my research and found a video that provided me with a solid starting point: *How to create a Quest System in Unity | RPG Style | Including Data Persistence* by Shaped by Rain Studios (link below).

### Iteration 1
The first iteration of the quest system used singular quest objects, so that the goal was encapsulated in a single step. While this essentially served as a quest system, the quests themselves were lacking substance given the fact that they were treated as individual objects rather than narratively linked in some way.

The first iteration defined the three types of quest objects: 
- **Collecting Coins (COLLECT_COINS)** where the goal is to collect a number of coins
- **Finding an Object (FIND_OBJECT)** where the goal is to find a specific object in the game world
- **Talk to NPC (TALK_NPC)** where the goal is to engage in discussion with a specific NPC

<br> Iteration 1 provided a good testing ground for the initial quest system. Below is an example of a TALK_NPC quest using this system:

<img src="Images/Unity Example NPCQuest 1_1.png" width="50%"/> <img src="Images/Unity Example NPCQuest 1_2.png" width="49%"/> <img src="Images/Unity Example NPCQuest 1_3.png" width="50%"/>


### Final system used 

I decided to upgrade this system and use quest series rather than singular objects, where each series has multiple quest steps to complete in order to accomplish the series' goal. This method allows for continuity between multiple quest steps and leads to more engaging quests series overall.

The diagram below shows a high-level overview of the quest system that this game uses:

<img src="Images/Quest series diagram.png" width="40%"/>

<br> Broadly, the system uses QuestSeries objects, where each series has a specific goal and is composed of a number of QuestObjects (or steps). Once all of the quest objects in a series are completed, the series completes and the next one (if any exist) begins.

At a high level, the code is structured as follows:
- A **QuestSeriesObject** class, where each instance is a singular quest series object. It contains information such as ID, title, description and a list of QuestObject instances
- A **QuestObject** class, which is the base class of each quest object and contains information such as ID, title, description, status and coin reward
- A **CollectCoinsQuest** class, which extends the QuestObject class and encapsulates the specific logic for collecting coins quests
- A **FindObjectQuest** class, which extends the QuestObject class and encapsulates the specific logic for finding object quests
- A **TalkNPCQuest** class, which extends the QuestObject class and encapsulates the specific logic for talking to NPC quests
- A **QuestSeriesManager** class, which is a singleton class and manages the list of all quest series objects. It is responsible for setting the state of quests (such as active and completed) and moving to the next quest in the series when one is completed

Below are some examples of the player view of the quest series system. The dropdown allows the player to view different quest series, and each series view displays the quest description (green box), and each quest step in pink boxes. They are blurred out when completed. When the full series is completed, it updates in the dropdown accordingly:

<img src="Images/doughnut dispute.png" width="45%"/> <img src="Images/Bean's coffee quest.png" width="45%"/>
<img src="Images/political brew.png" width="45%"/>

## Quest Types

There are three main quest types that the GPT model uses to construct quest series. 

For each quest type, I implemented a safeguard mechanism in the code to protect against any undefined values and/or provide bounds for numerical values. There is also logic in the code to quit the quest step immediately and move on to the next one if necessary.  

### Collect Coins (COLLECT_COINS)
The goal in this quest type is to collect a number of coins. When a COLLECT_COINS quest step starts up, the code instantiates *x* number of coins to find in the game world. When the player has found all the coins, the quest completes. Each coin also adds to the total number of coins that the player has. This amount can be viewed in the Inventory tab (by clicking the Inventory button). 

The JSON representation of the quest has the additional parameter NUM_COINS (number of coins to collect).

<img src="Images/coin screenshot 1.png" width="30%"/> <img src="Images/coin screenshot 2.png" width="30%"/> <img src="Images/coin screenshot 3.png" width="30%"/>

<br>

**Safeguard Mechanism**:
<br>It's important to ensure that the number of coins to search for is feasible. As a result, I've added code to cap the number of coins to search for to 10 (otherwise it becomes tedious for the player). 

### Talk to NPC (TALK_NPC)
The goal of this quest is to engage in dialogue with a specified NPC. The model both chooses the NPC to speak to and supplies both sides of the conversation (for the NPC and the main character, Fox). 

The list of available NPCs gets appended automatically to the system instructions before being sent to the GPT model, so that the model knows the viable options. 

The JSON representation has two additional parameters: NPC_NAME and DIALOGUE (a structure where each element is one side of the conversation, i.e. the character name and their respective line of dialogue).

<img src="Images/talk_npc 2.png" width="45%"/> <img src="Images/talk_npc 3.png" width="45%"/> 

<br>

**Safeguard Mechanism**
<br>The GPT model may return an undefined NPC tag. Left alone, this would cause errors to be thrown and the player wouldn't be able to continue. There is also the option of quitting the quest immediately and moving onto the next one in the series, but this would break the flow of the story as it would skip over pertinent steps. 

I've decided to implement a Default NPC Generator logic, whereby if the GPT model returns an NPC Tag that isn't defined, the architecture generates a default character and assigns the quest step/dialogue to this new character instead. They are generated in a random location in the game world.

This logic ensures that the player can still continue with the quest in a way that feels natural to the world and in line with the quest. 

I tested this first with a cube object to ensure the logic was working:
<br><img src="Images/random npc 1.png" width="60%"/> 
<br><img src="Images/random npc 2.png" width="45%"/> <img src="Images/random npc 3.png" width="45%"/> 

In this example, Cleora is not any of the defined NPCs and requires the default NPC instantiation instead. 

Then I switched to the actual 3D model, along with the suitable animation of Idle: 
<br><img src="Images/random npc quest 1.png" width="45%"/> 
<br><img src="Images/random npc quest 3.png" width="45%"/> <img src="Images/random npc quest 4.png" width="45%"/> 

### Find Object (FIND_OBJECT)
In this quest type, the player must search for a specific object in a part of the game world (either the village, the forest or the beach). 

The viable objects are defined in the Unity editor as a list of Inventory objects, where each object has a tag and a prefab. The list of objects as well as locations gets concatenated automatically to the system instructions sent to the GPT model so that the model can return quests that are technically feasible in the game world. It also means that removing or adding any items/locations automatically instructs the model accordingly, making the architecture more flexible. 

The JSON representation has two additional parameters: OBJECT and LOCATION. 

Each location has a box collider that is sized accordingly to the the size of the location. To place the object in the correct location, the code will randomly generate a position vector in the world and check that it collides with the specified location. It will repeat until it finds a match. Once a match is found, the object's transform position vector is set to the new position accordingly. 

<img src="Images/find obj cheese.png" width="30%"/> <img src="Images/find paper scroll.png" width="20%"/><img src="Images/cherry obj-1.png" width="23%"/> <img src="Images/find treasure.png" width="24%"/>

Some technical considerations for this quest type include:
- Ensuring that the generated objects are positioned in an accessible spot (i.e. not hidden under or in another game object)
- Ensuring that the objects are sized correctly

<br>

**Safeguard Mechanism**
<br> Similarly to the TALK_NPC quest above, the code implements a default asset if the one returned by GPT is undefined. In this case it is a primitive Unity object, a cube. 

<img src="Images/undefined obj 1.png" width="30%"/> <img src="Images/undefined obj 2.png" width="30%"/><img src="Images/undefined obj 3.png" width="33%"/> 

In this example, I hardcoded a dummy quest to test the undefined object PEN. As expected, the code falls back to the cube, allowing the player to continue with the quest. 

## Journal Entry Design 

Another key component of this game is the journal, which allows players to input their thoughts. These entries get packaged into a prompt and sent to the GPT model via the OpenAI API. 

The UI for this is relatively simple: the player writes their thoughts into the text field and hit Enter. The entry then gets displayed in the scroll window above, so that the history of journal entries are visible:

<img src="Images/journal system.png" width="60%"/>


## Save/Load Functionality 

I also added functionality to save the current game state and load it back up when stopping and starting the game in Unity. I followed a YouTube tutorial from Shaped by Rain Studios: *How to make a Save & Load System in Unity | 2022* (link below), which helped me set up the basic infrastructure. I then determined which game values needed to be saved and loaded accordingly (including quests and their respective states, dialogues had with NPCs, current coin amount, etc). I also save the history of GPT API calls.

I added a 'New Game' button at the bottom of the UI to easily restart the game, effectively erasing the game's history (and GPT API calls) in Unity. I also extended the code to update and reset the UI accordingly. 

## Game Play Mechanics  

The player can interact with the world using a keyboard and mouse in the following ways:

**Movement**
- **W, A, S, D** keys to move forward, backwards, left and right
- **Arrow keys** to move the angle of the camera and pan around
- **Spacebar** to jump

**Quest Interaction**
- **Spacebar** when close to an object to pick it up (both coins and other objects)
- The **T** key to start speaking with an NPC and spacebar to continue the conversation

**UI interaction**
- **Mouse** to click on any buttons in the UI
- **Keyboard** to add text to the journal 

# Communication with GPT
One of the main challenges in this project was figuring out how to communicate with GPT such that it would return actionable quests in the game world. More specifically, there are two main goals that need to be achieved:

1. Make the model understand its role and the output it has to return. It also needs to comprehend the game context such as its world and characters 
2. Process the model output in a tangible and playable manner in the Unity game

## Integration with GPT-3.5 

### Testing in ChatGPT
I began by playing around in ChatGPT to get a sense of how I could communicate with the model and how well it understood the ask:

<img src="Images/chatgpt test 1.png" width="60%"/>

<br>I was pleasantly surprised that it produced output that was in line with the initial ask. I kept prompting the model, adding a layer of extra detail each time (such as labelling the quests, adding specific fields, creating JSON output). Below are screenshots of such prompts:

<img src="Images/chatgpt test 2.png" width="60%"/>
<img src="Images/gpt test 3.png" width="60%"/>
<img src="Images/chatgpt test 4 npc.png" width="60%"/>
<img src="Images/gpt test 5.png" width="60%"/>
<img src="Images/gpt test 6.png" width="60%"/>

<br> Through this process, I was able to arrive at a set of system instructions to prompt the model with, and the JSON output I expected it to return. This would be a good start to integrate GPT-3.5 in my game.

## System Instructions
Providing the model with a complete set of system instructions was one of the harder challenges in this project. The more detailed the instructions are, the higher the likelihood that GPT would return sensible and correctly formatted output. However, this also comes at a monetary cost as the token count can increase quite quickly. 

The first iteration of communication between GPT and the game used the default model GPT-3.5. As a result, the model has no context or knowledge about its role and the game world at the start, so all information has to be provided in the system instructions. Below is one of the first iterations of system instructions that was sent to the GPT model:

<img src="Images/original system instructions.png" width="60%"/>

<br> Clearly, these instructions are quite long and verbose. They even provide sample output for the model to learn from, which lead to a greater cost due to extra tokens. This approach of sending requests to the original GPT-3.5 model with lengthy instructions works, but is not ideal in this game environment where tokenisation (cost of tokens sent to the model) is a serious consideration.

## Integration in the Game 

To integrate the model's output in the game environment, I extended the codebase with scripts responsible for: 
    
1. Making API calls to the GPT model 
2. Parsing the model's output into an instance of a quest series class, where each quest step was also parsed into instances of its respecive quest type class. 

To make API calls to the GPT model, I installed a package built by RageAgainstThePixel that wraps the OpenAI API calls into Unity functions. I also took a Udemy class titled *ChatGPT x Unity: The Ultimate Integration Guide* by Tabsil Makes Games, which helped me with the code needed to set up the GPT model and keys as well as make/receive calls to the GPT model using the API. The links to both the package and the course are below.

## Fine-tuning 

As I was researching the API capabilities on the OpenAI documentation site, I came across fine-tuning as a mechanism to tailor a GPT model to a specific application. This seems like it's a good fit for my project, where I could customise a model to my specific game world and required JSON format output while cutting down on the system instructions. 

### Data Pre-Processing 
Fine-tuning a model requires constructing a dataset to train the model with, where each example in the set is a packaged message of system, user and assistant prompts. The dataset should comprise of enough examples to paint a good picture of the game world, address edge cases and demonstrate the correct JSON return format.

I began by compiling a list of each parameter/nuance I wanted the fine-tuned model to understand and grasp. For example:

1. Example for getting to know that Chip is a baker
2. Example for getting to know that Chip likes to bake doughnuts 
3. Example for getting to know that Ivy likes to explore 
4. Example for getting to know that Bean loves coffee 
5. Example for finding an object of type BOTTLE, such a bottle of wine
6. Example for finding an object of type CHERRY, to bake a pie with

Then, I prompted ChatGPT to come up with example user prompts and corresponding quests, packaged correctly in system/user/assistant format. Below is an example of a parameter to include in finetuning (getting to know Chip) and the corresponding data point to train the model with. It includes the system instructions, the example user/journal prompt and a sample quest generated in the correct JSON format.

![Alt text](<Images/chip datapoint example.png>)

Once I had compiled all the examples, I wrote a python script to clean and validate the data, then create an OpenAI file. Once the file had been created, I created the fine-tuning job, keeping the default training parameters. 

The script to clean and validate the data was taken from OpenAI's *Data preparation and analysis for chat model fine-tuning* and adapted for my use case with help from ChatGPT. The YouTube video *How to Fine-tune a ChatGPT3.5 Turbo Model - Step by Step Guide* by All About AI also provided useful functions for file handling and creating fine-tuning jobs. All links are below. 

### Cutting down on System Instructions
Because the fine-tuned model would have learned the correct output format (both for the quest series and for each different quest type), I reduced the size of the system instructions significantly: 

<img src="Images/reduced system instructions.png" width="80%"/>

<br> Clearly, by providing the model with examples that all follow the same format, I can remove the explicit instruction for it to include standard JSON fields like QUEST_TYPE, QUEST_TITLE, QUEST_DESC. These are present in every single example, so the model already knows what the basic output format should look like. I could also remove the example in the first set of system instructions. 

As discussed, the code automatically adds the relevant world details at the end of the system instructions before the first API call to the model. This way the specific world details are automatically updated when assets or NPCs are added to the game. 

![Alt text](<Images/instructions details-2.png>)

### Fine-tuned Model 1

<img src="Images/Finetuning job 1.png" width="50%"/>

<br> I tested the first fine-tuned model in the OpenAI playground, using the streamlined system instructions. I used a couple of example prompts and analysed the corresponding JSONs to check that the output was both technically correct and contextually cohesive.

Here are some examples of the model's output in the OpenAI playground:

<img src="Images/finetuning 1 example.png" width="60%"/>
<img src="Images/finetuning 1 example 2.png" width="60%"/>

<br> These tests are promising in that they are correctly formatted as quest series and fit the style of the game. However, somed details are contextually incorrect (such as Bean being the village historian) and feel relatively incomplete as the dialogue ends on an ambiguous note. 

### Fine-tuned Model 2

To address some of the limitations of the first fine-tuned model, I added more examples to better capture the nuances of the game world and created a second fine-tuned model. 

<img src="Images/finetuning job 2.png" width="50%"/>

<img src="Images/finetuning 2 example 1.png" width="60%"/>
<img src="Images/finetuning 2 example 2.png" width="60%"/>
<img src="Images/finetuning 2 example 3.png" width="60%"/>

<br> These examples highlight the model's ability to produce more involved quests that feel complete, although some of the details are technically ambiguous (in the third example, it asks the user to search for a journal but the technical asset to seek is a bottle). 

Nevertheless, these examples represent technically complete quest series that are aligned with the overall theme and style of the game. Time to test them out in the game itself!

## Integration in the Game

I changed the API model to the second fine-tuned one directly in the Unity editor, so that all API calls are sent there rather than the standard GPT-3.5 model. 

<img src="Images/gpt model unity.png" width="60%"/>

<br> The second main goal of this section is building the code so that it can parse the model's output and create actionable playable quests in the game. There are several steps here:

1. Package and send the player's journal entry to the API
2. Receive the model's output
3. Parse the output and create an instance of the QuestSeries class
4. Parse each quest step into instances of their respective quest type
5. Add the quest series to the queue in the QuestSeriesManager class. Have the UI update the dropdown with the series
6. When the quest is active, have the QuestSeriesManager iterate through each quest step
7. For each quest step, perform any pre-processing steps (such as releasing objects from the pool and placing them in the world, preparing NPC dialogue, etc). The UI should display each step as well
8. When the player actions any steps (collects a coin, talks to an NPC, finds an object), the quest should complete and make the next step active. The UI should update accordingly by blurring the step
9. When all steps are complete, the quest series should complete. The UI should update accordingly

Below are examples of in-game playthroughs, which demonstrate the players perspective from start to finish. The dynamic quest generation architecture is able to successfully take a player thought, generate a related quest series, and update the game environment so that the player can perform each step in an actionable way.


### Quest 1 - The Hidden Treasure

Fox noticed a treasure chest in the forest and made a note of it in the journal:

<img src="Images/treasure 2 journal.png" width="60%"/>

<br> The quest series and its corresponding quest steps get instantiated in the game and the series becomes active, allowing the player to begin:

<img src="Images/treasure 1.png" width="60%"/>
<img src="Images/treasure 2.png" width="60%"/>
<img src="Images/treasure 3.png" width="60%"/>
<img src="Images/treasure 4.png" width="60%"/>
<img src="Images/treasure 5.png" width="60%"/>
<img src="Images/treasure 6.png" width="60%"/>
<img src="Images/treasure 7.png" width="60%"/>
<img src="Images/treasure 8.png" width="60%"/>
<img src="Images/treasure 9.png" width="60%"/>


### The Beach Adventure
The player expresses a wish to go to the beach, but isn't sure what to do once there.

<img src="Images/beach adventure 1.png" width="60%"/>
<img src="Images/beach adventure 2.png" width="60%"/>
<img src="Images/beach adventure 3.png" width="60%"/>
<img src="Images/beach adventure 4.png" width="60%"/>
<img src="Images/beach adventure 5.png" width="60%"/>
<img src="Images/beach adventure 6.png" width="60%"/>
<img src="Images/beach adventure 7.png" width="60%"/>
<img src="Images/beach adventure 8.png" width="60%"/>
<img src="Images/beach adventure 9.png" width="60%"/>
<img src="Images/beach adventure 10.png" width="60%"/>

<br>

These playthroughs highlight the architecture's ability to bridge the GPT model and the game's code, creating actionable goals and altering the 3D world accordingly. 


# Future Avenues 

In the written thesis component of this project, I delve into the future paths of the dynamic quest generation architecture and how extending the framework can lead to more immersive and cohesive quest series. Here I focus on the future avenues for the game rather than the framework as a whole. 

### 1. Extending inventory logic

Keeping a log of items found in an inventory opens up a new set of possible gameplay mechanisms. Perhaps a new quest type could be to collect a number of a certain object type, or the player could use items in the inventory to gift to NPCs or solve other quests. 

In addition, the player's current inventory can be additional data sent to the GPT model and further tailor quests. 

### 2. Adding object interactions

By recording items that the player has seen in the game world, the quest system can be further customised to player behavior and their experience. For instance, if the player stumbles upon a book in the forest, this interaction can be sent to the GPT model and the model can return a quest accordingly. This would heighten both immersion and player agency, making the world feel alive and interconnected. 

However, special care would be required to ensure the same object doesn't repeatedly spawn new, unrelated quests as that can quickly dismantle the cohesive narrative of the game.  

### 3. Incorporating Audio

Audio serves as a powerful element in any game, increasing player immersion and crafting truly magical worlds. Incorporating audio aligned with the dynamically generated quests has the potential to be very powerful. For instance, if the quest series is relaxed in tone, a happy song can play in the background. On the other hand, if the player must search for a mysterious object in the world, atmospheric music with suspensful undertones would be appropriate.

This feature would also apply to NPC interactions. If the dialogue returned by the GPT model is cheerful, then sound bites of exclamations can accompany their on-screen speech. If the NPC appears to be upset or annoyed, then an expression of frustration would be more suitable. 

### 4. Extending the quest system with additional quest types

I've spoken about this in my thesis, but will mention it here as well as it also ties in to the game itself. Adding additional quest types such as action-based quests or building-business quests would both add a layer of complexity and help shape the overarching theme of the game. 

Ideally, the GPT model should be trained on multiple examples of any additional quest types, as to properly understand each of their use cases and JSON representations.

### 5. Expanding the world 

Expanding the game world with additional locations offers the model more options when generating quests, as well as enhances the exploration experience in the game. This game iteration uses three locations in order to focus on refining the GPT model's ability to tailor quests based on location and invoke the correct behaviour from the game's code. As this functionality is at a stable state, including additional locations in the world would infuse it with more personality and greater gameplay potential.  

### 6. Default NPC generation

Another direction is to augment the default NPC generation framework if the the GPT model deems that it's more suitable for the quest under design. A drawback of relying on a predefined set of NPCs is that their personalities and previous interactions may not fit with a new quest and may subsequently break coherence if they are an actor in this quest. Allowing the model to create new NPCs tailored for the quest would enhance the narrative cohesion of both the storyline and the world. 

This would also extend the quest generation architecture into an NPC generation framework, allowing the player to truly create their own world populated with unique agents. 

# Acknowledgements

I would like to thank my supervisor and course leader Phoenix Perry for the understanding and support shown to me during these last few months, as well as for teaching us to find joy in our daily lives. This support has helped me immeasurably in being able to work on this project calmly and confidently. I would also like to thank all of my teachers, the CCI technicians and my fellow classmates for all of their help, guidance and inspiration during my time at the CCI.

# Unity Asset Store - Assets Used

**Conifers [BOTD] - Forst**: https://assetstore.unity.com/packages/3d/vegetation/trees/conifers-botd-142076

**FANTASTIC Village Pack - Tidal Flask Studios**: https://assetstore.unity.com/packages/3d/environments/fantasy/fantastic-village-pack-152970

**Food Props - Unity Technologies**: https://assetstore.unity.com/packages/3d/food-props-163295

**Grass Flowers Pack Free - ALP**: https://assetstore.unity.com/packages/2d/textures-materials/nature/grass-flowers-pack-free-138810

**Mega Fantasy Props Pack - Karboosx**: https://assetstore.unity.com/packages/3d/environments/fantasy/mega-fantasy-props-pack-87811

**Outdoor Ground Textures - A Dog's Life Software**: https://assetstore.unity.com/packages/2d/textures-materials/floors/outdoor-ground-textures-12555

**Rocky Hills Environment - Light Pack - Tobyfredson**: https://assetstore.unity.com/packages/3d/environments/landscapes/rocky-hills-environment-light-pack-89939

**Skybox Series Free - Avionx**: https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-series-free-103633

**Terrain Sample Asset Pack - Unity Technologies**: https://assetstore.unity.com/packages/3d/environments/landscapes/terrain-sample-asset-pack-145808

**Unity Terrain URP Demo Scene - Unity Technologies**: https://assetstore.unity.com/packages/3d/environments/unity-terrain-urp-demo-scene-213197

<br>

# Links 

**3D Fox Character Modeling | Blender Tutorial for Beginners [RealTime], Ksenia Starkova**: https://youtu.be/aMRRNC1J6tU?si=DXqA-xCHEUV9pDOE

**Blender**: https://www.blender.org/

**Mixamo**: https://www.mixamo.com/#/

**How to Fine-tune a ChatGPT 3.5 Turbo Model - Step by Step Guide, All About AI**: https://www.youtube.com/watch?v=2Pd0YExeC5o&t=500s&ab_channel=AllAboutAI

**How create a Quest System Design in Unity | RPG Style | Including Data Persistence, Shaped by Rain Studios**: https://youtu.be/UyTJLDGcT64?si=JYy9dGT-NXZnfMip

**How to make a Save & Load System in Unity | 2022, Shaped by Rain Studios**: https://youtu.be/aUi9aijvpgs?si=cQg8arza4maLpPYg

**ChatGPT x Unity : The Ultimate Integration Guide, Tabsil Makes Games**: https://www.udemy.com/course/unity-chatgpt/

**OpenAI API Unity Package by RageAgainstThePixel**: https://github.com/RageAgainstThePixel/com.openai.unity

**Data preparation and analysis for chat model fine-tuning, Michael Wu, Simón Fishman**: https://cookbook.openai.com/examples/chat_finetuning_data_prep

**OpenAI API Fine-tuning Documentation**: https://platform.openai.com/docs/guides/fine-tuning



