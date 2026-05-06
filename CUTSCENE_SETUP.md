# Cutscene Trigger Setup Guide

## Overview
The `CutsceneTrigger` script creates a trigger that plays audio and a video cutscene, then loads the next level.

## Setup Instructions

### 1. Create an Empty GameObject
- In your scene, create a new empty GameObject
- Name it something like "CutsceneTrigger" or "LevelTransition"
- Position it where you want the trigger to activate

### 2. Add Components
Add the following components to your GameObject:

#### A. Box Collider (for the trigger)
1. Add Component → Physics → Box Collider
2. Check **"Is Trigger"** checkbox
3. Adjust the size and center to cover your desired trigger area

#### B. CutsceneTrigger Script
1. Add Component → Search for "CutsceneTrigger"
2. Select the script you just created

#### C. Video Player
1. Add Component → Video → Video Player
2. In the Inspector, configure:
   - **Render Mode**: Set to "Render Texture" or "Camera Near Plane" (depending on your needs)
   - If using "Render Texture":
     - Create a new Render Texture asset
     - Assign it to the "Target Texture" field
     - Create a new Material with this texture and assign it to an in-world plane

### 3. Configure the CutsceneTrigger Script

In the Inspector, you'll see the following fields:

**Audio Settings:**
- **Audio Clip**: Drag your audio file here
- **Audio Volume**: Set the volume (0-1 range)

**Video Settings:**
- **Video Clip**: Drag your video file here
- **Video Player**: Drag the VideoPlayer component from this GameObject (or another)

**Trigger Settings:**
- **Trigger Once**: If TRUE, the trigger only activates once. If FALSE, it can trigger multiple times
- **Trigger Tag**: Set to "Player" (or whatever tag you want to trigger it)

### 4. Ensure Your Player Has the Correct Tag
- Select your Player GameObject
- In the Inspector, set its Tag to match the "Trigger Tag" (default is "Player")
- If your player is a Car, you may want to check both "Player" and "Car" tags in the script logic

### 5. Test

Press Play in Unity and walk/drive your player into the trigger area. The sequence should be:
1. Audio plays
2. Video plays after audio finishes
3. Next level loads after video finishes

## Video Setup Options

### Option A: UI Canvas (Recommended for simple cutscenes)
1. Create a Canvas if you don't have one
2. Add a RawImage component to the Canvas
3. In the VideoPlayer, set:
   - **Render Mode**: "Render Texture"
   - Create a new Render Texture
   - Assign it to the RawImage's Texture field

### Option B: 3D Plane (For in-world video)
1. Create a Plane in your scene
2. Create a new Material and set its Main Texture to a Render Texture
3. Assign the Material to the Plane
4. In VideoPlayer, set:
   - **Render Mode**: "Render Texture"
   - Assign the Render Texture to both the VideoPlayer and the Material

### Option C: Camera Near Plane
1. Set **Render Mode** to "Camera Near Plane"
2. Video will display directly on the camera (fullscreen effect)

## Important Notes

- **Audio + Video Duration**: The audio plays first, then the video. Make sure they're synced properly if needed
- **SceneController Requirement**: This script requires SceneController to be present in the scene (which you already have)
- **Next Level**: The script calls `SceneController.instance.NextLevel()` which loads the next scene by build index
- **Collider Requirements**: The GameObject must have a collider with "Is Trigger" checked
- **Player Tag**: Make sure your player has the correct tag set

## Troubleshooting

| Issue | Solution |
|-------|----------|
| Trigger doesn't activate | Check that your player has the correct tag and the collider is marked as trigger |
| Audio doesn't play | Ensure AudioClip is assigned and volume is > 0 |
| Video doesn't play | Ensure VideoClip is assigned and VideoPlayer component is referenced |
| Next level doesn't load | Check that SceneController exists and the next scene is in Build Settings |
| Video shows but no audio | Make sure VideoPlayer has "Audio Output Mode" set correctly in its settings |

