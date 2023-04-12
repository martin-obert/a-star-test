# AStar Test project
[![Tests Only](https://github.com/martin-obert/a-star-test/actions/workflows/run_tests.yml/badge.svg?event=push)](https://github.com/martin-obert/a-star-test/actions/workflows/run_tests.yml)

The goal of this project is to visualy validate implementation of a AStar pathfinding strategy. For this we create a grid of a hexagon tiles (cells) from a provided model. The grid size is set upon scene load and can only change after reloading the scene. Each tile also specifies the "travel cost" for the algorithm, this cost is also visually represented as "terrain" and affects the texture of the tile. User should be able to pick staring and destination point by a mouse and see the fastest path between those two points (which doesn't overlap).
From the following description we define following features:
- tile grid
- tile (cell)
- path
- pathfinding
- user input (interaction)
- camera movement

Layers
*Presentation* Unity API for rendering and capturing user inputs. Also provides periodic tick for frequent logic execution. This layer should be thin as possible, due to difficult unit testing.

*Business* Implements most of the core features logic. This layer must be easily testable and should expose functionality (API) on outside in form of a interfaces. Minimum intersection and dependecy with Unity API here is desired.

*Data* Should only contain arbitrary data describing objects. Furthermore it should be used for serialization. No additional logic should be implemented here.

### Tile (cell)
Since the grid size could consist of many tiles, we should avoid periodic tick on each cell and only reflect immediate changes on each cell to improve performance. Also decoupling from the Unity API as much as possible will be handy, if we want to run performance heavy logic in concerrent environment.
Therefore the MVVM pattern seems most suitable for this situation:
- *Data Model*
 - hold arbitrary data about the cell like: RowID, ColID, Terrain Type, necessary to create / recreate cell
 - can be further used for persistence (load/save) of the layout
- *View Model*
 - implements `IAStarNode` 
 - "live" part of the cell that also holds current state
 - can be observed for changes
 - can be modified by user input or pathfinding
- *View*
 - consists of Unity Components that observe the View Model and reflects the changes
 - this could be for ex: renderer that changes tint of a cell if selected or hovered
 
 Since the 
 
## Grid
Grid knows about current cells but only in term 

## User input and camera movement


## Phase 1 - Prototype

- automatically generate grid upon scene load
- hover and selection logic of a single cell on the grid
- handle player input, mouse clicks for ex.
- introduce higher level logic manager (game manager for instance) that will impl flow and utilize grid and pathfinding
- manager that should expose Pathfinding logic (AStar calls) and hold current state of a found path

### Improvements to next phase

- cell view model should evaluate hover out only when the if the cell is not part of currently ploted path
- **simple req/res pub/sub** - this should decouple the individual systems more
- **use-cases** - wrap complex logic into wholes, this execute highest "gameplay" related flow
