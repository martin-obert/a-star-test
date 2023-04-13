# AStar Test project
[![Tests Only](https://github.com/martin-obert/a-star-test/actions/workflows/run_tests.yml/badge.svg?event=push)](https://github.com/martin-obert/a-star-test/actions/workflows/run_tests.yml)

#Controls
- WASD (arrow keys) for camera movement
- mouse left button to select start/destination cell

## Personal conclusion
This was really nice exercise, where I could finally dig deeper into proper testing. I went to prototype first rather than [Test Driven Development](https://en.wikipedia.org/wiki/Test-driven_development) (TDD), since there was some sort of visual output for this.
In the end I think TDD approach could help me discover most of the flaws with design, like the Humble Objects were introduced almost at the end where I've tried to test the MonoBehavs directly.
I've managed to lift most of the heavy logic to the thread pool and don't block the main UI thread. See: [ParalelFor](https://github.com/martin-obert/a-star-test/blob/35e579da3461ad2d9f75ea1712991ccbf3a377c0/AStartUnity/Assets/Scripts/Runtime/Grid/GridGenerator.cs#L21)

I've added post-processing from [Universal Rendering Pipeline](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@16.0/manual/index.html) (URP) package and custom shader (via Shader Graph) to expose some configuration via editor, like tint color, in more natural way than just some script variable.
The MVVM pattern (mentioned below) allows to expose functionality of the Grid Cell to editor and split responsibility for rendering, animations etc. into individual components, that would listen to the view model changes.

Addressables are just for swag, there is literally no need to add them right now and to be honest I think that excluding them would make this easier, but hey, thats me. The issue with them is, that they need to pre-load or load async at startup which requires all other logic to hold. Also see **Entry scene** below which serves as splash screen.

The next improvement of this project would be some sort of a queue for instancing the cells which now happens in foreach loop and blocks the UI thread effectively.

# About the project

The goal of this project is to visualy validate implementation of a AStar pathfinding strategy. For this we create a grid of a hexagon tiles (cells) from a provided model. The grid size is set upon scene load and can only change after reloading the scene. Each tile also specifies the "travel cost" for the algorithm, this cost is also visually represented as "terrain" and affects the texture of the tile. User should be able to pick staring and destination point by a mouse and see the fastest path between those two points (which doesn't overlap).
From the following description we define following features:
- tile grid
- tile (cell)
- path
- pathfinding
- user input (interaction)
- camera movement


## Layers
**Presentation** Unity API for rendering and capturing user inputs. Also provides periodic tick for frequent logic execution. This layer should be thin as possible, due to difficult unit testing.

**Business** Implements most of the core features logic. This layer must be easily testable and should expose functionality (API) on outside in form of a interfaces. Minimum intersection and dependecy with Unity API here is desired.

**Data** Should only contain arbitrary data describing objects. Furthermore it should be used for serialization. No additional logic should be implemented here.

### Tile (cell)
Since the grid size could consist of many tiles, we should avoid periodic tick on each cell and only reflect immediate changes on each cell to improve performance. Also decoupling from the Unity API as much as possible will be handy, if we want to run performance heavy logic in concerrent environment.
Therefore the MVVM pattern seems most suitable for this situation:
- **Data Model**
 - part of the business layer
 - hold arbitrary data about the cell like: RowID, ColID, Terrain Type, necessary to create / recreate cell
 - can be further used for persistence (load/save) of the layout
- **View Model**
 - part of the business layer
 - implements `IAStarNode` 
 - "live" part of the cell that also holds current state
 - can be observed for changes
 - can be modified by user input or pathfinding
- **View**
 - consists of Unity Components that observe the View Model and reflects the changes
 - this could be for ex: renderer that changes tint of a cell if selected or hovered

### Grid
Grid knows about its cells on the business layer, so it can call methods exposed by the interface.
Injects itself into the DI at **Awake** so it's only available when instantiated in **Hex World** scene.

### Terrain
Terrain exposes:
- travel costs
- texture for visuals
- is walkable flag, to ensure we don't trespass the water for instance

### User input and camera movement
Allows player to hover over cells and select start/destination cell.
Camera movement is cliped within the grid bounds.

### Testing and Moq framework
- added Moq dlls for stubing necessary objects used in test cases
- test are written for most important features like the AStar alg or grid creation

### Humble Object Pattern
is used to separate the actual logic that has to be tested within objects derived from the MonoBehaviours (Components)
See for ex: 
- [GridCellRenderer](AStartUnity/Assets/Scripts/Runtime/Grid/Presenters/GridCellRenderer.cs)
- [GridCellRendererWrapper](AStartUnity/Assets/Scripts/Runtime/Grid/Presenters/GridCellRendererWrapper.cs)

### Facade Pattern
exposes business logic to Unity Editor. This way we can utilize the components via Unity API, either via direct ref editor time or in playmode via `FindObject` methods.
See: [GridCellFacade.cs](AStartUnity/Assets/Scripts/Runtime/Grid/Presenters/GridCellFacade.cs)

### MVVM
only for the cells which are defined as static and doesn't require regular tick per-instance.

See: 
 - [IGridCellViewModel](AStartUnity/Assets/Scripts/Runtime/Grid/Data/IGridCellViewModel.cs)
 - [GridCellViewModel](AStartUnity/Assets/Scripts/Runtime/Grid/Data/GridCellViewModel.cs)

For parts that would have logic executed each update I would use direct reference of some sort of other reactive pattern.

### Repository Pattern
Introduced for all data, that has to be pulled out either from scriptable objects, addressables or file system.
Only knows about the data layer. Repositories then can be simply stubed, when we want to use them in tests.

### Entry scene
- should include minimum of assets, purpose is to have small starting scene (splash screen) that would keep player entertained and executes pre-load logic, like loading addressable on the background
- this will reduce the load time of an app (if all other resources are bundled)
- Personal note: I do this almost in every of my Unity project.

### Dependency Inejction
- this might be my personal issue with static singletons, but DI allowed me to mock 

## 3rd party libraries
- Moq - creation of stubs within the tests
- UniTask - essential, allows better async management
- UniRx - helped to decouple UI from the rest of the systems, mainly used for the Pub/Sub

## Phase 1 - Prototype

- spawn grid from the hexa prefab
- implement hover + select functionality
- implement camera movement
- implemnet MVVM pattern over cells

### Phase 2 - Improvements to next phase

- cell view model should evaluate hover out only when the if the cell is not part of currently ploted path
- **simple req/res pub/sub** - this should decouple the individual systems more
- **use-cases** - wrap complex logic into wholes, this execute highest "gameplay" related flow
