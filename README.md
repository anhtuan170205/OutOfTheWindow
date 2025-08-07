# Out Of The Window

OutOfTheWindow is a first-person survival shooter where players battle through endless waves of enemies in a day-night cycle.  
Built with Unity 6, this project showcases modular game system design, event-driven architecture, and scalable gameplay loops.

## 🎯 Project Highlights

- Developed a full **state-driven game manager** to handle Bootstrap, Main Menu, In-Game, Paused, and GameOver states.
- Implemented a **day-night cycle system** that dynamically alters game behavior and visuals.
- Designed an **enemy wave spawner** with increasing difficulty and turn-based progression.
- Integrated **player combat mechanics**, including shooting, reloading, dashing, and weapon switching.
- Created a **shop system** allowing players to buy healing, shields, and unlock new weapons.
- Used **event-based input** and decoupled systems to maintain clean and scalable code architecture.
- Leveraged Unity’s **coroutines**, **scriptable objects**, and **custom singleton pattern** for efficient game state management.

## 🏗️ Tech Stack

| Technology   | Description                                  |
|--------------|----------------------------------------------|
| Unity        | 6.x                                           |
| C#           | Core game logic and architecture             |
| Input System | Unity’s new event-based Input System         |
| Audio        | SFX and background music tied to gameplay    |
| UI Toolkit   | UI elements for shop, HUD, and transitions   |
| Scene Flow   | Scene loading and transition management      |

## 📂 Project Structure

| Folder              | Description                          |
|---------------------|--------------------------------------|
| `Assets/Scripts/`   | Core scripts for managers and systems |
| `Assets/Scenes/`    | Game scenes (MainMenu, Game, etc.)    |
| `Assets/Prefabs/`   | Player, enemies, weapons, and UI prefabs |
| `Assets/Audio/`     | Background music and sound effects    |
| `Assets/UI/`        | UI elements like shop and HUD         |
| `Assets/Resources/` | Data assets and references            |
| `ProjectSettings/`  | Unity project settings                |
| `Packages/`         | Package dependencies                  |

## 🛠️ Getting Started

### Prerequisites

- Unity Hub (latest version)
- Unity 6.x LTS installed

### Clone the Repository

```bash
git clone https://github.com/anhtuan170205/OutOfTheWindow.git
