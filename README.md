# Unity Event System Utilities

This package provides a static, type-based event management system for Unity projects:

-   `EventManager`: A static class for managing event listeners and broadcasting custom events.
-   `IGameEvent`: An interface to be implemented by all custom event classes.

## Features

-   Type-safe event broadcasting and listening
-   Decoupled event handling for game systems
-   Easy registration and removal of event listeners
-   Supports custom event types via the `IGameEvent` interface
-   Minimal boilerplate for event-driven architecture

## Usage

### Defining an Event

Create a class that implements `IGameEvent`:

```csharp
using ran.utilities;

public class PlayerDiedEvent : IGameEvent
{
    public int playerId;
    public PlayerDiedEvent(int id) { playerId = id; }
}
```

### Listening for Events

Register a listener for your event type:

```csharp
EventManager.AddListener<PlayerDiedEvent>(OnPlayerDied);

void OnPlayerDied(PlayerDiedEvent evt)
{
    Debug.Log($"Player {evt.playerId} died.");
}
```

### Broadcasting Events

Broadcast an event to all listeners:

```csharp
EventManager.Broadcast(new PlayerDiedEvent(42));
```

### Removing Listeners

Remove a previously registered listener:

```csharp
EventManager.RemoveListener<PlayerDiedEvent>(OnPlayerDied);
```

### Clearing All Listeners

Clear all registered listeners (useful for scene changes or cleanup):

```csharp
EventManager.Clear();
```

## API Reference

### IGameEvent

-   Marker interface for custom event types.

### EventManager

-   `AddListener<T>(Action<T> evt)`: Register a listener for event type `T`.
-   `RemoveListener<T>(Action<T> evt)`: Remove a listener for event type `T`.
-   `Broadcast(IGameEvent evt)`: Broadcast an event to all listeners of its type.
-   `Clear()`: Remove all listeners and event collections.

## Installation

### Option 1: Unity Git Package Manager (Recommended)

Add the following line to your project's `manifest.json` dependencies:

```json
"com.ran-utils.event-system": "https://github.com/khalishzhafran/utils-event-system.git"
```

This will automatically fetch and update the package via Unity's Package Manager.

### Option 2: Manual Copy

Copy the `Runtime/EventStystem` folder into your Unity project's `Assets` directory.

## License

Copyright (c) 2025 Ran. Free to use, modify, and distribute for personal and commercial projects as long as this notice remains intact.
