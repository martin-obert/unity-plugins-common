# Project

Only include essential components, that are commonly used across multiple projects. 
This package shouldn't have any 3rd party dependencies.

## Structure
Root of this repository is basically a Unity project with ```.idea``` folder for JetBrains Rider

The package root is ```Assets\Scripts```, so to update for ex.: manifest, navigate to ```Assets\Scripts\package.json```
Other configuration is same as Unity describes in manual for [custom packages](https://docs.unity3d.com/Manual/CustomPackages.html)

Include sample assets in the ```Assets\Scripts\Samples``` folder. From here they are automatically copied to ```Assets\Scripts\Samples~``` folder upon new package version is deployed. The reason for this process is, that Unity doesn't track assets in ```Assets\Scripts\Samples~``` making it difficult to work with.

# Package

[README](https://github.com/martin-obert/unity-plugins-common?path=/Assets/Scripts/Readme.md)
