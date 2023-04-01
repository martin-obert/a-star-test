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
### Grid data layer
- *Generator* - creates a grid of hex cells of fixed rows/cols (width/height)
### Grid visualizer
- takes generated grid data layer and spawns appropriate 3D objects in the scene
### Grid navigation
- *Grid Cursor* - virtual cursor that moves from cel to cell. This will signal what cell we're hovering (or not)
- translates world coords to grid ones
- can retrieve currently pointing grid cell
### Cell selection
### Cell highlight

## Phase 1 - Prototype
[![Editor & PlayMode Tests](https://github.com/martin-obert/a-star-test/actions/workflows/run_tests.yml/badge.svg?event=push)](https://github.com/martin-obert/a-star-test/actions/workflows/run_tests.yml)

- automatically generate grid upon scene load
