# AStar Test project

## Release version

- dynamically create board of terrain hex cells
- each hex cell has randomly picked terrain variation
- each terrain variation will define its properties, such as traveling weight
- player can select cell as starting point and destination
- selected path will be chosen with AStar alg and highlighted

## Inputs
- mouse & keyboard

## Features
### Grid data layer (view-model)
- *Generator* - creates a grid of hex cells of fixed rows/cols (width/height)
### Grid visualizer (view)
- takes generated grid data layer and spawns appropriate 3D objects in the scene
- subscribes on data view model and reflects changing state of the model, like: selection, hover etc.
### Grid navigation
- translates world coords to grid ones
- can retrieve currently pointing grid cell
### Cell selection
- make cell visuals different from others when selected
- the selection should happen by player inputs (mouse click)
### Cell highlight
- hover like functionality that changes cell visuals when player hovers cursor over and resets on cursor exit
- also should be overriden by selection

## Phase 1 - Prototype
[![Tests Only](https://github.com/martin-obert/a-star-test/actions/workflows/run_tests.yml/badge.svg?event=push)](https://github.com/martin-obert/a-star-test/actions/workflows/run_tests.yml)

- automatically generate grid upon scene load
- hover and selection logic of a single cell on the grid
- handle player input, mouse clicks for ex.
- introduce higher level logic manager (game manager for instance) that will impl flow and utilize grid and pathfinding
- manager that should expose Pathfinding logic (AStar calls) and hold current state of a found path

### Improvements to next phase

- cell view model should evaluate hover out only when the if the cell is not part of currently ploted path
- **simple req/res pub/sub** - this should decouple the individual systems more
- **use-cases** - wrap complex logic into wholes, this execute highest "gameplay" related flow
