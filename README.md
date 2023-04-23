# Common implementations Project
[![Made with Unity](https://img.shields.io/badge/Made%20with-Unity-57b9d3.svg?style=flat&logo=unity)](https://unity3d.com)
[![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/martin-obert/unity-plugins-common/publish-package.yml?label=UPM%20deployment)](http://upm.obert.cz/)
[![Unity Tests](https://github.com/martin-obert/unity-plugins-common/actions/workflows/unity-tests.yml/badge.svg)](https://github.com/martin-obert/unity-plugins-common/actions/workflows/unity-tests.yml)

Only include essential components, that are commonly used across multiple projects. 

## Prerequisites

UniTask:
install - https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask

UniRx:
download - https://github.com/neuecc/UniRx/releases/download/7.1.0/UniRx.unitypackage

## Installation
Update projects manifest.json with following entry
```
"dependencies": {
    ...
    "com.obert.common": "1.14.0",
    ...
},
"scopedRegistries": [
    {
      "name": "Obert",
      "url": "http://upm.obert.cz",
      "scopes": [
        "com.obert"
      ]
    }
  ]
```

Or follow Unity official setup guide for [scoped registeries](https://docs.unity3d.com/Manual/upm-scoped.html)

# Contribution
- use [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/), so the Release Please can auto catch on those changes

# Package

Follow package specific ReadMe for features description
[README](https://github.com/martin-obert/unity-plugins-common?path=/Assets/Scripts/Readme.md)
