# Unity Helper Package

This package was created in order to help the development of simple projects by providing easy, ready to use systems.

## Table of contents

- [Installation](#installation)
- [Singleton](#singleton)
- [Event System](#event-system)
- [Game View Buttons](#game-view-buttons)

## Installation

This package follows the [Package layout recommendation by Unity](https://docs.unity3d.com/Manual/cus-layout.html).

To install, simply open the *Package Manager*, and add the package from the git URL <https://github.com/JonathanRN/UnityHelperPackage.git>

![Package manager instruction](Documentation~/package-manager.png)

> Unity version 2019.4 or later is recommended.

### Updating

A limitation from Unity prevents git packages to update automatically or when refreshing the Package Manager. However it is still possible to update the package by re-adding the package using the git URL.

> Deleting the package beforehand is **not** necessary.

---

## Singleton

A simple `singleton.cs` file is available to the Singleton pattern into applications.

This implementation uses [lazy initialization](https://docs.microsoft.com/en-us/dotnet/api/system.lazy-1?view=net-5.0) which allows the Singleton to be created only once when the static `Instance` is called.

### Usage

```csharp
public class Example : Singleton<Example>
{
    public void ExampleMethod()
    {
        // Code...
    }
}

// Somewhere else in the code
{
    Example.Instance.ExampleMethod();
}
```

---

## Event System

A unique event system was created for easier management of communication between Unity components.

### Usage

To register (subscribe) to an event, simply call `SubscribeTo` from any object (even primitives). Specify the name of the event (string) and the callback when said event is called (Action\<object, object\>).

```csharp
public class Example : MonoBehaviour
{
    private void Awake()
    {
        // This works
        this.SubscribeTo("yourEventName", (sender, args) =>
        {
            // Do something with the event
        });

        // This also works
        int myInt = 3;
        myInt.SubscribeTo("yourEventName", (sender, args) =>
        {
            // Do something with the event
        });
    }
}
```

To unsubscribe from an event, call `UnsubscribeFrom` from the same object.

```csharp
public class Example : MonoBehaviour
{
    private void Awake()
    {
        this.SubscribeTo("yourEventName", (sender, args) =>
        {
            // Do something with the event
        });

        int myInt = 3;
        // This will throw an exception since `myInt` never subscribed to the event
        // myInt.UnsubscribeFrom("yourEventName");
    }

    private void OnDestroy()
    {
        this.UnsubscribeFrom("yourEventName");
    }
}
```

To call (emit) an event, call `Emit` with the event name, and an optional parameter.

```csharp
{
    // Anywhere else in the code
    this.Emit("yourEventName");

    // Another example
    // Since emitting is possible from any object, this makes it easier to pass in arguments
    this.SubscribeTo("yourEventName", (sender, args) =>
    {
        // `sender` here will be the integer itself!
        // int received = (int)sender;
        // received = 3
    });

    int myInt = 3;
    myInt.Emit("yourEventName");
}
```

You can even wait for an event as a Unity coroutine. Simply use `WaitForEvent` with an event name.

```csharp
public class CoroutineTest : MonoBehaviour
{
    private IEnumerator Start()
    {
        // Start the wait coroutine
        StartCoroutine(EventCoroutine());

        // Wait for 2 seconds to simulate something else going on
        yield return new WaitForSeconds(2);

        this.Emit("event");
    }

    private IEnumerator EventCoroutine()
    {
        // This code runs as soon as the coroutine is started
        Debug.Log("Before");
        yield return this.WaitForEvent("event");
        // This gets called as soon as the event is finally received
        Debug.Log("After");
    }
}
```

---

## Game View Buttons

The single file `GameViewButton` allows to add buttons to the Unity Game View with a simple call. The buttons are created upon start of the application and removed on exit. They are located by default on the top-left of the window.

![Game View buttons](Documentation~/game-view-buttons.png)

### Usage

```csharp
// It is recommended to call from Awake, Start, or whenever initialization happens.
private void Start()
{
    GameViewButton.Show("Button title goes here", OnClickAction);
}
```
