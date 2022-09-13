# Skallu-Utils

<p align="center">
	<img alt="package.json version" src ="https://img.shields.io/github/package-json/v/Skallu0711/Skallu-Utils" />
	<a href="https://github.com/Skallu0711/Skallu-Utils/issues">
		<img alt="GitHub issues" src ="https://img.shields.io/github/issues/Skallu0711/Skallu-Utils" />
	</a>
	<a href="https://github.com/Skallu0711/Skallu-Utils/pulls">
		<img alt="GitHub pull requests" src ="https://img.shields.io/github/issues-pr/Skallu0711/Skallu-Utils" />
	</a>
	<a href="https://github.com/Skallu0711/Skallu-Utils/blob/master/LICENSE">
		<img alt="GitHub license" src ="https://img.shields.io/github/license/Skallu0711/Skallu-Utils" />
	</a>
	<img alt="GitHub last commit" src ="https://img.shields.io/github/last-commit/Skallu0711/Skallu-Utils" />
</p>

My package of Unity [extensions](http://en.wikipedia.org/wiki/Extension_method) and stuff, which I find useful.

Keep in mind that everything here is under development, so things may change in the future :)

### Extensions
Bunch of very useful extension methods
```csharp
using SkalluUtils.Extensions;
```
* [GameObject Extensions](Runtime/Extensions/GameObjectExtensions.cs) 
* [String Extensions](Runtime/Extensions/StringExtensions.cs)
* [Vector2 & Vector3 Extensions](Runtime/Extensions/VectorExtensions.cs)
* [Collider & Collider2D Extensions](Runtime/Extensions/ColliderExtensions.cs)
* [List Extensions](Runtime/Extensions/ListExtensions.cs)

### Property Attributes
Bunch of custom property attributes
```csharp
using SkalluUtils.PropertyAttributes;
```
* [PropertyAttributes](Runtime/PropertyAttributes)

### Utils
Some utils that i often implement
```csharp
using SkalluUtils.Utils;
```
* [Utils](Runtime/Utils)
* [Editor GUI Utils](Editor/Utils/EditorGUIUtils.cs)
