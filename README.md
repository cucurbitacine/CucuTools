# CucuTools

## Async

Features is that it is possible convert ***IEnumerator***, ***Coroutine***, ***UnityEvent*** to ***Task*** (***UnityEvent< T >*** to ***Task< T >***). 

**Examples:**

### Enumerator as Task:
```c#
private IEnumerator SomeEnumerator()
{
    yield return new WaitForSeconds(1f);
}

public async void CallEnumeratorAsTask()
{
    await SomeEnumerator().AsTask();
}
```

### Coroutine as Task:
```c#
private Coroutine _coroutine;

public async void CallCoroutineAsTask()
{
    _coroutine = StartCoroutine(SomeEnumerator());
    
    await _coroutine.AsTask();
}
```

### UnityEvent as Task:
```c#
private UnityEvent _unityEvent;

private void InvokeUnityEvent()
{
    _unityEvent.Invoke();
}

public async void CallUnityEventAsTask()
{
    Invoke(nameof(InvokeUnityEvent), 1f);
    
    await _unityEvent.AsTask();
}
```

### UnityEvent<T> as Task<T>:
```c#
private UnityEvent<float> _unityEvent;

private void InvokeUnityEvent()
{
    _unityEvent.Invoke(1.618f);
}

public async void CallUnityEventAsTask()
{
    Invoke(nameof(InvokeUnityEvent), 1f);
    
    var value = await _unityEvent.AsTask(); // 1.618f
}
```

---
## Attributes

Some useful attributes.

### *CucuButton*

It is displaying buttons in inspector which invoke the current method. Working only inside ***CucuBehaviour***.

> - *name* - button display name. Methods name is default name
> - *order* - order of sort of buttons. Between 0 and 255, 127 is default
> - *group* - group name of buttons. Buttons are stacking together with equals group names
> - *colorHex* - color code which using for button display 

- Code:
```c#
public class ExampleAttributes : CucuBehaviour
{
    [CucuButton]
    public void SomeMethod1() { }
    
    [CucuButton("Some Method 2", 1, "Grouped")]
    public void SomeMethod2() { }
    
    [CucuButton(name: "Some Method 3", order: 0, group: "Grouped", colorHex: "ff0000")]
    public void SomeMethod3() { }
}
```

- Inspector:
![cucubutton]

### *CucuLayer*

It is enabling pick layer (as integer) into inspector.

- Code:
```c#
[CucuLayer]
public int LayerValue;
```

- Inspector:
![cuculayer]

### *CucuReadOnly*

It is displaying fields with readonly mode.

- Code:
```c#
[CucuReadOnly]
public bool boolValue = true;
[CucuReadOnly]
public int intValue = 42;
[CucuReadOnly]
public float floatValue = 1.618f;
[CucuReadOnly]
public string stringValue = "Hello world";
```

- Inspector:
![cucureadonly]

### *CucuScene*

It is enabling pick scene (as string) into inspector. Pickable scene must be in build settings.

- Code:
```c#
[CucuScene]
public string sceneName;
```

- Inspector:
![cucuscene]

---

<!-- Images -->

[cucubutton]:Images/Attributes/CucuButton.png
[cuculayer]:Images/Attributes/CucuLayer.png
[cucureadonly]:Images/Attributes/CucuReadOnly.png
[cucuscene]:Images/Attributes/CucuScene.png