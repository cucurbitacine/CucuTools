- [CucuTools](#cucutools)
  - [Install](#install)
  - [Async](#async)
    - [Enumerator as Task:](#enumerator-as-task)
    - [Coroutine as Task:](#coroutine-as-task)
    - [UnityEvent as Task:](#unityevent-as-task)
    - [UnityEvent as Task:](#unityevent-as-task-1)
  - [Attributes](#attributes)
    - [*CucuButton*](#cucubutton)
    - [*CucuLayer*](#cuculayer)
    - [*CucuReadOnly*](#cucureadonly)
    - [*CucuScene*](#cucuscene)
    - [**PlayerRigidController**](#playerrigidcontroller)
    - [**VisionController \& TouchController**](#visioncontroller--touchcontroller)
    - [**HoverController \& DragController**](#hovercontroller--dragcontroller)
    - [***How setup player***](#how-setup-player)
  - [**Damage System**](#damage-system)
    - [**Damage**](#damage)
    - [**DamageEvent**](#damageevent)
    - [**DamageSource**](#damagesource)
    - [**DamageReceiver**](#damagereceiver)
    - [**DamageManager**](#damagemanager)
    - [**DamageFactory**](#damagefactory)
    - [***Sequence of methods calls***](#sequence-of-methods-calls)
    - [**Examples**](#examples)

# CucuTools

## Install

- ```Window``` > ```Package manager``` > ```Add Package from git URL...``` >
  - ```https://github.com/cucurbitacine/CucuTools.git?path=/Assets/CucuTools#develop```

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

### **PlayerRigidController**

<!-- ![playerrigid] -->

[>>> Watch video about Player Rigid Controller <<<][playerrigidvideo]

> ***Features***
> 
> - rigidbody movement
> - ground checking
> - platform detecting
> - allows use it as npc

### **VisionController & TouchController**

[>>> Watch video demonstration of vision and touch <<<][visiontouchvideo]

> *One Ring to rule them all*
> 
> - ***VisionController*** checks what we are looking at at the moment
> - ***VisionController*** keeps RaycastHit, so you could use it how do you want
> - ***TouchController*** uses a ***VisionController*** to check how close the object we are looking at is
  
|Vision|Touch|
|:-:|:-:|
|![vision]|![touch]|

### **HoverController & DragController**

<!-- ![hoverdrag] -->

- ***HoverController*** and ***DragController*** use a ***TouchController*** to handle interactive objects
- just put ***HoverableBehaviour*** or ***DragableBehaviour*** on object with *Rigidbody* - and go on
- ***DragController*** takes the weight of objects into account when dragging
- physically dragging objects

> *Hover Controller* is maximum simple.
> 
> ![hover]

> *Drag Controller* is a little more complicated
> 
> ![drag]
>
> - ***Fast Drag***: if turned off - physically dragging objects by changes velocity and angularVelocity of rigidbody. else - uses *MovePosition* and *MoveRotation*
> - ***Is Phys Gun***: (like PhysGun in Half Life 2) if turned on - saves rotation before drag and rotates object relative eyes. else - rotates objects only around axis Y
> - ***Dragging Speed Max***: max speed of dragging object
> - ***Use Drop Distance***: if turned on - drops objects when real object's real position to far from expected position
> - ***Drop Distance Max***: max allowed distance between object's real position and expected position. may vary if the weight of the object is taken into account
> - ***Use Drag Smooth***: if turned on - more smooth dragging
> - ***Drag Smooth***: degree of smooth
> - ***Use Power per Mass***: if turned on - dragging properties depend on object's mass
> - ***Max Mass***: max allowed dragging object's mass
> - ***Power by Mass***: relation of dragging force to weight
> - ***Drag Position and Rotation Offsets***: world offsets dragging position and rotation
> - ***Drag Physic Material***: if not null - change *Physic Material* during dragging
> - ***Why Player and Touch?***: it takes a player for ignoring collision colliders during dragging. touch is required for handle available objects

### ***How setup player***

[>>> Watch video how use player system <<<][playersystemvideo]

---
## **Damage System**

- simple
- intuitive clear
- flexable
- extentionable


### **Damage**

Base damage class to be passed between objects

```c#
public class Damage
{
    public int amount;
    public bool critical;
}
```

### **DamageEvent**

Damage event represents a event of damage. Who hit, who had damaged, how, where... 

```c#
public class DamageEvent
{
    public readonly Damage damage = null;
    public readonly DamageSource source = null;
    public readonly DamageReceiver receiver = null;

    // ...
}
```

### **DamageSource**

Bad guy, who creates damage and send it to damage receivers.

Concrete realisations of DamageSource would *Create Damage*. It could be just Damage or some child of Damage.

When source will be *Generate Damage*, it must know who is his target. Because later it could *Handle Damage* event and change it. For example, add some more damage to goblins!

For us useful and comfortable is function *SendDamage* which... sending damage!

```c#
public abstract class DamageSource : MonoBehaviour
{
    [Space] public bool mute = false;

    public DamageManager manager = null;
    
    public abstract Damage CreateDamage();

    public DamageEvent GenerateDamage(DamageReceiver receiver)
    {
        // ...
    }

    public virtual void SendDamage(DamageReceiver receiver)
    {
        // ...
    }

    protected virtual void HandleDamage(DamageEvent e)
    {
    }
}
```

### **DamageReceiver**

Poor whipping boy...

Well, it is receiving damage. That's it. His child could *Handle Damage* as well. Also it has event *on Damage Received*.

```c#
public class DamageReceiver : MonoBehaviour
{
    [Space]
    public bool mute = false;

    public DamageManager manager = null;
    
    [Space]
    public UnityEvent<DamageEvent> onDamageReceived = new UnityEvent<DamageEvent>();

    public void ReceiveDamage(DamageEvent e)
    {
        // ...
    }
    
    protected virtual void HandleDamage(DamageEvent e)
    {
    }
}
```

### **DamageManager**

Imagine two seven-headed hydras fighting each other. Each head is a separate source of damage and has different attack bonuses. And each head can also receive damage based on its own defence bonuses. In addition, each hydra has a set of different bonuses that are not associated with specific heads.

So Hydra-A's head-3 bites Hydra-B's head-5. How do we know that it was Hydra-A that attacked Hydra-B? And how do I apply the common hydra bonuses to the attack?

This is where the *DamageManager* comes into play. *DamageSource* and *DamageReceiver* have a link to *DamageManager*, which represent fighting entities.

Note: having a *DamageManager* is optional! But if you have a need for it, just point out which sources and receivers belong to the *DamageManager*.

```c#
public class DamageManager : MonoBehaviour
{
    [Space]
    public UnityEvent<DamageEvent> onDamageReceived = new UnityEvent<DamageEvent>();
    
    [Space]
    public List<DamageSource> sources = new List<DamageSource>(); 
    public List<DamageReceiver> receivers = new List<DamageReceiver>(); 

    public void ReceiveDamage(DamageEvent e)
    {
        onDamageReceived.Invoke(e);
    }
    
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
    public DamageEvent GenerateDamage(DamageReceiver receiver)
    {
        var e = new DamageEvent(CreateDamage(), this, receiver);

        HandleDamage(e);

        if (manager != null)
        {
            manager.HandleDamageAsSource(e);
        }

        return e;
    }
    // ...
}

public class DamageReceiver : MonoBehaviour
{
    // ...
    public void ReceiveDamage(DamageEvent e)
    {
        if (mute) return;
        
        HandleDamage(e);
        
        if (manager != null)
        {
            manager.HandleDamageAsReceiver(e);
        }
        
        onDamageReceived.Invoke(e);
    }
    // ...
}
```

### **DamageFactory**

*Damage Factory* is *ScriptableObject*. So you can create some factories, and use it. For example, create *Damage Factory* for melee iron sword. Put some knights with this swords in scene. And you will could change damage amount of all of its knights.

```c#
public abstract class DamageFactory : ScriptableObject
{
    public abstract Damage CreateDamage();
}

public class DamageSourceReference : DamageSource
{
    [Space]
    public DamageFactory factory;
    
    public override Damage CreateDamage()
    {
        return factory.CreateDamage();
    }
}
```

### ***Sequence of methods calls***

- *DamageSource*: void SendDamage(DamageReceiver receiver)
  - *DamageSource*: DamageEvent GenerateDamage(DamageReceiver receiver)
    - *DamageSource* : Damage CreateDamage()
    - *DamageSource* : void HandleDamage(DamageEvent e)
    - *DamageManager*: void HandleDamageAsSource(DamageEvent e)
- *DamageReceiver*: void ReceiveDamage(DamageEvent e)
  - *DamageReceiver*: void HandleDamage(DamageEvent e)
  - *DamageManager* : void HandleDamageAsReceiver(DamageEvent e)
  - *DamageReceiver*: onDamageReceived.Invoke(info)
  - *DamageManager*: void ReceiveDamage(DamageEvent e)
  - *DamageManager*: onDamageReceived.Invoke(e)

### **Examples**

Below is an example where there is
- gun whose damage depends on its level
- damage source of elemental damage (fire, water, etc.)
- damage receiver in the head (doubles the damage)
- damage receiver in the arm (halves damage, but not 0)
- zombies that take double fire damage

<!-- ![zombie] -->

[>>> Watch video about damage system with zombies <<<][zombievideo]

```c#
public class Gun : DamageSourceReference
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

        template.Apply(dmg);

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
public class HeadShotDamageReceiver : DamageReceiver
{
    protected override void HandleDamage(DamageEvent e)
    {
        e.damage.amount *= 2;
    }
}
```

```c#
public class ArmShotDamageReceiver : DamageReceiver
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

[playerrigidvideo]:https://streamable.com/8z39uf
[visiontouchvideo]:https://streamable.com/ppnqjt
[playersystemvideo]:https://streamable.com/20ll9q
[zombievideo]:https://streamable.com/vbeaku

[vision]:Images/PlayerSystem/vision.png
[touch]:Images/PlayerSystem/touch.png
[hover]:Images/PlayerSystem/hover.png
[drag]:Images/PlayerSystem/drag.png