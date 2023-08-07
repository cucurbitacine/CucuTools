- [CucuTools](#cucutools)
  - [*Install*](#install)
  - [Async](#async)
    - [Enumerator as Task:](#enumerator-as-task)
    - [Coroutine as Task:](#coroutine-as-task)
    - [UnityEvent as Task:](#unityevent-as-task)
    - [UnityEvent as Task:](#unityevent-as-task-1)
  - [Attributes](#attributes)
    - [DrawButton](#drawbutton)
    - [LayerSelect](#layerselect)
    - [ReadOnly](#readonly)
    - [SceneSelect](#sceneselect)
  - [Damage System](#damage-system)
    - [Damage](#damage)
    - [DamageEvent](#damageevent)
    - [DamageSource](#damagesource)
    - [DamageReceiver](#damagereceiver)
    - [DamageManager](#damagemanager)
    - [DamageFactory](#damagefactory)
    - [DamageBox \& HitBox](#damagebox--hitbox)
    - [Sequence of methods calls](#sequence-of-methods-calls)
    - [*Examples*](#examples)

# CucuTools

## *Install*

- ```Window``` > ```Package manager``` > ```Add Package from git URL...``` >
  - ```https://github.com/cucurbitacine/CucuTools.git?path=/Assets/CucuTools```

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

### DrawButton

It is displaying buttons in inspector which invoke the current method. Working only inside ***CucuBehaviour***.

> - *name* - button display name. Methods name is default name
> - *order* - order of sort of buttons. Between 0 and 255, 127 is default
> - *group* - group name of buttons. Buttons are stacking together with equals group names
> - *colorHex* - color code which using for button display 

- Code:
```c#
public class ExampleAttributes : CucuBehaviour
{
    [DrawButton]
    public void SomeMethod1() { }
    
    [DrawButton("Some Method 2", 1, "Grouped")]
    public void SomeMethod2() { }
    
    [DrawButton(name: "Some Method 3", order: 0, group: "Grouped", colorHex: "ff0000")]
    public void SomeMethod3() { }
}
```

- Inspector:
![cucubutton]

### LayerSelect

It is enabling pick layer (as integer) into inspector.

- Code:
```c#
[CucuLayer]
public int LayerValue;
```

- Inspector:
![cuculayer]

### ReadOnly

It is displaying fields with readonly mode.

- Code:
```c#
[ReadOnly]
public bool boolValue = true;
[ReadOnly]
public int intValue = 42;
[ReadOnly]
public float floatValue = 1.618f;
[ReadOnly]
public string stringValue = "Hello world";
```

- Inspector:
![cucureadonly]

### SceneSelect

It is enabling pick scene (as string) into inspector. Pickable scene must be in build settings.

- Code:
```c#
[SceneSelect]
public string sceneName;
```

- Inspector:
![cucuscene]

---
## Damage System

- simple
- intuitive clear
- flexable
- extentionable


### Damage

Base damage class to be passed between objects.

```c#
public class Damage
{
    public int amount;
    public bool critical;
    // ...
}
```

### DamageEvent

Damage event represents a event of damage. Who hits, who had damaged, and how.

```c#
public class DamageEvent
{
    public readonly Damage damage = null;
    public readonly DamageSource source = null;
    public readonly DamageReceiver receiver = null;
    // ...
}
```

### DamageSource

Bad guy, who creates damage and sends it to damage receivers.

Concrete realisations of *DamageSource* would *Create Damage*. It could be just Damage or some child of Damage.

For us useful and comfortable is function *SendDamage* which... sending damage!

Also you could to override *Handle Damage*. For example, to add some more damage to goblins!

```c#
public abstract class DamageSource : MonoBehaviour
{
    public DamageManager manager = null;
    
    public abstract Damage CreateDamage();

    public void SendDamage(DamageReceiver receiver)
    {
        // ...
    }

    protected virtual void HandleDamage(DamageEvent e)
    {
    }
}
```

### DamageReceiver

Poor whipping boy...

Well, it is receiving damage. That's it. His child could *Handle Damage* as well. Also it has event *on Damage Received*.

Also you could to override *Handle Damage*. For example, to increase damage from undead!

```c#
public class DamageReceiver : MonoBehaviour
{
    public DamageManager manager = null;
    // ...
    public void ReceiveDamage(DamageEvent e)
    {
        // ...
    }
    
    protected virtual void HandleDamage(DamageEvent e)
    {
    }
}
```

### DamageManager

Imagine two seven-headed hydras fighting each other. Each head is a separate source of damage and has different attack bonuses. And each head can also receive damage based on its own defence bonuses. In addition, each hydra has a set of different bonuses that are not associated with specific heads.

So Hydra-A's head-3 bites Hydra-B's head-5. How do we know that it was Hydra-A that attacked Hydra-B? And how do I apply the common hydra bonuses to the attack?

This is where the *DamageManager* comes into play. *DamageSource* and *DamageReceiver* have a link to *DamageManager*, which represent fighting entities.

Note: having a *DamageManager* is optional! But if you have a need for it, just point in sources and receivers which *DamageManager* they belong to.

```c#
public class DamageManager : MonoBehaviour
{
    // ...
    public virtual void HandleDamageAsSource(DamageEvent e)
    {
    }
    
    public virtual void HandleDamageAsReceiver(DamageEvent e)
    {
    }
    // ...
}
```

Below is how *Damage Event* handling is sources and receivers when they have *DamageManager*

```c#
public abstract class DamageSource : MonoBehaviour
{
    // ...
    public void SendDamage(DamageReceiver receiver)
    {
        var dmg = CreateDamage();
        var e = new DamageEvent(dmg, this, receiver);

        HandleDamage(e);

        if (manager)
        {
            manager.HandleDamageAsSource(e);
        }
        // ...
    }
    // ...
}

public class DamageReceiver : MonoBehaviour
{
    // ...
    public void ReceiveDamage(DamageEvent e)
    {
        HandleDamage(e);
        
        if (manager)
        {
            manager.HandleDamageAsReceiver(e);
        }

        // ...
    }
    // ...
}
```

### DamageFactory

*Damage Factory* is *ScriptableObject*. So you can create some factories, and use it. For example, create *Damage Factory* for melee iron sword. Put some knights with this swords in scene. And you will could change damage amount of all of its knights.

```c#
public abstract class DamageFactory : ScriptableObject
{
    public abstract Damage CreateDamage();
}

public class DamageSourceFactory : DamageSource
{
    [Space]
    public DamageFactory factory;
    
    public override Damage CreateDamage()
    {
        return factory.CreateDamage();
    }
}
```

### DamageBox & HitBox

Now you know how to ~~deal pain~~ send damage. You will probably find the best and most convenient ways to do it in Unity. 
But if you don't want to do it yourself, there is a ready-made solution: ***DamageBox*** and ***HitBox***.

How it works:
- *DamageBox* linked to some *DamageSource*
- *DamageBox* Triggered or Collided with some *HitBox*
- *HitBox* linked to some *DamageReceiver*
- *DamageBox* sends damage from his *DamageSource* to *HitBox's* *DamageReceiver*

That's easy. *DamageBox* has a few parameters:
- HitType : How to collide? - Through triggering or colliding?
- HitMode : When to collide? - Enter, Stay or Exit?

Reminder: Please, don't forget set up your GameObject's *Collider* and *Rigidbody* to make it possible to *DamageBox* could triggering or colliding. He doesn't know how to do it on his own yet. More information about triggering and colliding you can find on official documentations.

```c#
public abstract class DamageBox : MonoBehaviour
{
    public HitType hitType = HitType.TriggerOrCollision;
    public HitMode hitMode = HitMode.Stay;
    // ...
    public DamageSource source;
    
    public void Hit(HitBox hitBox)
    {
        // ...
        source.SendDamage(hitBox.receiver);
    }
}
```

Well, *HitBox* is more easier... No one parameter, just put it on GameObject and go on! He is ready to take on all the pain of the world!

```c#
public abstract class HitBox : MonoBehaviour
{
    // ...
    public DamageReceiver receiver;
}
```

### Sequence of methods calls

- *Source* : **Send Damage** to *Receiver*
  - *Source* : Create Damage
  - *Source* : Handle Damage
  - *Manager* : Handle Damage as Source
    - *Receiver* : **Receive Damage** from *Source*
      - *Receiver* : Handle Damage
      - *Manager* : Handle Damage As Receiver
      - *Receiver* : Damage Received Event
      - *Manager* : Damage Received Event
  - *Source* : Damage Delivered Event
  - *Manager* : Damage Delivered Event

### *Examples*

Below is an example where there is
- gun whose damage depends on its level
- damage source of elemental damage (fire, water, etc.)
- damage receiver in the head (doubles damage)
- damage receiver in the arm (halves damage)
- zombies that take double fire damage

```c#
public class Gun : DamageSource
{
    public int level = 1;
    // ...
    protected override void HandleDamage(DamageEvent e)
    {
        e.damage.amount += (level - 1);
    }
    // ...
}
```

```c#
public class ElementalDamageFactory : DamageFactory
{
    // ...    
    public override Damage CreateDamage()
    {
        Damage dmg;
        
        switch (Random.Range(0, 5))
        {
            case 0: dmg = new FireDamage(); break;
            case 1: dmg = new WaterDamage(); break;
            case 2: dmg = new AirDamage(); break;
            case 3: dmg = new EarthDamage(); break;
            case 4: dmg = new LightningDamage(); break;
            default: dmg = new Damage(); break;
        }

        return dmg;
    }
}

public class ElementalDamage : Damage
{
}

public class FireDamage : ElementalDamage
{
}
// ...
```

```c#
public class HeadDamageReceiver : DamageReceiver
{
    protected override void HandleDamage(DamageEvent e)
    {
        e.damage.amount *= 2;
    }
}
```

```c#
public class ArmDamageReceiver : DamageReceiver
{
    protected override void HandleDamage(DamageEvent e)
    {
        e.damage.amount = Mathf.FloorToInt(e.damage.amount * 0.5f);
    }
}
```

```c#
public class ZombieDamageManager : DamageManager
{
    // ...
    public override void HandleDamageAsReceiver(DamageEvent e)
    {
        if (e.damage is FireDamage fire)
        {
            fire.amount *= 2;
        }
    }
    // ...
}
```

---

<!-- Images -->

[cucubutton]:Images/Attributes/CucuButton.png
[cuculayer]:Images/Attributes/CucuLayer.png
[cucureadonly]:Images/Attributes/CucuReadOnly.png
[cucuscene]:Images/Attributes/CucuScene.png