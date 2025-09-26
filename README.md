# BusinessLife

BusinessLife is a Unity (C#) mobile prototype for a BitLife-inspired business sim. It features a JSON-driven event engine, deterministic randomness, and a minimal UI for running month-to-month company decisions.

## Project structure

```
Assets/
  Resources/data/        # JSON-driven industries, events, and config
  Scenes/                # Scene placeholders for Boot, Menu, NewGame, Game, Endings
  Scripts/
    Core/                # Game systems (controllers, services)
    Models/              # Serializable data models for JSON and runtime state
    UI/                  # uGUI binding scripts for HUD, modals, tabs, etc.
    Tests/               # NUnit play/edit mode tests covering core systems
```

## Data-driven content

* `industries.json` defines each industry, color theme, and the event IDs available to it.
* `events_*.json` files (tech, food, fashion, real_estate) include 25+ flavorful events each. Add new entries to extend gameplay without touching code.
* `config.json` stores rarity weights, starting year, turn length, and starting attributes per background/education.

All JSON files live under `Assets/Resources/data/` so they are addressable via `Resources.Load<TextAsset>()`. To add new content, duplicate an existing entry, tweak IDs, and ensure the event ID appears in the matching industry list.

## Scenes & setup

The provided `.unity` files are textual placeholders. In Unity:

1. Create the Boot scene and add an empty GameObject with the `BootLoader` component. Set `nextScene` to `MainMenu`.
2. Build the MainMenu with uGUI buttons wired to `MainMenuView` methods (`NewGame`, `ContinueGame`, `OpenSettings`).
3. In the NewGame scene, bind input fields and dropdowns to `NewGameView` serialized fields.
4. In the Game scene, lay out HUD, Dashboard, Event Modal, Tabs, Toast canvas, and Endings panel, wiring them to `GameController`.
5. Optionally create a dedicated Endings scene that references `EndingsView` to recap runs.

## Save data

`SaveService` persists the `GameState` and journal to `Application.persistentDataPath` as JSON (`businesslife_save.json` and `businesslife_journal.json`). Autosaves trigger after each turn, and manual saves can be hooked via the Settings tab.

## Running tests

Use Unity Test Runner (Edit Mode) to execute the NUnit test fixtures under `Assets/Scripts/Tests/`. They cover deterministic RNG, event engine scheduling, effect clamping, and save/load roundtrips.

## Extending the game

* Add more industries by creating new `events_newindustry.json` files and updating `industries.json`.
* Create new traits or endings by extending the JSON schemas and updating the relevant services.
* Hook up the placeholder tabs (Team, Shop, Journal, Settings) with additional UI when expanding scope.

Enjoy iterating on BusinessLife!
