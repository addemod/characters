# Addemod/characters
[![License](https://img.shields.io/github/license/Addemod/characters.svg)](LICENSE)
[![Build Status](https://img.shields.io/appveyor/ci/Addemod/characters/master.svg)](https://ci.appveyor.com/project/Addemod/characters)
[![Release Version](https://img.shields.io/github/release/Addemod/characters/all.svg)](https://github.com/Addemod/characters/releases)

Add characters to your server

## Installation
~~Install the plugin into your server from the [NFive Hub](https://hub.nfive.io/Addemod/characters): `nfpm install Addemod/characters`~~

## Server events
### CharacterEvents.Selecting
#### Server -> Server
Called when at the start of a character being selected
```csharp
void OnCharacterSelecting(ICommunicationMessage e, Character character)
```

### CharacterEvents.Selected
#### Server -> Server
Called when a character has been completely selected
```csharp
void OnCharacterSelecting(ICommunicationMessage e, CharacterSession characterSession)
```

### CharacterEvents.Deselecting
#### Server -> Server
Called when a character is being deselected (i.e. when a character session is no longer in use)
```csharp
void OnCharacterDeselecting(ICommunicationMessage e, CharachterSession characterSession)
```

### CharacterEvents.Deselected
#### Server -> Server
Called when a character has been completely deselected
```csharp
void OnCharacterDeselected(ICommunicationMessage e, CharachterSession characterSession)
```
