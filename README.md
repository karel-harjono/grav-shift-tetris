# Grav-Shift-Tetris
A Tetris clone with a twist `Gravity Shift`

## Demo Video
https://github.com/user-attachments/assets/971f9671-4371-457a-b4d6-a60025ad225a

## Playable Link
https://jgresl.itch.io/grav-shift-tetris

## The Twist
`Gravity Shift` works by randomly changing which edge Tetris pieces gravitate towards. It is a difficult twist because the player has to think about how placing the piece on one edge could restrict the ability to place pieces on the neighboring edges - especially in the corners.

## Versioning
v1.0.1

## Controls
- Move piece up `W`
- Move piece left `A`
- Move piece down `S`
- Move piece right `D`
- Rotate piece left `Q`
- Rotate piece right `E`
- Move piece directly to edge `SPACE`

## Features
- Start game screen
- Controls screen
    - Back button
- In game menu
    - Resume button
    - Restart button
- Core gameplay
    - `Gravity Shift` twist
    - Detect full lines
    - Clear full lines
    - Re-adjust pieces after clearing lines
        - Halfway indicator
- Audio
    - Background music
    - Sound FX
    - Mute / Unmute icons
- HUD Display
    - Show next tile
    - Show next gravity direction
    - Show score
        - DoTween animation updating score
- Game over screen
    - Restart button

## Scoring
- 20 points for clearing 1 row
- 80 points for clearing 2 rows
- 160 points for clearing 3 rows
- 320 points for clearing 4 rows `TETRIS!`

## Cloning & Setting Up the Project
1. Ensure you have
	- **Unity Hub** (Download: [Unity Hub](https://unity.com/download))
	- **Unity Version** (6000.0.31f1)
	- **Git** ([Download Git](https://git-scm.com/)) or **Github Desktop** ([Download Github Desktop](https://desktop.github.com/download/))
2. Clone the project via [Git](https://docs.github.com/en/repositories/creating-and-managing-repositories/cloning-a-repository?tool=cli) or [Github Desktop](https://docs.github.com/en/repositories/creating-and-managing-repositories/cloning-a-repository?tool=desktop)
3. Open **Unity Hub**
4. Click **"Open"** and select the cloned project folder (with Assets, ProjectSettings, Packages)

## References
The following tutorials were used as guidance to create this project:
- https://www.youtube.com/watch?v=ODLzYI4d-J8
- https://github.com/zigurous/unity-tetris-tutorial