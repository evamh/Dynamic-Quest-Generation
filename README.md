# Dynamic Quest Generation
## Using a fine-tuned GPT model to craft customised quests in a 3D, RPG style video game 

**Note**: Because of the large project size, the full repo can be found at the following link: https://git.arts.ac.uk/22012837/ThesisProject

For an in-depth explanation of the research process and links to resources used, please check out the *Research Weblog* element in the repo. 

## Overview

This is a 3D, RPG style game set in a woodland environment. The player controls Fox, a villager in this small town. 

The main premise of this game is that the player crafts their own quests using the dynamic quest generation architecture. Using a in-game journal, the player enters thoughts which makes an API call to the GPT model and generates a unique quest tailored to the input. The model itself is fine-tuned using a training dataset of input/quest examples, which aim to paint a good picture of both the details of the game world and the desired tone and output format of the quests. 

The model outputs a *quest series*, which is composed of one or several *quest* objects. A quest object can be either to collect coins, talk to an NPC or find an object in the game world. The game's code then translates this output into actionable and playable quests in the game, allowing the player to experience their tailored quest start to finish. 

The game was built in the Unity game engine with the code written in C#. The fine-tuning code was written in Python.

<img src="https://git.arts.ac.uk/storage/user/650/files/e159bd89-0a83-4200-8936-cff6bc026508" width="40%"/> <img src="https://git.arts.ac.uk/storage/user/650/files/71b16776-ef38-43c9-8da8-b720ab8d1e85" width="40%"/>
<img src="https://git.arts.ac.uk/storage/user/650/files/2c835cd3-9d00-43d9-be0b-7313e4df735a" width="40%"/> <img src="https://git.arts.ac.uk/storage/user/650/files/67b017d4-72fa-4d1a-bc65-6b0bc1a8a4ab" width="40%"/>
<img src="https://git.arts.ac.uk/storage/user/650/files/699ccc13-fb48-4ccb-8c20-e5a881791b9f" width="40%"/> <img src="https://git.arts.ac.uk/storage/user/650/files/53eeb460-0b6f-4c82-af04-dd978af920c9" width="40%"/>


## Gameplay Mechanics

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

## Set Up Instructions

1. Create an OpenAI account (https://openai.com/) and ensure that you have both an API key and organisation ID
2. In a document, copy and paste the keys in the following format. Save the file and note the path

![api key screenshot](https://git.arts.ac.uk/storage/user/650/files/d639108d-9486-4ad6-815c-370aefca0fc7)

3. In the Unity Editor, navigate to the Hierarchy view and go to Game Managers -> Managers
4. Look for the GPT Manager in the Inspector
5. Under Path To Keys, paste the path to the file with your keys:

![gpt model unity](https://git.arts.ac.uk/storage/user/650/files/94ccf25c-cada-490e-b8d3-cfb03475a607)

6. You should now be able to play the game and generate API requests to the fine-tuned model!


