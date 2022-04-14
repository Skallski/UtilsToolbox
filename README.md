# Skallu-Utils
My package of Unity [extensions](http://en.wikipedia.org/wiki/Extension_method) and stuff, which I find useful.

## Usage

## Extensions

All the extensions here are under development.

```csharp
using SkalluUtils.Extensions;
```

### GameObject

```csharp
// destroys all components of game object except the one provided as method parameter
gameObject.DestroyComponentsExceptProvided(spriteRenderer);

// destroys all components of game object except the list of those provided as method parameter
gameObject.DestroyComponentsExceptProvided(new List<Component>{spriteRenderer, rigidBody2D});

// destroys all components of game object
gameObject.DestroyAllComponents();
```

### string

```csharp
// changes color of log message to one provided as method parameter
Debug.Log("sample text".Color(Color.blue));
```

## Property Attributes

```csharp
using SkalluUtils.PropertyAttributes;
```

### Read Only Inspector

`ReadOnlyInspector` is a custom property attribute, which makes serialized variables read-only

```csharp
using SkalluUtils.PropertyAttributes.ReadOnlyInspectorPropertyAttribute;
```

```csharp
[ReadOnlyInspector] [SerializeField] private bool canAttack;
```

## Utils

All the utils here are under development.

```csharp
using SkalluUtils.Utils;
```
