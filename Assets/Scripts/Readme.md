# Unity Plugins - Common

This package contains mainly scripts with implementation of features that I've frequently used in variety of my projects and had to implement them over and over again.

# FEATURES
## GameObject extensions
- methods for transform child objects, like: Destroy(Immediate), ChildrenToArray
- get components of interface

## Background tasks
- wraps around Tasks (UniTasks) and adds monitoring about currently running tasks on the background
- helps to debug what is currently running, improves readability

## Repositories
- simple repository pattern implemented as following variants

### InMemory
- fast and cheap, ideal for prototyping

### Component (Mono)
- for editor time references via inspector

### Component (Scriptable)
- for mid/late stage of development
- allows dev to create scritable object that contains homogenous data/objects

### Logging
- just wrapper around the ```UnityEngine.Debug```
- decouples the Unity API from logging

## Scene Orchestration

Best to use when working with multi scene workflow, so mainly larger projects

- fasten up the load of your application, especially when using streaming assets (like Addressables)
- ability to group scenes into logic 
