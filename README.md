# GenericModEngine

This is a CSharp mod manager/loader inspired by the way Rimworld handle mods and that aims to be able to integrate with any Godot game using C# scripting language, and potentially any other game-engine using C# code as the code in this project aim to be as engine-agnostic as possible. 

Please note that this is targeted towards game developers intending to add mod support to their game, **this likely won't help you if you're a player wanting to add mods a retail game**.

Also this is currently WIP, so it's absolutely not remotely near an usable state.

# Planned Features

- Mod list/load order organising, with automatic fix for simple errors such as incorrect dependencies order
- Loading and merging of JSON Data from active mods
- Loading of active mods assemblies with depedency isolation
- Application of JSON Patches from active mods to the JSON Data from other mods or the base game
- Communication with mod assemblies and game engine through C# events.

Please note that the data the game engine will get from this Mod Engine will be raw JSON, it is the game's responsability to validate and parse it into game usable objects. 

Furthermore, this is a code only library, providing an UI to interact with mods is also the game's responsiblity.