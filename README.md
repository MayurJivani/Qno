<div align="center">

# Qno

**Uno, except the cards are quantum gates.**

[Play the web version](https://qno.futile.studio) · [Qno-web](https://github.com/MayurJivani/Qno-web) · [Futile Studio](https://futile.studio)

[![Unity](https://img.shields.io/badge/Unity-2022.3-0b0b0b?style=flat-square&labelColor=0b0b0b&logo=unity&logoColor=white)](https://unity.com)
[![C#](https://img.shields.io/badge/C%23-0b0b0b?style=flat-square&labelColor=0b0b0b&logo=csharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)
[![Photon](https://img.shields.io/badge/Photon-PUN-0b0b0b?style=flat-square&labelColor=0b0b0b)](https://www.photonengine.com/pun)
[![Last commit](https://img.shields.io/github/last-commit/MayurJivani/Qno?style=flat-square&labelColor=0b0b0b&color=3b0a7a)](https://github.com/MayurJivani/Qno/commits/main)

</div>

> **Archive** · Unity original of Qno-web · not maintained

## Overview

Qno is a two-player card game made in Unity. It starts from Uno, but the
special cards are quantum ideas: superposition, entanglement, measurement and
Pauli gates. This repo is the Unity version. The browser rewrite with online play
lives in [Qno-web](https://github.com/MayurJivani/Qno-web) and runs at
[qno.futile.studio](https://qno.futile.studio).

## How it plays

What the scripts do today:

- Connect online with Photon. Pick a room (Room1, Room2 or Room3). The game
  joins a random room with the same room value and two seats, or creates one.
- Pick Player 1 or Player 2. A seat already taken by the other player is disabled.
- The deck has a light side and a dark side. Press **F** to flip it.
  The light side uses Red, Blue, Green and Yellow. The dark side uses Orange,
  Purple, Teal and Pink.
- Click the draw pile to draw. Light side draws from the top, dark side from the bottom.
- Click a card in your hand to play it. It moves to the discard pile.

Each side has 108 cards, built in `CardDatabase.cs`:

- Numbers 0 to 9 in each colour. 0 appears once, 1 to 9 appear twice.
- Two action cards per colour. Light: Pauli X and Pauli Z. Dark: Pauli Y and Teleportation.
- Black wild cards. Light: Entanglement, Measurement, Colour Superposition. Dark: Superposition, Measurement, Colour Superposition.

The action cards are dealt and can be played, but I did not find code that
applies their effects. Treat the rules as not implemented yet.

## Stack

- Unity 2022.3.8f1 (`ProjectSettings/ProjectVersion.txt`)
- C#, in `Assets/Scripts`
- Photon PUN and Photon Realtime, bundled in `Assets/Photon`
- Universal Render Pipeline 14.0.8, TextMesh Pro and QuickOutline (`Packages/manifest.json`, `Assets/QuickOutline`)

## Running it locally

1. Install Unity 2022.3.8f1, the version in `ProjectSettings/ProjectVersion.txt`.
2. Open this folder as a Unity project. Let it import the packages from `Packages/manifest.json`.
3. Open `Assets/Scenes/Cards.unity` or `Assets/Scenes/SampleScene.unity` and press play. I have not checked which one holds the full game.

You need two players to test online play. Two editor windows or an editor
plus a build will do.

## Configuration

There are no environment variables. The one setting you may change is the Photon
app ID, stored as `AppIdRealtime` in
`Assets/Photon/PhotonUnityNetworking/Resources/PhotonServerSettings.asset`.
If you fork this, replace it with an app ID from your own Photon account.

## Project structure

- `Assets/Scripts/NetworkManager.cs`: Photon connection, room join, seat choice
- `Assets/Scripts/UiManager.cs`: connect screen and seat screen
- `Assets/Scripts/GameManager.cs`: deck, draw and flip
- `Assets/Scripts/CardDatabase.cs`: the card list for both sides
- `Assets/Scripts/Card.cs`: card faces, materials and colours
- `Assets/Scripts/HandManager.cs` and `PlayCard.cs`: hand and playing cards
- `Assets/Resources/Textures`: card face textures

## Status

Archived and not maintained. The code has open TODOs, for example in
`GameManager.cs`: `TODO: Handle case when cardNumber >= 108`.

## Licence

No licence file yet, all rights reserved.

---

<p align="center">
  <a href="https://futile.studio"><img src="https://raw.githubusercontent.com/MayurJivani/MayurJivani/main/assets/futile-studio.png" alt="futile.studio" width="180"></a>
  <br>
  <sub>Built by <a href="https://github.com/MayurJivani">Mayur Jivani</a>. Everything is futile. Do it anyway.</sub>
</p>
