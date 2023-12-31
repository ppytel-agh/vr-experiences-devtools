# Changelog
All notable changes to the webrtc package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [0.1.0] - 2023-07-10

- Initial version

## [0.2.0] - 2023-09-02

### Added

- Initial rail movement system with sample scene
- Pano 360 scene example
- lector panel example
- interactive display prototype
- scenes transition example

## [0.3.0] - 2023-11-08

### Added

- New implementation of pano 360 embedded on scene
- Guided tour with guide prefab
- Guide sample scene
- Lector panel sample scene
- Interactive display sample scene
- Rail locomotion sample scene
- Lector panel prefab

### Changed

- Modifed rails system to create multinode rails
- Updated VRExp XR Rig
- Pano 360 sample scene
- Refactored RailStartAnchor script to get locomotion provider from interactor
- Refactored guided tour to find player by tag

### Fixed

- Missing materials

## [0.4.0] - 2023-12-31

## Added

- Tunneling vignette in scenes transition
- VR Rig Spawner

### Changed

- Interactive Display encapsulates the model inside a boundary
- Scenes transition portal takes the name of gameobject with which the player is aligned in new scene

### Fixed

- Issues with scenes transition system

## [1.0.0] - 2024-01-03

### Added

- Text Content reference in LectorPanel
- Graphics and audio for samples

### Changed

- merged pano-manager with VR rig

### Fixed

- Missing materials issues